using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using SecretsSharing.DTO;
using Microsoft.AspNetCore.Mvc;
using SecretsSharing.Data.Models;
using SecretsSharing.Service;

namespace SecretsSharing.Util
{
    /// <summary>
    /// Class with methods to work with the file.
    /// </summary>
    public class FileUtils
    {
        
        private readonly IConfiguration _iconfiguration;
        private readonly ILogger<DocumentFileService> _logger;

        private readonly long _fileSizeLimit;

        public FileUtils(IConfiguration iconfiguration,
                         ILogger<DocumentFileService> logger)
        {
            _iconfiguration = iconfiguration;
            _logger = logger;
            _fileSizeLimit = _iconfiguration.GetValue<long>("FileSizeLimit");
        }

        /// <summary>
        /// Method for downloading a file from storage.
        /// </summary>
        /// <param name="path">The path of the file to download.</param>
        /// <returns>File byte array.</returns>
        public async Task<byte[]> DownloadFileAsync(string path)
        {
            try
            {
                var filePath = Path.Combine(_iconfiguration["StoredFiles"], path);
                return await File.ReadAllBytesAsync(filePath);  
            }
            catch (Exception)
            {
                throw new Exception("Error download file.");
            }

        }

        /// <summary>
        /// Method for saving a file to storage.
        /// </summary>
        /// <param name="file">The file to be saved.</param>
        /// <returns>Information about the saved file.</returns>
        public async Task<FileInfoDTO> UploadFileAsync(IFormFile file)
        {
            string filePath = null;
            try
            {
                _logger.LogInformation("Start upload file.");
                ValidateFile(file);
               
                filePath = CreateFilePath(file);

                using (var stream = File.Create(Path.Combine(_iconfiguration["StoredFiles"], filePath)))
                {
                    await file.CopyToAsync(stream);
                }

                _logger.LogInformation("Finish upload file. File success uploaded.");

                var fileInfo = new FileInfoDTO()
                {
                    FileName = Path.GetFileName(file.FileName),
                    FilePath = filePath,
                };
                return fileInfo;
            }
            catch(Exception ex)
            {
                throw new Exception("Error upload file. " + ex.Message);
            }

        }

        /// <summary>
        /// Method for deleting a file from storage.
        /// </summary>
        /// <param name="path">The path of the file stored in the database.</param>
        public void DeleteFile(string path)
        {
            if (path == null)
                throw new Exception("Invalid path.");

            var filePath = Path.Combine(_iconfiguration["StoredFiles"], path);

            if (!File.Exists(filePath))
                throw new Exception("File is not exist.");
            try
            {
                File.Delete(filePath);
            }
            catch
            {
                throw new Exception("Error delete file.");
            }
        }

        /// <summary>
        /// Method for file validation.
        /// </summary>
        /// <param name="file">The file to be saved in the repository.</param>
        private void ValidateFile(IFormFile file)
        {
            string info;
            // Null validation
            if (file.Length <= 0)
            {
                info = "The file is Empty.";
                _logger.LogInformation(info);
                throw new Exception(info);
            }
            
            // Size validation
            if (file.Length > _fileSizeLimit)
            {
                info = "The file is too large.";
                _logger.LogInformation(info);
                throw new Exception(info);
            }
        }

        /// <summary>
        /// Creates a new file name and path.
        /// </summary>
        /// <param name="file">The file to be saved in the repository.</param>
        /// <returns>New file path for db.</returns>
        private string CreateFilePath(IFormFile file)
        {
            try
            {
                var untrustedFileName = Path.GetFileName(file.FileName);
                var ext = Path.GetExtension(untrustedFileName).ToLowerInvariant();
                var filename = Guid.NewGuid().ToString() + ext;
                return Path.Combine(_iconfiguration["StoredFilesPackage"], filename);
            }
            catch
            {
                throw new Exception("Failed to create path.");
            }
        }

    }
}
