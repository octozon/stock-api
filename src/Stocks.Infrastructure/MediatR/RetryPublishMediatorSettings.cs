namespace Stocks.Infrastructure.MediatR
{
    public class RetryPublishMediatorSettings
    {
        public bool Enabled { get; set; }
        
        public int RetryAttempts { get; set; }

        public bool RetryWithExponentialBackoff { get; set; }

        public double RetryDelay { get; set; }
    }
}