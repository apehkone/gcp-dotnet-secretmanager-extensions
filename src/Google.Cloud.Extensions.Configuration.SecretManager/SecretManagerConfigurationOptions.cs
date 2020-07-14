using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Google.Cloud.Extensions.Configuration.SecretManager
{
    public class SecretManagerConfigurationOptions
    {
        public string ProjectName { get; set; }

        public ILoggerFactory LoggerFactory { get; set; } = NullLoggerFactory.Instance;
    }
}