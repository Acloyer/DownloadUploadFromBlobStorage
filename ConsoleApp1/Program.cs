using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BlobStorageExample
{
    class Program
    {
        private static readonly string connectionString = "DefaultEndpointsProtocol=https;AccountName=downloaduploadapp;AccountKey=eBo2dqjZx6UcMjo8hYx81941KZYBl/cNxZRc4ROmj8T6uzC9rO0Jx2iMZ2fPKm/qMG3iohZ1sWu5+ASt069PUg==;EndpointSuffix=core.windows.net";
        private static readonly string containerName = "files";

        static async Task Main(string[] args)
        {
            await Console.Out.WriteLineAsync("короч код отправляет на контейнер files файл example.txt который находитсся в 'bin\\Debug\\net8.0'\n" +
                "и скачивает файл из этого же контейнера но в папку downloads_from_blobstorage, если папки нет - то он создается во время скачивания example.txt\n\n\n");
            var downloadDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "downloads_from_blobstorage");
            Directory.CreateDirectory(downloadDirectory);

            await UploadFileAsync("example.txt");
            await DownloadFileAsync("example.txt", downloadDirectory);

            Console.WriteLine("Done!");
        }

        static async Task UploadFileAsync(string filePath)
        {
            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(Path.GetFileName(filePath));

            using (var fileStream = File.OpenRead(filePath))
            {
                await blobClient.UploadAsync(fileStream, true);
            }

            Console.WriteLine($"File {Path.GetFileName(filePath)} uploaded successfully.");
        }

        static async Task DownloadFileAsync(string fileName, string downloadDirectory)
        {
            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            var downloadFilePath = Path.Combine(downloadDirectory, fileName);

            using (var downloadFileStream = File.OpenWrite(downloadFilePath))
            {
                await blobClient.DownloadToAsync(downloadFileStream);
            }

            Console.WriteLine($"File {fileName} downloaded successfully to {downloadFilePath}.");
        }
    }
}
