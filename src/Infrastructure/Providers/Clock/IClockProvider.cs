using System;

namespace Infrastructure.Providers.Clock
{
    public interface IClockProvider
    {
        DateTimeOffset UtcNow();
        DateTime Now();
    }
}