using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace FFME.Avalonia.Sample.Services;

public interface IFileService
{
    public Task<IStorageFile?> OpenFileAsync();
    public Task<IStorageFile?> SaveFileAsync();
}