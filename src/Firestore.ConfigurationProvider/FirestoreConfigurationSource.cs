using Microsoft.Extensions.Configuration.Json;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]
namespace Microsoft.Extensions.Configuration.Firestore.Core
{
  internal class FirestoreConfigurationSource : JsonStreamConfigurationSource
  {
    public override IConfigurationProvider Build(IConfigurationBuilder builder)
    {
      return new FirestoreConfigurationProvider(this);
    }
  }
}
