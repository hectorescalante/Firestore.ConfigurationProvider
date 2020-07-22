using Microsoft.Extensions.Configuration;

namespace Firestore.ConfigurationProvider
{
  public static class FirestoreConfigurationExtensions
  {
    public static IConfigurationBuilder AddFirestoreConfiguration(this IConfigurationBuilder configurationBuilder)
    {
      return configurationBuilder.Add(new FirestoreConfigurationSource());
    }
  }
}