using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _env;

        public FileStorageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveTruckFileAsync(IFormFile file, int transferId)
        {
            var uploadsRoot = Path.Combine(_env.WebRootPath ?? "wwwroot", "Uploads", "Truck", $"Transfer-{transferId}");

            var destinationPath = Path.Combine("E:", "ARMS2", "wwwroot", "ArmsStaticFiles", "Truck", $"Transfer-{transferId}");
            //var destinationPath = Path.Combine("C:", "Data", "Arms", "ARMS.v2", "Views", "wwwroot", "ArmsStaticFiles", "Truck", $"Transfer-{transferId}");

            if (!Directory.Exists(uploadsRoot))
                Directory.CreateDirectory(uploadsRoot);

            var extension = Path.GetExtension(file.FileName).ToLower();
            var fileName = Guid.NewGuid().ToString() + extension;
            var filePath = Path.Combine(uploadsRoot, fileName);
            var destfilePath = Path.Combine(destinationPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            // Copy the file
            System.IO.File.Copy(filePath, destfilePath, true); // 'true' to overwrite if exists

            return $"/ArmsStaticFiles/Truck/Transfer-{transferId}/{fileName}".Replace("\\", "/");
        }

        public async Task<(byte[] FileBytes, string ContentType, string FileName)> GetTruckFileAsync(int transferId, string fileName)
        {
            var destinationPath = Path.Combine("E:", "ARMS2", "wwwroot", "ArmsStaticFiles", "Truck", $"Transfer-{transferId}");
            var fullPath = Path.Combine(destinationPath, fileName);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException("File not found", fileName);

            byte[] fileBytes;
            await using (var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {
                using var memory = new MemoryStream();
                await stream.CopyToAsync(memory);
                fileBytes = memory.ToArray();
            }

            var contentType = GetContentType(fullPath);

            return (fileBytes, contentType, fileName);
        }

        private string GetContentType(string path)
        {
            var types = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
            {
                [".txt"] = "text/plain",
                [".pdf"] = "application/pdf",
                [".doc"] = "application/msword",
                [".docx"] = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                [".xls"] = "application/vnd.ms-excel",
                [".xlsx"] = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                [".png"] = "image/png",
                [".jpg"] = "image/jpeg",
                [".jpeg"] = "image/jpeg",
                [".gif"] = "image/gif"
            };

            var ext = Path.GetExtension(path);
            return types.ContainsKey(ext) ? types[ext] : "application/octet-stream";
        }
    }
}
