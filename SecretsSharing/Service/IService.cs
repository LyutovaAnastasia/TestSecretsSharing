using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace SecretsSharing.Service
{
    public interface IService<T, U>
    {
        public Task<T> DownloadAsync(string key);

        public Task<IEnumerable<T>> GetFilesAsync(Guid userId);

        public Task<U> UploadAsync(T _object);

        public Task DeleteAsync(string key);
       
    }
}
