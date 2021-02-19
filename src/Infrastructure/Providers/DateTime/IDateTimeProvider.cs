using System;

namespace Infrastructure.Providers.DateTime
{
    public interface IDateTimeProvider
    {
        DateTimeOffset UtcNow();
    }
}