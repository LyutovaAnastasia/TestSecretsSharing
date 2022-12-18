using Microsoft.Extensions.Configuration;
using SecretsSharing.Data.Models;
using SecretsSharing.Data.Repository;
using SecretsSharing.DTO;
using SecretsSharing.Utils;
using System;
using System.Threading.Tasks;

namespace SecretsSharing.Service
{
    /// <summary>
    /// Class service for user.
    /// </summary>
    public class UserService
    {
        private readonly UserRepository _repository;
        private readonly IConfiguration _iconfiguration;

        private readonly string _pepper;
        private readonly int _iteration = 3;
        public UserService(UserRepository userRepository, IConfiguration configuration)
        {
            _repository = userRepository;
            _iconfiguration = configuration;
            _pepper = _iconfiguration["PasswordHashPepper"];
        }

        public async Task<UserResponseDTO> RegistrationAsync(UserRequestDTO userRequest)
        {
            var user = new User
            {
                Email = userRequest.Email,
                PasswordSalt = PasswordHasherUtils.GenerateSalt(),
                AutoDeleteDocument = false,
                AutoDeleteText = false
            };

            user.PasswordHash = PasswordHasherUtils.CreateHash(userRequest.Password,
                                                               user.PasswordSalt, _pepper, _iteration);
            await _repository.CreateAsync(user);
           
            return new UserResponseDTO { 
                Id = user.Id, 
                Email = user.Email 
            };
        }

        public async Task<UserResponseDTO> LoginAsync(UserRequestDTO userRequest)
        {
            var user = await _repository.GetByEmailAsync(userRequest.Email);

            if (user == null)
                throw new Exception("The user with this email was not found.");

            var passwordHash = PasswordHasherUtils.CreateHash(userRequest.Password,
                                                              user.PasswordSalt, _pepper, _iteration);
            if (user.PasswordHash != passwordHash)
                throw new Exception("Email or password did not match.");

            return new UserResponseDTO
            {
                Id = user.Id,
                Email = user.Email
            };
        }

        /// <summary>
        /// Method for getting the user's auto-deletion files settings.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <returns>Auto-deletion files settings</returns>
        public async Task<UserSettingsDTO> GetUserSettingsByIdAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            var result = new UserSettingsDTO
            {
                AutoDeleteDocument = user.AutoDeleteDocument,
                AutoDeleteText = user.AutoDeleteText,
            };
            return result;
        }

        /// <summary>
        /// Method for changing user settings auto delete files.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <param name="userSettingsDTO">Auto-delete settings.</param>
        public async Task SetUserSettingsByIdAsync(int id, UserSettingsDTO userSettingsDTO)
        {
            var user = await _repository.GetByIdAsync(id);
            user.AutoDeleteDocument = userSettingsDTO.AutoDeleteDocument;
            user.AutoDeleteText = userSettingsDTO.AutoDeleteText;

            await _repository.SetUserSettingsAsync(user);
        }


    }
}
