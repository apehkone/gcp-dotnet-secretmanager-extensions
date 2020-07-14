using Google.Cloud.SecretManager.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Google.Cloud.Extensions.Configuration.SecretManager
{
    public class SecretManagerConfigurationSource : IConfigurationSource
    {
        public SecretManagerConfigurationSource(SecretManagerConfigurationOptions options)
        {
            ProjectName = options.ProjectName;
            LoggerFactory = options.LoggerFactory;
        }

        public string ProjectName { get; }

        public ILoggerFactory LoggerFactory { get; }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new SecretManagerConfigurationProvider(SecretManagerServiceClient.Create(),  this);
        }
    }
}