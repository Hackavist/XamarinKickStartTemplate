using System.IO;
using System.Threading.Tasks;

namespace BaseTemplate.Services.FileSystemService
{
    public interface IFileSystemService
    {
        string GetFilePath(string filename);
        Task<Stream> OpenFileStream(string filename);
    }
}