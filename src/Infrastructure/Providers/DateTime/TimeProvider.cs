using System;

namespace Infrastructure.Providers.DateTime
{
    public abstract class TimeProvider
    {
        private static TimeProvider _current =
            DefaultTimeProvider.Instance;

        public static TimeProvider Current
        {
            get => TimeProvider._current;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                TimeProvider._current = value;
            }
        }

        public abstract System.DateTime UtcNow { get; }

        public static void ResetToDefault()
        {
            TimeProvider._current = DefaultTimeProvider.Instance;
        }
    }
}