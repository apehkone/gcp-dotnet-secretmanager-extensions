# Introduction #

This library simplifies GCP SecretManager usage in .NET Core by creating a custom configuration provider. The configuration provider allows loading application configuration from the GCP Secret Manager in a same manner as from a local settings file and then read the data as key-value pairs by using IConfiguration as a source. 

## ASP.NET CORE example:

```csharp
IConfiguration config = new ConfigurationBuilder()
    .AddSecretManager(c => {
        c.ProjectName = "GOOGLE_PROJECT_ID";
    })
    .Build();
```

## Authentication ## 

The underlying Google Client library requires a service account that is typically stored in a JSON file. The authentication checks **GOOGLE_APPLICATION_CREDENTIALS** environment variable that should point to the service account file. 

More information about the GCP authentication [here](https://cloud.google.com/docs/authentication/production) 


## ASP.NET Configuration providers ## 

See https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1
