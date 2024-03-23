using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopStore.Services.Services.StorageService
{
    public interface IStorageService
    {
        Task<string> SaveImageAsync(IFormFile formFile);
        Task<bool> DeleteImageAsync(string fileName);
        Task<Stream> GetImageAsync(string fileName);
    }
}
