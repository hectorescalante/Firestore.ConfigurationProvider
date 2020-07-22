using Firestore.ConfigurationProvider.Core;
using Firestore.ConfigurationProvider.Infrastructure;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

[assembly: InternalsVisibleTo("Firestore.ConfigurationProvider.Test")]
namespace Firestore.ConfigurationProvider
{
  internal class FirestoreConfigurationProvider : JsonStreamConfigurationProvider
  {
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger _logger;
    private ApplicationSettingsManager _applicationSettings;
    private readonly FirestoreConfigurationOptions _configurationOptions;
    private static readonly Mutex _mutex = new Mutex();

    public FirestoreConfigurationProvider(FirestoreConfigurationSource source) : base(source)
    {
      _loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
      _logger = _loggerFactory.CreateLogger<FirestoreConfigurationProvider>();
      _configurationOptions = new FirestoreConfigurationOptions(_loggerFactory.CreateLogger<FirestoreConfigurationOptions>());
    }

    public override void Load()
    {
      if (_configurationOptions.IsEnabled)
      {
        _logger.LogInformation($"Loading remote configuration... {DateTime.Now}");
        var asmLogger = _loggerFactory.CreateLogger<ApplicationSettingsManager>();
        var fcmLogger = _loggerFactory.CreateLogger<FirestoreConnectionManager>();
        _applicationSettings = new ApplicationSettingsManager(asmLogger, _configurationOptions, new FirestoreConnectionManager(fcmLogger, _configurationOptions), new FileManager());
        _applicationSettings.Setup().Wait();
        _applicationSettings.CreateListeners(JsonSettingsToDictionary, ReloadSettings);
      }
      else
      {
        _logger.LogInformation("Remote configuration is disabled!");
      }
    }

    public IDictionary<string, string> JsonSettingsToDictionary(string jsonSettings)
    {
      try
      {
        _logger.LogInformation($"Loading {jsonSettings}");
        Load(new MemoryStream(Encoding.UTF8.GetBytes(jsonSettings)));
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, ex.Message);
        throw ex;
      }

      return Data;
    }

    public void ReloadSettings(ConcurrentDictionary<string, string> remoteSettingsData)
    {
      _mutex.WaitOne();
      //Assign the previous collected keys from all levels to the final Data dictionary.
      foreach (var item in remoteSettingsData)
      {
        if (Data.ContainsKey(item.Key)) { Data[item.Key] = item.Value; } else { Data.Add(item); };
      }
      _mutex.ReleaseMutex();

      //Refresh change token.
      _logger.LogInformation("Refreshing token...");
      OnReload();
    }
  }
}