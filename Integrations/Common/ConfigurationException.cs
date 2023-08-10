namespace Integrations
{
    public static partial class DependancyInjection 
    {
        public class ConfigurationException : Exception
        {
            public ConfigurationException(string message) : base(message)
            {
            }
        }
    }
}