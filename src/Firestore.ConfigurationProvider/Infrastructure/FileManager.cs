using Firestore.ConfigurationProvider.Core.Abstractions;
using System.IO;
using System.Text;

namespace Firestore.ConfigurationProvider.Infrastructure
{
  internal class FileManager : IFileManager
  {
    public string GetFileContent(string path)
    {
      return File.ReadAllText(path, Encoding.UTF8);
    }
  }
}
