using System.Threading.Tasks;
using System.Threading;
using SecretsSharing.DTO;

namespace SecretsSharing.Service
{
    public interface IUserServise
    {
        Task<UserResponseDTO> Registration(UserRequestDTO userRequest);
        Task<UserResponseDTO> Login(UserRequestDTO userRequest);
    }
}
