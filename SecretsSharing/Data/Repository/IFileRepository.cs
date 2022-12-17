using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecretsSharing.Data.Repository
{
    public interface IFileRepository<T>
    {
        public Task<T> GetByIdAsync(Guid id);
        public Task<IEnumerable<T>> GetAllByUserIdAsync(Guid userId);
        public Task CreateAsync(T _object);
        public Task DeleteAsync(T _object);

    }
}
