# Firestore.ConfigurationProvider
.Net Core Configuration Provider that allows developers to use Google Cloud Firestore as a Configuration Source in their applications.

[![Build status](https://dev.azure.com/hectorescalante/Github%20Projects/_apis/build/status/Firestore.ConfigurationProvider)](https://dev.azure.com/hectorescalante/Github%20Projects/_build/latest?definitionId=4)

## FirestoreConfigurationOptions

| Setting | Default Value | Description | Required |
| ------- | ------------- | ----------- | -------- |
| FIRESTORECONFIG_ENABLED | true | Enable or disable configuration load | no |
| FIRESTORECONFIG_PROJECTID | "" | The google cloud project identifier where the firestore service exists | yes |
| FIRESTORECONFIG_APPLICATION | AppDomain.CurrentDomain.FriendlyName | Name of the application  | no |
| FIRESTORECONFIG_STAGE | ASPNETCORE_ENVIRONMENT | Name of the current environment  | yes |

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

## Automatic Settings Upload
For the first time, automatically load the appsettings.json file into a Firestore Document named as the application and also a Stages collection will be created with an empty Document named as the current environment.

## Change Detection
The provider will be listening for changes in three documents:

1. ApplicationSettings/{applicationName}
2. ApplicationSettings/{applicationName}/Stages/{stageName}
3. ApplicationSettings/{applicationName}/Stages/{stageName}/Machines/{machineName}

When a change is made in any of the three documents the provider reload the configuration in order from general to particular (Application -> Stage -> Machine).

## Usage
Since the settings are loaded from Firestore into the Application Configuration they will be available through the IConfiguration interface at Startup, just like environment variables or the appsettings.json file.
