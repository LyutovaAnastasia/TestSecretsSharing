using Microsoft.EntityFrameworkCore;
using SecretsSharing.Data.Models;
using System.Threading.Tasks;
using System;

namespace SecretsSharing.Data.Repository
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByEmailAsync(string email);
        Task CreateAsync(User user);
        Task SetUserSettingsAsync(User user);
    }
}
