using SecretsSharing.Data.Models;
using SecretsSharing.Data.Repository.impl;
using SecretsSharing.DTO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System;

namespace SecretsSharing.Service
{
    public interface ITextFileService
    {
        Task<TextFileDTO> GetByIdAsync(Guid id);
        Task<IEnumerable<TextFileDTO>> GetFilesAsync(int userId);
        Task<string> CreateAsync(int userId, TextFileRequestDTO textFileRequestDTO);
        Task DeleteAsync(Guid id);
        Guid ConvertKeyToId(string key);
    }
}
