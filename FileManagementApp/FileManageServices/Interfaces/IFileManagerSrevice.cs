using FileManagerModels.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FileManageServices.Interfaces
{
    public interface IFileManagerSrevice
    {
        Task<string> UploadFile(Stream fileStream, string fileName, string contentType);
        List<string> GetFilesList();
        Task<FileDetails> ViewFile(string fileName); 
        Task<FileDetails> DownloadFile(string fileName);
        Task Delete(string fileName); 
    }
}
