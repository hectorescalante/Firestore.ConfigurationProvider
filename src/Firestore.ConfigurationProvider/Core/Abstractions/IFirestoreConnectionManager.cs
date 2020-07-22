using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Microsoft.Extensions.Configuration.Firestore.Core.Abstractions
{
  internal interface IFirestoreConnectionManager
  {
    void Setup();
    Task<bool> IsDocumentEmptyAsync(string documentPath);
    Task SaveAsync(string documentPath, Dictionary<string, object> fields);
    Task<Dictionary<string, object>> GetDocumentFieldsAsync(ConfigurationLevels level);
    void CreateListeners(Func<ConfigurationLevels, string, Task> LoadOnChangeAsyncCallback);
    Task RemoveListener(ConfigurationLevels level);
    void CreateListener(ConfigurationLevels level, string documentPath, Func<ConfigurationLevels, string, Task> LoadOnChangeAsyncCallback);
    IEnumerable<ConfigurationLevels> GetConfigurationDocumentLevels();
  }
}
