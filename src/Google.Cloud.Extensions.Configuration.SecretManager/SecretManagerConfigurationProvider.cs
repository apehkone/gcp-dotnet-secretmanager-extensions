using Google.Api.Gax.ResourceNames;
using Google.Cloud.SecretManager.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Google.Cloud.Extensions.Configuration.SecretManager
{
    public class SecretManagerConfigurationProvider : ConfigurationProvider
    {
        private readonly SecretManagerServiceClient client;
        private readonly ProjectName projectName;
        private readonly ILogger<SecretManagerConfigurationProvider> logger;

        public SecretManagerConfigurationProvider(SecretManagerServiceClient client, SecretManagerConfigurationSource configurationSource)
        {
            this.client = client;
            projectName = new ProjectName(configurationSource.ProjectName);
            logger = configurationSource.LoggerFactory.CreateLogger<SecretManagerConfigurationProvider>();
        }

        public override void Load()
        {
            logger.LogDebug($"Loading secrets from project: {projectName}");
          
            var secrets = client.ListSecrets(projectName);

            foreach (var secret in secrets)
            {
                var secretVersionName = new SecretVersionName(secret.SecretName.ProjectId, secret.SecretName.SecretId, "latest");
                
                logger.LogDebug($"Loading secret: {secretVersionName}");
                
                var version = client.AccessSecretVersion(secretVersionName);

                var payload = version.Payload.Data.ToStringUtf8();
                
                Data.Add(secret.SecretName.SecretId, payload);
            }
        }
    }
}