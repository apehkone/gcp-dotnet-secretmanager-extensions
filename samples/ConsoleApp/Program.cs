using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ConsoleApp
{
    class Program
    {
        private static void Main(string[] args)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("Google.Cloud.Extensions.Configuration.SecretManager.SecretManagerConfigurationProvider", LogLevel.Debug)
                    .AddConsole();
            });

            IConfiguration config = new ConfigurationBuilder()
                .AddSecretManager(c => {
                        c.ProjectName = Environment.GetEnvironmentVariable("GOOGLE_PROJECT_ID");
                        c.LoggerFactory = loggerFactory;
                    }
                )
                .Build();

            Console.Write($"Param1 value is {config["param1"]}");
        }
    }
}