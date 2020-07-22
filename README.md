# Firestore.ConfigurationProvider
.Net Core Configuration Provider that allows developers to use Google Cloud Firestore as a Configuration Source in their applications.

## FirestoreConfigurationOptions

| Setting | Default Value | Description | Required |
| ------- | ------------- | ----------- | -------- |
| FIRESTORECONFIG_ENABLED | true | Enable or disable configuration load | no |
| FIRESTORECONFIG_PROJECTID | "" | The google cloud project identifier where the firestore service exists | yes |
| FIRESTORECONFIG_APPLICATION | AppDomain.CurrentDomain.FriendlyName | Name of the application  | no |
| FIRESTORECONFIG_STAGE | ASPNETCORE_ENVIRONMENT | Name of the current environment  | yes |
| FIRESTORECONFIG_MACHINE | Environment.MachineName | Name of the current host | no |

## Add Firestore Configuration in Program class
```
  public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hotContext, config) =>
            {
              config.AddFirestoreConfiguration();
            })            
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            });
  }
```

## Usage
Since the settings are loaded from Firestore into the Application Configuration they will be available through the IConfiguration interface at Startup, just like environment variables or the appsettings.json file.
