using EntertainmentApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace EntertainmentApp.Application.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _environment;

        private string localFilePath;
        public FileUploadService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public void UploadFile(Stream input, string fileName)
        {
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;

            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }


            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            localFilePath = Path.Combine("uploads", uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                input.CopyTo(fileStream);
            }
        }

        public string GetLocalFilePath()
        {
            return localFilePath;
        }


    }
}
