namespace Firestore.ConfigurationProvider.Core.Abstractions
{
  internal interface IFileManager
  {
    string GetFileContent(string path);
  }
}
