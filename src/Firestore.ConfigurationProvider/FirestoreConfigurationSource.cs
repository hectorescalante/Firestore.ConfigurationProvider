using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Firestore.ConfigurationProvider
{
  internal class FirestoreConfigurationSource : JsonStreamConfigurationSource
  {
    public override IConfigurationProvider Build(IConfigurationBuilder builder)
    {
      return new FirestoreConfigurationProvider(this);
    }
  }
}
