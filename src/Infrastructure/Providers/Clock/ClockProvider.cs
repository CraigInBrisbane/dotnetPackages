using System;

namespace Infrastructure.Providers.Clock
{
    public class ClockProvider : IClockProvider
    {

        private readonly TimeSpan _difference;
        
        public ClockProvider()
        {
            _difference = TimeSpan.Zero;
        }

        public ClockProvider(DateTimeOffset currentDate)
        {
            _difference = currentDate - DateTimeOffset.UtcNow;
        }
        
        public DateTimeOffset UtcNow()
        {
            return DateTimeOffset.UtcNow + _difference;
        }

        public DateTime Now()
        {
            return DateTime.Now + _difference;
        }
    }
}