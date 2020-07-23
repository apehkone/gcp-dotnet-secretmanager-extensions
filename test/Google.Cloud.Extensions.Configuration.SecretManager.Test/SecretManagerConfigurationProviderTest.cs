using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Api.Gax;
using Google.Api.Gax.Grpc;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.SecretManager.V1;
using Google.Protobuf;
using Grpc.Core;
using Moq;
using Xunit;

namespace Google.Cloud.Extensions.Configuration.SecretManager.Test
{
    public class SecretManagerConfigurationProviderTest
    {
        private const string PROJECT_NAME = "gcp-mock-project";

        [Fact]
        public void ShouldInstantiateConfigurationProvider()
        {
            var mockGrpcClient = new Mock<SecretManagerServiceClient>();

            mockGrpcClient
                .Setup(e => e.ListSecretsAsync(new ProjectName(PROJECT_NAME), null, null, null))
                .Returns(new MockPagedEnumerable());
            
            
            var secretResponse = new AccessSecretVersionResponse() {
                Payload = new SecretPayload{
                    Data = ByteString.FromBase64( System.Convert.ToBase64String(Encoding.UTF8.GetBytes("value")) )}};
            
            mockGrpcClient
                .Setup(e => e.AccessSecretVersionAsync(new SecretVersionName(PROJECT_NAME, "param1", "latest"), null))
                .Returns(Task.FromResult(secretResponse));
            
            var configurationOptions = new SecretManagerConfigurationOptions {ProjectName = PROJECT_NAME};
            var configurationSource = new SecretManagerConfigurationSource(configurationOptions);
            
            var provider = new SecretManagerConfigurationProvider(mockGrpcClient.Object, configurationSource);
            
            provider.Load();

            Assert.True(provider.TryGet("param1", out var value));
            Assert.Equal("value", value);
        }

        private class MockPagedEnumerable : PagedAsyncEnumerable<ListSecretsResponse, Secret>
        {
            public override IAsyncEnumerator<Secret> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
            {
                var result = new List<Secret> {new Secret {SecretName = new SecretName(PROJECT_NAME, "param1")}};
                return result.ToAsyncEnumerable().GetAsyncEnumerator(cancellationToken);

            }
        }
    }
  
}
