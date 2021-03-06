﻿using Google.Cloud.Firestore;
using Firestore.ConfigurationProvider.Core;
using Firestore.ConfigurationProvider.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Firestore.ConfigurationProvider.Infrastructure
{
  internal class FirestoreConnectionManager : IFirestoreConnectionManager
  {
    private static readonly ConcurrentDictionary<string, FirestoreDb> _firestoreClients = new ConcurrentDictionary<string, FirestoreDb>();
    private readonly ILogger _logger;
    private readonly FirestoreConfigurationOptions _options;

    public FirestoreDb FirestoreClient { get; private set; }
    public Dictionary<ConfigurationLevels, DocumentReference> ConfigurationDocuments { get; private set; } = new Dictionary<ConfigurationLevels, DocumentReference>();
    public Dictionary<ConfigurationLevels, FirestoreChangeListener> ConfigurationListeners { get; private set; } = new Dictionary<ConfigurationLevels, FirestoreChangeListener>();

    public FirestoreConnectionManager(ILogger logger, FirestoreConfigurationOptions options)
    {
      _logger = logger;
      _options = options;
    }

    public void Setup()
    {
      _logger.LogDebug($"Creating client for {_options.ProjectId}...");
      FirestoreClient = _firestoreClients.GetOrAdd(_options.ProjectId, value => { return FirestoreDb.Create(_options.ProjectId); });
    }

    public async Task<bool> IsDocumentEmptyAsync(string documentPath)
    {
      var document = FirestoreClient.Document(documentPath);
      var snapshot = await document.GetSnapshotAsync();
      return (!snapshot.Exists || snapshot.ToDictionary().Count == 0);
    }

    public async Task SaveAsync(string documentPath, Dictionary<string, object> documentFields)
    {
      var document = FirestoreClient.Document(documentPath);
      await document.SetAsync(documentFields);
    }

    public void CreateListeners(Func<ConfigurationLevels, string, Task> loadOnChangeAsyncCallback)
    {
      ConfigurationDocuments.Add(ConfigurationLevels.Application, FirestoreClient.Document(_options.GetApplicationDocumentPath()));
      ConfigurationDocuments.Add(ConfigurationLevels.Stage, FirestoreClient.Document(_options.GetStageDocumentPath()));
      ConfigurationDocuments.Add(ConfigurationLevels.Machine, FirestoreClient.Document(_options.GetMachineDocumentPath()));
      ConfigurationDocuments.Add(ConfigurationLevels.Tag, FirestoreClient.Document(_options.GetTagDocumentPath()));

      ConfigurationDocuments.ToList().ForEach(d =>
        ConfigurationListeners.Add(d.Key, d.Value.Listen(async snapshot =>
        {
          await loadOnChangeAsyncCallback(d.Key, snapshot.Reference.Path);
        }))
      );
    }

    public async Task<Dictionary<string, object>> GetDocumentFieldsAsync(ConfigurationLevels level)
    {
      if (ConfigurationDocuments.TryGetValue(level, out var document))
      {
        var snapshot = await document.GetSnapshotAsync();
        if (snapshot.Exists)
          return snapshot.ToDictionary();
      }
      return new Dictionary<string, object>();
    }

    public async Task RemoveListener(ConfigurationLevels level)
    {
      if (ConfigurationListeners.ContainsKey(level))
        await ConfigurationListeners[level].StopAsync();

      if (ConfigurationDocuments.ContainsKey(level))
        ConfigurationDocuments.Remove(level);
    }

    public void CreateListener(ConfigurationLevels level, string documentPath, Func<ConfigurationLevels, string, Task> LoadOnChangeAsyncCallback)
    {
      var document = FirestoreClient.Document(documentPath);
      ConfigurationDocuments.Add(level, document);
      document.Listen(async snapshot => await LoadOnChangeAsyncCallback(level, snapshot.Id));
    }

    public IEnumerable<ConfigurationLevels> GetConfigurationDocumentLevels()
    {
      return ConfigurationDocuments.Keys.OrderBy(level => level);
    }
  }
}
