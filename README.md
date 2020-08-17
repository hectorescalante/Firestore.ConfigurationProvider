# Firestore.ConfigurationProvider
.Net Core Configuration Provider that allows developers to use Google Cloud Firestore as a Configuration Source in their applications.

[![Build status](https://dev.azure.com/hectorescalante/Github%20Projects/_apis/build/status/Firestore.ConfigurationProvider)](https://dev.azure.com/hectorescalante/Github%20Projects/_build/latest?definitionId=4)

## FirestoreConfigurationOptions

| Setting | Default Value | Description |
| ------- | ------------- | ----------- |
| FIRESTORECONFIG_ENABLED | true | Enable or disable configuration load |
| FIRESTORECONFIG_PROJECTID | "" | The google cloud project identifier where the firestore service exists |
| FIRESTORECONFIG_APPLICATION | AppDomain.CurrentDomain.FriendlyName | Name of the application  |
| FIRESTORECONFIG_STAGE | ASPNETCORE_ENVIRONMENT | Name of the current environment  |
| FIRESTORECONFIG_TAG | "Default" | Useful for application versioning, blue-green deployment or any other stage subclassification |

## Add Firestore Configuration in Program class

```
  public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostContext, config) =>
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
The provider will be listening for changes in these documents:

1. ApplicationSettings/{applicationName}
2. ApplicationSettings/{applicationName}/Stages/{stageName}
3. ApplicationSettings/{applicationName}/Stages/{stageName}/Machines/{machineName}
3. ApplicationSettings/{applicationName}/Stages/{stageName}/Tags/{tag}

When a change is made in any document, the provider reload the configuration in order from general to particular (Application -> Stage -> Machine -> Tag).

> Machines and Tags collections are at stage level so when you configure both the tag configuration overrides the machine configuration.

## Usage
Since the settings are loaded from Firestore into the Application Configuration they will be available through the IConfiguration interface at Startup, just like the settings declared in environment variables or appsettings.json file.
