using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public interface IFileStorageService
    {
        Task<string> SaveTruckFileAsync(IFormFile file, int transferId);
    }
}