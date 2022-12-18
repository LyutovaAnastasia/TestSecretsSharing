using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SecretsSharing.Data.Models;
using SecretsSharing.Data.Repository;
using SecretsSharing.Data.Repository.impl;
using SecretsSharing.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSharing.Service
{
    /// <summary>
    /// Class service for document files.
    /// </summary>
    public class DocumentFileService
    {
        private readonly IFileRepository<DocumentFile> _documentFileRepository;
        private readonly IConfiguration _iconfiguration;
        private readonly UserRepository _userRepository;

        public DocumentFileService(IFileRepository<DocumentFile> repository,
                                   IConfiguration iconfiguration,
                                   UserRepository userRepository)
        {
            _documentFileRepository = repository;
            _iconfiguration = iconfiguration;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Getting document from the database by id.
        /// </summary>
        /// <param name="id">Document file id.</param>
        /// <returns>Document file.</returns>
        public async Task<DocumentFile> GetByIdAsync(Guid id)
        {
            return await _documentFileRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Getting all the user's document files.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns>List of user's document files.</returns>
        public async Task<IEnumerable<DocumentFileDTO>> GetFilesAsync(int userId)
        {
            var sourse = await _documentFileRepository.GetAllByUserIdAsync(userId);
            return sourse.Select(d => new DocumentFileDTO
            {
                Name = d.Name,
                AddDate = d.AddDate,
            });
        }

        /// <summary>
        /// Creating a new document file and saving it to the database.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="fileInfo">Data about the file to be saved in the database.</param>
        /// <returns>File access url.</returns>
        public async Task<string> CreateAsync(int userId, FileInfoDTO fileInfo)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                var documentFile = new DocumentFile()
                {
                    Id = Guid.NewGuid(),
                    Name = fileInfo.FileName,
                    FilePath = fileInfo.FilePath,
                    AddDate = DateTime.Now,
                    User = user,

                };
                await _documentFileRepository.CreateAsync(documentFile);

                return documentFile.GetUrl(_iconfiguration["UrlDocumentFile"], documentFile.Id);
            }

            return null;

        }

        /// <summary>
        /// Deleting a document file from the database.
        /// </summary>
        /// <param name="id">Document file id.</param>
        public async Task DeleteAsync(Guid id)
        {
            var source = await _documentFileRepository.GetByIdAsync(id);
            await _documentFileRepository.DeleteAsync(source);
        }

        /// <summary>
        /// Converts the file key to the file id.
        /// </summary>
        /// <param name="key">The key of the file from the link.</param>
        /// <returns>File Id.</returns>
        public Guid ConvertKeyToId(string key)
        {
            string guid = Encoding.UTF8.GetString(Convert.FromBase64String(key));
            if (!string.IsNullOrEmpty(guid))
            {
                return Guid.Parse(guid);

            }
            return Guid.Empty;
        }
    }
}
