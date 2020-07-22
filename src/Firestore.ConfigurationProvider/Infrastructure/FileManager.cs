using Microsoft.Extensions.Configuration.Firestore.Core.Abstractions;
using System.IO;
using System.Text;

namespace Microsoft.Extensions.Configuration.Firestore.Infrastructure
{
  internal class FileManager : IFileManager
  {
    public string GetFileContent(string path)
    {
      return File.ReadAllText(path, Encoding.UTF8);
    }
  }
}
