using System.IO;
using System.Threading.Tasks;

namespace CoreServices.FileService
{
	public interface IFileSystemService
	{
		string GetFilePath(string filename);
		Task<Stream> OpenFileStream(string filename);
	}
}