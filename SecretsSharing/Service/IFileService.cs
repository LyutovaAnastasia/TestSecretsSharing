using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace SecretsSharing.Service
{
    public interface IFileService<T, U>
    {
        public Task<T> DownloadAsync(string key);

        public Task<IEnumerable<T>> GetFilesAsync(Guid userId);

        public Task<U> UploadAsync(Guid userId, T _object);

        public Task DeleteAsync(string key);
       
    }
}
