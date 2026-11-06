#Requires -Version 7.4
$ErrorActionPreference = 'Stop'

function Assert($condition, $message) {
    if (!$condition) { throw $message }
}

function Get-LoopbackPort {
    $listener = [Net.Sockets.TcpListener]::new([Net.IPAddress]::Loopback, 0)
    $listener.Start()
    try { $listener.LocalEndpoint.Port }
    finally { $listener.Stop() }
}

function Start-App($name, $url, $appEnvironment) {
    $dll = Join-Path $repo "$name/bin/Debug/net10.0/$name.dll"
    Assert (Test-Path -LiteralPath $dll) "Build PetCareHub.sln before running this script."
    Start-Process -FilePath dotnet -ArgumentList "`"$dll`" --urls $url" -WindowStyle Hidden -PassThru `
        -WorkingDirectory (Join-Path $repo $name) -Environment $appEnvironment
}

function Wait-ForHttp($url, $process) {
    for ($attempt = 0; $attempt -lt 60; $attempt++) {
        if ($process.HasExited) { throw "App exited before serving $url" }
        try {
            $response = Invoke-WebRequest $url -TimeoutSec 1
            if ($response.StatusCode -eq 200) { return }
        } catch { Start-Sleep -Milliseconds 250 }
    }
    throw "Timed out waiting for $url"
}

function Read-Token($response) {
    $token = [regex]::Match($response.Content, 'name="__RequestVerificationToken"[^>]*value="([^"]+)"').Groups[1].Value
    Assert $token 'The form did not render an antiforgery token.'
    $token
}

$repo = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$database = Join-Path ([IO.Path]::GetTempPath()) "petcarehub-smoke-$([guid]::NewGuid()).db"
$apiUrl = "http://127.0.0.1:$(Get-LoopbackPort)"
$webUrl = "http://127.0.0.1:$(Get-LoopbackPort)"
$apiEnvironment = @{ ConnectionStrings__PetCareHub = "Data Source=$database" }
$webEnvironment = @{ ApiSettings__BaseUrl = "$apiUrl/" }
$api = $null
$web = $null

