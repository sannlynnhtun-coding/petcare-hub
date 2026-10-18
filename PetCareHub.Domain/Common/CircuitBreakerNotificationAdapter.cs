namespace PetCareHub.Domain.Common;

// Circuit Breaker Pattern - simple version for learning
public class CircuitBreakerNotificationAdapter : INotificationAdapter
{
    private readonly INotificationAdapter _inner;
    private int _failureCount;
    private DateTime _lastFailureTime;
    private bool _isOpen;

    private const int FailureThreshold = 3;
    private static readonly TimeSpan OpenDuration = TimeSpan.FromSeconds(30);

    public CircuitBreakerNotificationAdapter(INotificationAdapter inner)
    {
        _inner = inner;
    }

    public async Task<bool> SendAsync(string message)
    {
        if (_isOpen)
        {
            if (DateTime.UtcNow - _lastFailureTime > OpenDuration)
            {
                // Half-open: try again
                _isOpen = false;
                _failureCount = 0;
            }
            else
            {
                Console.WriteLine("[CircuitBreaker] OPEN - request blocked");
                return false;
            }
        }

        try
        {
            var result = await _inner.SendAsync(message);
            _failureCount = 0;
            return result;
        }
        catch
        {
            _failureCount++;
            _lastFailureTime = DateTime.UtcNow;

            if (_failureCount >= FailureThreshold)
            {
                _isOpen = true;
                Console.WriteLine("[CircuitBreaker] OPENED due to failures");
            }

            return false;
        }
    }
}
