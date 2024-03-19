using Microsoft.AspNetCore.Http;

namespace Cinema.Application.Services.Abstractions;

internal interface IFileService
{
    bool Load(IFormFile file, string fileName);
    bool Remove(string fileName);
}