try {
    $api = Start-App 'PetCareHub.Api' $apiUrl $apiEnvironment
    Wait-ForHttp "$apiUrl/api/pets" $api
    $web = Start-App 'PetCareHub.WebApp' $webUrl $webEnvironment
    Wait-ForHttp "$webUrl/" $web

    $dashboard = Invoke-WebRequest "$webUrl/"
    Assert ($dashboard.Content -match 'Total Pets' -and $dashboard.Content -match 'Upcoming Reminders') 'Dashboard totals are missing.'

    $petName = "Smoke Pet $([guid]::NewGuid())"
    $form = Invoke-WebRequest "$webUrl/Pets/Create" -SessionVariable browser
    $petFields = @{ Name = $petName; Species = 'Ferret'; Gender = 'Unknown'; PhotoUrl = 'https://example.test/pet.jpg' }
    $blocked = Invoke-WebRequest "$webUrl/Pets/Create" -Method Post -Body $petFields -WebSession $browser -SkipHttpErrorCheck
    Assert ($blocked.StatusCode -eq 400) 'A write without an antiforgery token succeeded.'
    $petFields.__RequestVerificationToken = Read-Token $form
    $petsPage = Invoke-WebRequest "$webUrl/Pets/Create" -Method Post -Body $petFields -WebSession $browser
    Assert ($petsPage.Content.Contains($petName)) 'The created pet is missing from the list.'
    $pet = (Invoke-RestMethod "$apiUrl/api/pets").data | Where-Object name -eq $petName
    $details = Invoke-WebRequest "$webUrl/Pets/Details/$($pet.id)"
    Assert ($details.Content.Contains($petName)) 'The pet details are missing.'
    $missing = Invoke-WebRequest "$webUrl/Pets/Details/$([guid]::NewGuid())"
    Assert ($missing.BaseResponse.RequestMessage.RequestUri.AbsolutePath -eq '/Pets') 'Missing pet did not redirect to the list.'

    $petEditUrl = "$webUrl/Pets/Edit/$($pet.id)"
    Assert ($details.Content.Contains("/Pets/Edit/$($pet.id)")) 'Pet Details did not link to Edit.'
    $petEdit = Invoke-WebRequest $petEditUrl -WebSession $browser
    Assert ($petEdit.Content -match 'value="Ferret" selected' -and $petEdit.Content -match 'value="Unknown" selected' -and
        $petEdit.Content.Contains('https://example.test/pet.jpg') -and $petEdit.Content.Contains($petName)) 'Pet Edit did not prefill the stored profile.'
    $editedPetName = "$petName edited"
    $petEditFields = @{
        Name = $editedPetName
        Species = 'Ferret'
        Gender = 'Unknown'
        Notes = 'Edited pet notes'
        DateOfBirth = '2020-03-04'
        PhotoUrl = $pet.photoUrl
    }
    $blockedEdit = Invoke-WebRequest $petEditUrl -Method Post -Body $petEditFields -WebSession $browser -SkipHttpErrorCheck
    Assert ($blockedEdit.StatusCode -eq 400) 'Pet Edit accepted a POST without an antiforgery token.'
    $petEditFields.__RequestVerificationToken = Read-Token $petEdit
    $invalidPetEdit = $petEditFields.Clone()
    $invalidPetEdit.DateOfBirth = 'invalid-date'
    $rejectedPetEdit = Invoke-WebRequest $petEditUrl -Method Post -Body $invalidPetEdit -WebSession $browser
    $storedPet = (Invoke-RestMethod "$apiUrl/api/pets/$($pet.id)").data
    Assert ($rejectedPetEdit.Content -match 'validation-summary-errors' -and $rejectedPetEdit.Content.Contains($editedPetName) -and
        $storedPet.name -eq $petName) 'Invalid pet edits failed to retain input or changed the stored pet.'
    $updatedPetDetails = Invoke-WebRequest $petEditUrl -Method Post -Body $petEditFields -WebSession $browser
    Assert ($updatedPetDetails.BaseResponse.RequestMessage.RequestUri.AbsolutePath -eq "/Pets/Details/$($pet.id)" -and
        $updatedPetDetails.Content.Contains($editedPetName) -and $updatedPetDetails.Content.Contains('Edited pet notes')) 'Pet Edit did not save and return to Details.'
    $missingPetEdit = Invoke-WebRequest "$webUrl/Pets/Edit/$([guid]::NewGuid())" -SkipHttpErrorCheck
    Assert ($missingPetEdit.StatusCode -eq 404) 'Pet Edit did not return 404 for a missing pet.'

    $form = Invoke-WebRequest "$webUrl/Reminders/Create" -WebSession $browser
    $reminderFields = @{
        PetId = $pet.id
        Type = 'Checkup'
        Title = 'Smoke checkup'
        __RequestVerificationToken = Read-Token $form
    }
    $invalid = Invoke-WebRequest "$webUrl/Reminders/Create" -Method Post -Body $reminderFields -WebSession $browser
    Assert ($invalid.Content -match 'validation-summary-errors') 'The missing due date did not render a validation error.'
    $invalidApi = Invoke-RestMethod "$apiUrl/api/reminders" -Method Post -ContentType 'application/json' -SkipHttpErrorCheck -StatusCodeVariable invalidStatus -Body (
        @{ petId = $pet.id; type = 'Checkup'; title = 'Missing date' } | ConvertTo-Json
    )
    Assert ($invalidStatus -eq 400 -and $invalidApi.message -eq 'Due date is required.') 'The API accepted a missing due date.'

    $reminderFields.DueDate = [DateTime]::UtcNow.AddDays(30).ToString('yyyy-MM-dd')
    $reminderFields.Notes = 'Original notes'
    $list = Invoke-WebRequest "$webUrl/Reminders/Create" -Method Post -Body $reminderFields -WebSession $browser
    Assert ($list.Content.Contains('[Checkup] Smoke checkup')) 'The 30-day reminder is missing from the pending list.'
    $reminder = (Invoke-RestMethod "$apiUrl/api/reminders/pending").data | Where-Object petId -eq $pet.id
    $before = (Invoke-RestMethod "$apiUrl/api/reminders/upcoming").data
    Assert ($reminder.type -eq 'Checkup' -and $reminder.id -notin $before.id) 'The 30-day reminder leaked into upcoming.'

    $editUrl = "$webUrl/Reminders/Edit/$($reminder.id)"
    Assert ($list.Content.Contains("/Reminders/Edit/$($reminder.id)")) 'The pending list is missing its Edit link.'
    $edit = Invoke-WebRequest $editUrl -WebSession $browser
    Assert ($edit.Content.Contains('value="Smoke checkup"') -and $edit.Content.Contains('Original notes') -and
        $edit.Content.Contains($reminderFields.DueDate) -and $edit.Content -match 'value="Checkup" selected') 'The edit form did not prefill the stored fields.'

    $fields = @{
        Title = 'Rescheduled checkup'
        Type = 'Medicine'
        Notes = 'Updated notes'
        __RequestVerificationToken = Read-Token $edit
    }
    $badEdit = Invoke-WebRequest $editUrl -Method Post -Body $fields -WebSession $browser
    Assert ($badEdit.Content -match 'validation-summary-errors' -and $badEdit.Content.Contains('value="Rescheduled checkup"')) 'Invalid edits did not retain input and show feedback.'

    $editApi = "$apiUrl/api/reminders/$($reminder.id)"
    $badType = @{ title = 'Bad edit'; type = 'Invalid'; dueDate = $reminderFields.DueDate } | ConvertTo-Json
    $rejected = Invoke-WebRequest $editApi -Method Put -ContentType 'application/json' -Body $badType -SkipHttpErrorCheck
    $stored = (Invoke-RestMethod $editApi).data
    Assert ($rejected.StatusCode -eq 400 -and $stored.title -eq 'Smoke checkup' -and $stored.notes -eq 'Original notes') 'Invalid edits changed the stored reminder.'

    $fields.DueDate = [DateTime]::UtcNow.AddDays(1).ToString('yyyy-MM-dd')
    $list = Invoke-WebRequest $editUrl -Method Post -Body $fields -WebSession $browser
    Assert ($list.Content.Contains('[Medicine] Rescheduled checkup') -and $list.Content.Contains('Reminder updated.')) 'The saved edit did not appear in the pending list.'
    $upcoming = (Invoke-RestMethod "$apiUrl/api/reminders/upcoming").data
    $updated = $upcoming | Where-Object id -eq $reminder.id
    $dashboard = Invoke-WebRequest "$webUrl/"
    Assert ($updated.title -eq '[Medicine] Rescheduled checkup' -and $updated.petId -eq $pet.id -and
        $dashboard.Content.Contains('Rescheduled checkup')) 'The updated reminder did not appear on the dashboard.'

    $completeFields = @{ id = $reminder.id; __RequestVerificationToken = Read-Token $list }
    $done = Invoke-WebRequest "$webUrl/Reminders/Complete" -Method Post -Body $completeFields -WebSession $browser
    Assert ($done.Content.Contains('Reminder marked as completed.') -and !$done.Content.Contains('Rescheduled checkup')) 'Completion did not remove the reminder.'
    $retry = Invoke-WebRequest "$webUrl/Reminders/Complete" -Method Post -Body $completeFields -WebSession $browser
    Assert ($retry.Content.Contains('Reminder is already completed.')) 'Repeat completion did not show the API error.'

    $fields.Title = 'Stale edit'
    $stale = Invoke-WebRequest $editUrl -Method Post -Body $fields -WebSession $browser
    Assert ($stale.Content.Contains('Pending reminder not found.')) 'The edit form saved after the reminder was completed.'

    $racingReminder = Invoke-RestMethod "$apiUrl/api/reminders" -Method Post -ContentType 'application/json' -Body (
        @{ petId = $pet.id; type = 'Custom'; title = 'Concurrent completion'; dueDate = $reminderFields.DueDate } | ConvertTo-Json
    )
    $client = [Net.Http.HttpClient]::new()
    try {
        $completeUrl = "$apiUrl/api/reminders/$($racingReminder.data.id)/complete"
        $first = $client.PostAsync($completeUrl, $null)
        $second = $client.PostAsync($completeUrl, $null)
        $statuses = @([int]$first.GetAwaiter().GetResult().StatusCode, [int]$second.GetAwaiter().GetResult().StatusCode)
        Assert (200 -in $statuses -and 400 -in $statuses) 'Concurrent completion did not produce one success and one rejection.'
    } finally { $client.Dispose() }

    $persistedReminder = Invoke-RestMethod "$apiUrl/api/reminders" -Method Post -ContentType 'application/json' -Body (
        @{ petId = $pet.id; type = 'Checkup'; title = 'Before restart'; dueDate = $reminderFields.DueDate } | ConvertTo-Json
    )
    $persistedEditApi = "$apiUrl/api/reminders/$($persistedReminder.data.id)"
    $persistedEdit = @{ title = 'Persisted edit'; type = 'Medicine'; notes = 'Persisted notes'; dueDate = $fields.DueDate } | ConvertTo-Json
    Invoke-RestMethod $persistedEditApi -Method Put -ContentType 'application/json' -Body $persistedEdit | Out-Null

    Stop-Process -InputObject $api
    $api.WaitForExit()
    $api = Start-App 'PetCareHub.Api' $apiUrl $apiEnvironment
    Wait-ForHttp "$apiUrl/api/pets" $api
    $persisted = (Invoke-RestMethod "$apiUrl/api/pets/$($pet.id)").data
    Assert ($persisted.name -eq $editedPetName -and $persisted.notes -eq 'Edited pet notes' -and
        $persisted.species -eq 'Ferret' -and $persisted.gender -eq 'Unknown' -and
        $persisted.photoUrl -eq 'https://example.test/pet.jpg' -and
        ([datetime]$persisted.dateOfBirth).ToString('yyyy-MM-dd') -eq '2020-03-04') 'The edited pet profile did not survive an API restart.'
    $stored = (Invoke-RestMethod $persistedEditApi).data
    Assert ($stored.title -eq 'Persisted edit' -and $stored.type -eq 'Medicine' -and $stored.notes -eq 'Persisted notes' -and
        ([datetime]$stored.dueDate).ToString('yyyy-MM-dd') -eq $fields.DueDate) 'The reminder edit did not survive restart.'
    $retryAfterRestart = Invoke-RestMethod "$apiUrl/api/reminders/$($reminder.id)/complete" -Method Post -SkipHttpErrorCheck -StatusCodeVariable retryStatus
    Assert ($retryStatus -eq 400 -and $retryAfterRestart.message -eq 'Reminder is already completed.') 'Completion did not survive an API restart.'
    $completedEdit = Invoke-WebRequest $editApi -Method Put -ContentType 'application/json' -Body $persistedEdit -SkipHttpErrorCheck
    Assert ($completedEdit.StatusCode -eq 400) 'Completed reminder accepted an edit after restart.'
    Write-Output 'MVP smoke checks passed.'
}
finally {
    foreach ($process in @($web, $api)) {
        if ($null -ne $process -and !$process.HasExited) {
            Stop-Process -InputObject $process
            $process.WaitForExit()
        }
    }
    Remove-Item -LiteralPath $database, "$database-shm", "$database-wal", "$database-journal" -ErrorAction SilentlyContinue
}