using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FileManagerModels.Models;
using FileManageServices.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FileManageServices.Services
{
    public class FileManagerSrevice : IFileManagerSrevice
    {
      //  private readonly string storageConnectionString;
        private readonly BlobContainerClient blobContainerClient; 

        public FileManagerSrevice(IConfiguration configuration)
        {
            string storageConnectionString = configuration.GetConnectionString("AzureStorage");
            this.blobContainerClient = new BlobContainerClient(storageConnectionString, "file-container");
        }

        public async Task<string> UploadFile(Stream fileStream, string fileName, string contentType)
        {
            var createResponse = await blobContainerClient.CreateIfNotExistsAsync();

            if (createResponse != null && createResponse.GetRawResponse().Status == 201)
                await blobContainerClient.SetAccessPolicyAsync(PublicAccessType.Blob);

            var blob = blobContainerClient.GetBlobClient(fileName);
            await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);

            await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType });

            return blob.Uri.ToString();
        }

        public List<string> GetFilesList()
        {
            List<string> fileNames = new List<string>();

            var blobList = blobContainerClient.GetBlobs();

            foreach (BlobItem blobItem in blobList)
            {
                fileNames.Add(blobItem.Name);
            }
            return fileNames;

        }

        public async Task<FileDetails> ViewFile(string fileName)
        {
            FileDetails fileDetails = new FileDetails();

            var blobClient = blobContainerClient.GetBlobClient(fileName);
            if (await blobClient.ExistsAsync())
            {
                var downloadContent = await blobClient.DownloadAsync();
                using (MemoryStream ms = new MemoryStream())
                {
                    await downloadContent.Value.Content.CopyToAsync(ms);
                    fileDetails.ContentType = downloadContent.Value.ContentType;
                    fileDetails.Content = ms.ToArray();
                    return fileDetails;
                }
            }
            else
            {
                return null;
            }
        }


        public async Task<FileDetails> DownloadFile(string fileName)
        {
            var blob = blobContainerClient.GetBlobClient(fileName);

            if (await blob.ExistsAsync())
            {
                var a =  await blob.DownloadAsync();

                FileDetails fileDetails = new FileDetails();
                fileDetails.FileStream = a.Value.Content;
                fileDetails.ContentType = a.Value.ContentType;
                fileDetails.Name = fileName;

                return fileDetails; 

            }
            else
            {
                return null; 
            }
        }

        public async Task Delete(string fileName)
        {
            var blobClient = blobContainerClient.GetBlobClient(fileName);
            await blobClient.DeleteAsync();
        }

    }
}
