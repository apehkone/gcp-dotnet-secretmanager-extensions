# Introduction #

This library simplifies GCP SecretManager usage in .NET Core by creating a custom configuration provider. The configuration provider allows fetching application configuration from the SecretManager in a same manner as it is normally fetched from appSettings file. 


## ASP.NET CORE example:

```csharp
IConfiguration config = new ConfigurationBuilder()
    .AddSecretManager(c => {
        c.ProjectName = "GOOGLE_PROJECT_ID";
    })
    .Build();
```

