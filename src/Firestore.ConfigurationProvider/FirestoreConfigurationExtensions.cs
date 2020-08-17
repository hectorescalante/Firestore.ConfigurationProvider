using Firestore.ConfigurationProvider.Core.Helpers;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Firestore.ConfigurationProvider
{
  public static class FirestoreConfigurationExtensions
  {
    public static IConfigurationBuilder AddFirestoreConfiguration(this IConfigurationBuilder configurationBuilder)
    {
      return configurationBuilder.Add(new FirestoreConfigurationSource());
    }

    public static async Task WaitForFirestoreLoad(this IConfiguration configuration)
    {
      await configuration.WaitForCompleteLoad();
    }
  }
}