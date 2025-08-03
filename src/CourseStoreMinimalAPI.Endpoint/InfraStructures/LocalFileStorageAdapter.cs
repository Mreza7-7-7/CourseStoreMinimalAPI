
using Microsoft.AspNetCore.Authentication;

namespace CourseStoreMinimalAPI.Endpoint.InfraStructures;

public class LocalFileStorageAdapter(IHostEnvironment environment) : IFileAdapter
{
    private readonly IHostEnvironment _environment = environment;

    public string DeleteFile(string fileName, string path)
    {
        var webroot = _environment.ContentRootPath;
        var finalPath = Path.Combine(webroot, path);
        if (File.Exists(finalPath))
        {
            File.Delete(finalPath);
            return finalPath;
        }
        return string.Empty;
    }

    public string InsertFile(IFormFile file, string path)
    {
        string extension = Path.GetExtension(file.FileName);
        string fileName = $"{Guid.NewGuid().ToString()}.{extension}";
        var webroot = _environment.ContentRootPath;
        var folderForSave = Path.Combine(webroot, path);
        if (!Directory.Exists(folderForSave))
        {
            Directory.CreateDirectory(folderForSave);
        }
        using MemoryStream memoryStream = new();
        file.CopyTo(memoryStream);
        var finalPath = Path.Combine(folderForSave, fileName);
        File.WriteAllBytes(finalPath, memoryStream.ToArray());
        return fileName;
    }
}
