namespace Microsoft.Extensions.Configuration.Firestore.Core
{
  public static class FirestoreConfigurationExtensions
  {
    public static IConfigurationBuilder AddFirestoreConfiguration(this IConfigurationBuilder configurationBuilder)
    {
      return configurationBuilder.Add(new FirestoreConfigurationSource());
    }
  }
}