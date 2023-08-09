namespace Integrations.Common
{
    public class IntegrationException : Exception
    {
        public IntegrationException(string? message) : base(message)
        {
        }
    }
}