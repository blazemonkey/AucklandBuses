using System.Threading.Tasks;
using Windows.Storage;

namespace AucklandBuses.Services.FileReaderService
{
    public interface IFileReaderService
    {
        Task<string> ReadFile(string fileName, string folderName = "");
    }
}
