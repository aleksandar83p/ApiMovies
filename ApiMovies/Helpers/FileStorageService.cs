using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMovies.Helpers
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _Env;        

        public FileStorageService(IWebHostEnvironment env)
        {
            this._Env = env;           
        }

        public async Task<string> SaveFile(string containerName, IFormFile file)
        {
            string fileName = new String(Path.GetFileNameWithoutExtension(file.FileName).Take(10).ToArray()).Replace(" ", "-");
            string imageName = $"{fileName}_{Guid.NewGuid()}_{DateTime.Now.ToString("MMddyyyy_HHmmss")}{Path.GetExtension(file.FileName)}";

            var imagePath = Path.Combine(_Env.ContentRootPath, $"Images/{containerName}", imageName);

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return imageName;
        }

        public async Task<string> EditFile(string containerName, IFormFile file, string fileRoute)
        {
            await DeleteFile(fileRoute, containerName);
            return await SaveFile(containerName, file);
        }

        public Task DeleteFile(string fileRoute, string containerName)
        {
            if (string.IsNullOrEmpty(fileRoute))
            {
                return Task.CompletedTask;
            }

            var fileName = Path.GetFileName(fileRoute);
            var fileDirectory = Path.Combine($"{_Env.ContentRootPath}/Images/{containerName}/{fileName}");

            if (File.Exists(fileDirectory))
            {
                File.Delete(fileDirectory);
            }

            return Task.CompletedTask;
        }     
    }
}
