using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace LaptopStore.Services.Services.StorageService
{
    public class StorageService : IStorageService
    {
        private readonly List<string> _allowedExtensions = new List<string>() { ".jpg", ".jpeg", ".png", ".gif" };
        private readonly long _maxFileSize = 10485760; // 10M
        private readonly string _imagePath = string.Empty;
        private readonly string _imageFolderName = "images";

        public StorageService(string webRootPath)
        {
            _imagePath = Path.Combine(webRootPath, _imageFolderName);
        }

        public async Task<string> SaveImageAsync(IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
            {
                throw new ArgumentException("The file is empty.");
            }

            var extension = Path.GetExtension(formFile.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
            {
                throw new ArgumentException("The image extension is not allowed.");
            }

            if (formFile.Length > _maxFileSize)
            {
                throw new ArgumentException("The file is too large.");
            }

            if (string.IsNullOrEmpty(_imagePath))
            {
                throw new ArgumentException("The image path is empty.");
            }

            if (!Directory.Exists(_imagePath))
            {
                Directory.CreateDirectory(_imagePath);
            }

            var uniqueFileName = GenerateUniqueFileName(extension);
            var filePath = Path.Combine(_imagePath, uniqueFileName);

            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await formFile.CopyToAsync(fileStream);

            return $"/{_imageFolderName}/{uniqueFileName}";
        }

        public async Task<bool> DeleteImageAsync(string fileName)
        {
            var filePath = Path.Combine(_imagePath, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
                return true;
            }
            return false;
        }

        private string GenerateUniqueFileName(string extension)
        {
            return $"{Guid.NewGuid()}{extension}";
        }

        public async Task<Stream> GetImageAsync(string fileName)
        {
            var filePath = Path.Combine(_imagePath, fileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The image is not found");
            }
            return System.IO.File.OpenRead(filePath);
        }
    }
}
