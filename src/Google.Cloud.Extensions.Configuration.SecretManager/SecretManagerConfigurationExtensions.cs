// ReSharper disable once CheckNamespace

using System;
using Google.Cloud.Extensions.Configuration.SecretManager;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Configuration
{
    public static class SecretManagerConfigurationExtensions
    {
        public static IConfigurationBuilder AddSecretManager(this IConfigurationBuilder builder,
            Action<SecretManagerConfigurationOptions> options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            var configurationOptions = new SecretManagerConfigurationOptions();
            options(configurationOptions);

            return builder.Add(new SecretManagerConfigurationSource(configurationOptions));
        }
    }
}