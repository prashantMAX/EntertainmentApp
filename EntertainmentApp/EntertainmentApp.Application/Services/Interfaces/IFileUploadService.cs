using System.IO;

namespace EntertainmentApp.Application.Services.Interfaces
{
    public interface IFileUploadService
    {
        void UploadFile(Stream input, string filepath);
        string GetLocalFilePath();

    }
}
