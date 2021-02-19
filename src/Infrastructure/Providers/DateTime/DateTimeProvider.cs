using System;

namespace Infrastructure.Providers.DateTime
{
    public class DateTimeProvider : IDateTimeProvider
    {

        private readonly TimeSpan _difference;
        
        public DateTimeProvider()
        {
            _difference = TimeSpan.Zero;
        }

        public DateTimeProvider(DateTimeOffset currentDate)
        {
            _difference = currentDate - DateTimeOffset.UtcNow;
        }
        
        public DateTimeOffset UtcNow()
        {
            return DateTimeOffset.UtcNow + _difference;
        }
    }
}