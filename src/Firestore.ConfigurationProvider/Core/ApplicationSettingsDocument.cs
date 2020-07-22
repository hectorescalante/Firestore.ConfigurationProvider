using System.Text.Json;

namespace Firestore.ConfigurationProvider
{
  public class ApplicationSettingsDocument
  {
    public JsonElement Data { get; set; }
  }
}
