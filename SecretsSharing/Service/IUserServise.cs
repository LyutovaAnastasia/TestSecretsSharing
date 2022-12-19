using System.Threading.Tasks;
using SecretsSharing.DTO;

namespace SecretsSharing.Service
{
    public interface IUserServise
    {
        Task<UserResponseDTO> RegistrationAsync(UserRequestDTO userRequest);
        Task<string> LoginAsync(UserRequestDTO userRequest);
        Task<UserSettingsDTO> GetUserSettingsByIdAsync(int id);
        Task SetUserSettingsByIdAsync(int id, UserSettingsDTO userSettingsDTO);
    }
}
