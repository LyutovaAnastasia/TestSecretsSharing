using Microsoft.Extensions.Configuration;
using SecretsSharing.Data.Models;
using SecretsSharing.Data.Repository;
using SecretsSharing.DTO;
using SecretsSharing.Utils;
using System;
using System.Threading.Tasks;

namespace SecretsSharing.Service.impl
{
    /// <summary>
    /// Class service for user.
    /// </summary>
    public class UserService : IUserServise
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _iconfiguration;
        private readonly JwtUtils _jwtUtils;

        private readonly string _pepper;
        private readonly int _iteration = 3;
        public UserService(IUserRepository userRepository, IConfiguration configuration,
                           JwtUtils jwtUtils)
        {
            _userRepository = userRepository;
            _iconfiguration = configuration;
            _pepper = _iconfiguration["PasswordHashPepper"];
            _jwtUtils = jwtUtils;
        }

        /// <summary>
        /// Registration for the user.
        /// </summary>
        /// <param name="userRequest">User data for registration.</param>
        /// <returns>Information about the registered user.</returns>
        public async Task<UserResponseDTO> RegistrationAsync(UserRequestDTO userRequest)
        {

            var userExist = await _userRepository.GetByEmailAsync(userRequest.Email);
            if (userExist != null)
                throw new Exception("User already exists!");

            var user = new User
            {
                Email = userRequest.Email,
                PasswordSalt = PasswordHasherUtils.GenerateSalt(),
                AutoDeleteDocument = false,
                AutoDeleteText = false
            };

            user.PasswordHash = PasswordHasherUtils.CreateHash(userRequest.Password,
                                                               user.PasswordSalt, _pepper, _iteration);
            await _userRepository.CreateAsync(user);

            return new UserResponseDTO
            {
                Id = user.Id,
                Email = user.Email
            };
        }

        /// <summary>
        /// User login and jwt token creation.
        /// </summary>
        /// <param name="userRequest">User data for login.</param>
        /// <returns>Jwt token.</returns>
        public async Task<string> LoginAsync(UserRequestDTO userRequest)
        {
            var user = await _userRepository.GetByEmailAsync(userRequest.Email);

            if (user == null)
                throw new Exception("The user with this email was not found.");

            var passwordHash = PasswordHasherUtils.CreateHash(userRequest.Password,
                                                              user.PasswordSalt, _pepper, _iteration);
            if (user.PasswordHash != passwordHash)
                throw new Exception("Email or password did not match.");

            var token = _jwtUtils.CreateToken(user);
            return token;
        }

        /// <summary>
        /// Method for getting the user's auto-deletion files settings.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <returns>Auto-deletion files settings.</returns>
        public async Task<UserSettingsDTO> GetUserSettingsByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new Exception("The user not found.");

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
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new Exception("The user not found.");
            user.AutoDeleteDocument = userSettingsDTO.AutoDeleteDocument;
            user.AutoDeleteText = userSettingsDTO.AutoDeleteText;

            await _userRepository.SetUserSettingsAsync(user);
        }


    }
}
