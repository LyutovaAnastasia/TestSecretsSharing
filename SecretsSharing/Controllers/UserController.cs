using Microsoft.AspNetCore.Mvc;
using SecretsSharing.Service;
using SecretsSharing.DTO;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;

namespace SecretsSharing.Controllers
{
    /// <summary>
    /// Class controller for user.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("rest/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserServise _userService;

        public UserController(IUserServise userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Registration for the user.
        /// </summary>
        /// <param name="userRequest">User data for registration.</param>
        /// <returns>Information about the registered user.</returns>
        [AllowAnonymous]
        [HttpPost("registration")]
        public async Task<IActionResult> RegistrationAsync([FromBody] UserRequestDTO userRequest)
        {
            try
            {
                return Ok(await _userService.RegistrationAsync(userRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(new { messageError = ex.Message });
            }
        }

        /// <summary>
        /// User login and jwt token creation.
        /// </summary>
        /// <param name="userRequest">User data for login.</param>
        /// <returns>Jwt token.</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserRequestDTO userRequest)
        {
            try
            {
                return Ok(await _userService.LoginAsync(userRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(new { messageError = ex.Message });
            }
        }

        /// <summary>
        /// Method for getting the user's auto-deletion files settings.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <returns>Auto-deletion files settings.</returns>
        [HttpGet("getSettings/{id}")]
        public async Task<IActionResult> GetUserSettingsAsync(int id)
        {
            try
            {
                return Ok(await _userService.GetUserSettingsByIdAsync(id));
            }
            catch(Exception ex)
            {
                return BadRequest(new { messageError = ex.Message });
            }
        }

        /// <summary>
        /// Method for changing user settings auto delete files.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <param name="userSettingsDTO">New user settings auto delete files</param>
        /// <returns>Response status.</returns>
        [HttpPost("setSettings/{id}")]
        public async Task<IActionResult> SetUserSettingsAsync(int id, [FromBody] UserSettingsDTO userSettingsDTO)
        {
            try
            {
                await _userService.SetUserSettingsByIdAsync(id, userSettingsDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { messageError = ex.Message });
            }
        }
    }
}
