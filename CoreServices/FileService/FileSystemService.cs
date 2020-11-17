using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace CoreServices.FileService
{
	public class FileSystemService : IFileSystemService
	{
		public string GetFilePath(string filename)
		{
			return Path.Combine(FileSystem.AppDataDirectory, filename);
		}

		public async Task<Stream> OpenFileStream(string filename)
		{
			return await FileSystem.OpenAppPackageFileAsync(filename);
		}
	}
}