using Google.Cloud.Firestore;
using System.Collections.Generic;
using System.Text.Json;

namespace Microsoft.Extensions.Configuration.Firestore
{
  public class ApplicationSettingsDocument
  {
    public JsonElement Data { get; set; }
  }
}
