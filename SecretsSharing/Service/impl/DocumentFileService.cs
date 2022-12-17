using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SecretsSharing.Data.Models;
using SecretsSharing.Data.Repository;
using SecretsSharing.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSharing.Service.impl
{
    public class DocumentFileService
    {
        private readonly IFileRepository<DocumentFile> _repository;
        private readonly IConfiguration _iconfiguration;
        private readonly ILogger<DocumentFileService> _logger;
        private readonly UserRepository _userRepository;

        private string[] permittedExtensions = { ".txt", ".pdf" };
        private readonly long _fileSizeLimit;

        public DocumentFileService(IFileRepository<DocumentFile> repository,
                                   IConfiguration iconfiguration,
                                   ILogger<DocumentFileService> logger,
                                   UserRepository userRepository)
        {
            _repository = repository;
            _iconfiguration = iconfiguration;
            _logger = logger;
            _fileSizeLimit = _iconfiguration.GetValue<long>("FileSizeLimit");
            _userRepository = userRepository;
        }

        public async Task<DocumentFile> DownloadAsync(string key)
        {
            string guid = Encoding.UTF8.GetString(Convert.FromBase64String(key));
            if (!string.IsNullOrEmpty(guid))
            {
                Guid id = Guid.Parse(guid);
                return await _repository.GetByIdAsync(id);
            }
            return null;
        }

        public async Task<string> UploadAsync(Guid userId, IFormFile file)
        {
   
            string filePath = null;
            try
            {
                _logger.LogInformation("Start upload file.");
                if (ValidateFile(file))
                {
                    filePath = CreateFilePath(file);

                    using (var stream = File.Create(Path.Combine(_iconfiguration["StoredFiles"], filePath)))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var user = await _userRepository.GetByIdAsync(userId);
                    if (user == null)
                    {
                        Console.WriteLine("throw");
                    }

                    var documentFile = new DocumentFile()
                    {
                        Id = Guid.NewGuid(),
                        Name = Path.GetFileName(file.FileName),
                        FilePath = filePath,
                        AddDate = DateTime.Now,
                        User = user,

                    };

                    await _repository.CreateAsync(documentFile);
                    string url = _iconfiguration["UrlDocumentFile"] + Convert
                                 .ToBase64String(Encoding.UTF8.GetBytes(Convert
                                 .ToString(documentFile.Id)));

                    _logger.LogInformation("Finish upload file. File success uploaded.");
                    return url;
                }
                return null;
            }
            catch
            {
                throw;
            }
            
        }
        public async Task<IEnumerable<DocumentFileDTO>> GetFilesAsync(Guid userId)
        {
            var sourse = await _repository.GetAllByUserIdAsync(userId);
            return sourse.Select(d => new DocumentFileDTO
            {
                Name = d.Name,
                AddDate = d.AddDate,
            });
        }

        public async Task DeleteAsync(string key)
        {
            string guid = Encoding.UTF8.GetString(Convert.FromBase64String(key));
            if (!string.IsNullOrEmpty(guid))
            {
                var source = await _repository.GetByIdAsync(Guid.Parse(guid));
                var isDeleted = DeleteFile(source.FilePath);
                if (isDeleted) 
                {
                     await _repository.DeleteAsync(source);
                }
            }
        }

        private bool DeleteFile(string path)
        {
            if (path == null)
                return false;

            var filePath = Path.Combine(_iconfiguration["StoredFiles"], path);

            if (!File.Exists(filePath)) 
                return false;
            try
            {
                File.Delete(filePath);
                return true;
            }
            catch
            {
                throw;
            }
        }

        private bool ValidateFile(IFormFile file)
        {
            // Null validation
            if (file.Length <= 0)
            {
                _logger.LogInformation("Error upload file. The file is Empty.");
                return false;
            }

            var untrustedFileName = Path.GetFileName(file.FileName);
            var ext = Path.GetExtension(untrustedFileName).ToLowerInvariant();

            // File extension validation
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                _logger.LogInformation("Error upload file. The extension of file is invalid.");
                return false;
            }

            // Size validation
            if (file.Length > _fileSizeLimit)
            {
                _logger.LogInformation("Error upload file. The file is too large.");
                return false;
            }

            return true;
        }

        private string CreateFilePath(IFormFile file)
        {
            var untrustedFileName = Path.GetFileName(file.FileName);
            var ext = Path.GetExtension(untrustedFileName).ToLowerInvariant();
            var filename = Guid.NewGuid().ToString() + ext;
            return Path.Combine(_iconfiguration["StoredFilesPackage"], filename);
        }

        private string CreateFileName(IFormFile file)
        {
            var untrustedFileName = Path.GetFileName(file.FileName);
            var ext = Path.GetExtension(untrustedFileName).ToLowerInvariant();
            return Guid.NewGuid().ToString() + ext;
        }

    }
}
