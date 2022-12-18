using Microsoft.AspNetCore.Mvc;
using SecretsSharing.Service;
using SecretsSharing.DTO;
using System.Threading.Tasks;
using System;
using System.Threading;

namespace SecretsSharing.Controllers
{
    /// <summary>
    /// Class controller for user.
    /// </summary>
    [ApiController]
    [Route("rest/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> RegistrationAsync([FromBody] UserRequestDTO userRequest)
        {
            try
            {
                return Ok(await _userService.RegistrationAsync(userRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserRequestDTO userRequest)
        {
            try
            {
                return Ok(await _userService.LoginAsync(userRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Method for getting the user's auto-deletion files settings.
        /// </summary>
        /// <param name="id">User id.</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserSettingsAsync(int id)
        {
            try
            {
                return Ok(await _userService.GetUserSettingsByIdAsync(id));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Method for changing user settings auto delete files.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <param name="userSettingsDTO">New user settings auto delete files</param>
        [HttpPost("{id}")]
        public async Task<IActionResult> SetUserSettingsAsync(int id, [FromBody] UserSettingsDTO userSettingsDTO)
        {
            try
            {
                await _userService.SetUserSettingsByIdAsync(id, userSettingsDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
