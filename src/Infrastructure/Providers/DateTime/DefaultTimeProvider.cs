namespace Infrastructure.Providers.DateTime
{
    public class DefaultTimeProvider : TimeProvider
    {

        private static readonly DefaultTimeProvider _Instance =
            new DefaultTimeProvider();

        private DefaultTimeProvider()
        {
        }

        public override System.DateTime UtcNow => System.DateTime.UtcNow;

        public static DefaultTimeProvider Instance => DefaultTimeProvider._Instance;
    }
}