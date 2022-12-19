using Microsoft.Extensions.Configuration;
using SecretsSharing.Data.Models;
using SecretsSharing.Data.Repository.impl;
using SecretsSharing.Data.Repository;
using SecretsSharing.DTO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System;

namespace SecretsSharing.Service
{
    public interface IDocumentService
    {
        Task<DocumentFile> GetByIdAsync(Guid id);
        Task<IEnumerable<DocumentFileDTO>> GetFilesAsync(int userId);
        Task<string> CreateAsync(int userId, FileInfoDTO fileInfo);
        Task DeleteAsync(Guid id);
        Guid ConvertKeyToId(string key);
    }
}
