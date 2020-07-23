using System.Threading.Tasks;
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

        public override void Load() => LoadAsync().ConfigureAwait(false).GetAwaiter().GetResult();

        private async Task LoadAsync()
        {
            logger.LogDebug($"Loading secrets from project: {projectName}");
          
            var secrets = client.ListSecretsAsync(projectName);

            await foreach (var secret in secrets)
            {
                var secretVersionName = new SecretVersionName(secret.SecretName.ProjectId, secret.SecretName.SecretId, "latest");
                
                logger.LogDebug($"Loading secret: {secretVersionName}");
                
                var version = await client.AccessSecretVersionAsync(secretVersionName);

                var payload = version.Payload.Data.ToStringUtf8();
                
                Data.Add(secret.SecretName.SecretId, payload);
            }
        }
    }
}