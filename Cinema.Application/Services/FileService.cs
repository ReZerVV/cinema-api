using Cinema.Application.Services.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Cinema.Application.Services;

internal class FileService : IFileService
{
    private readonly IHostingEnvironment _env;

    public FileService(IHostingEnvironment env)
    {
        _env = env;
    }

    public bool Load(IFormFile file, string fileName)
    {
        var filePath = Path.Combine(_env.WebRootPath, fileName);
        using var fileStream = new FileStream(filePath, FileMode.Create);
        file.CopyTo(fileStream);
        fileStream.Close();
        return true;
    }

    public bool Remove(string fileName)
    {
        var filePath = Path.Combine(_env.WebRootPath, fileName);
        if (!File.Exists(filePath))
            return false;
        File.Delete(filePath);
        return true;
    }
}
