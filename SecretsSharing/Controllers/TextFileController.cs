using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using SecretsSharing.DTO;
using System.Threading.Tasks;
using SecretsSharing.Service;
using Microsoft.AspNetCore.Authorization;

namespace SecretsSharing.Controllers
{
    /// <summary>
    /// Class controller for text files.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("rest/text")]
    public class TextFileController : ControllerBase
    {
        private readonly ITextFileService _textFileService;

        public TextFileController(ITextFileService service)
        {
            _textFileService = service;
        }

        /// <summary>
        /// Downloading a file from the database.
        /// </summary>
        /// <param name="key">Url file key.</param>
        /// <returns>Text file.</returns>
        [AllowAnonymous]
        [HttpGet("download/{key}")]
        public async Task<IActionResult> DownloadAsync(string key)
        {
            Guid id;
            try
            {
                id = _textFileService.ConvertKeyToId(key);
                return Ok(await _textFileService.GetByIdAsync(id));
            }
            catch(Exception ex)
            {
                return BadRequest(new { messageError = ex.Message });
            }
        }

        /// <summary>
        /// Getting all the user's text files.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns>List of user's text files.</returns>
        [HttpGet("getAll/{userId}")]
        public async Task<IEnumerable<TextFileDTO>> GetFilesAsync(int userId)
        {
            return await _textFileService.GetFilesAsync(userId);
        }

        /// <summary>
        /// Uploading a file to the database.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="textFileRequestDTO"></param>
        /// <returns>File access url.</returns>
        [HttpPost("upload/{userId}")]
        public async Task<ActionResult> UploadAsync(int userId, [FromBody] TextFileRequestDTO textFileRequestDTO)
        {
            try
            {
                var result = await _textFileService.CreateAsync(userId, textFileRequestDTO);
                return Ok(new { result });
            }
            catch(Exception ex)
            {
                return BadRequest(new { messageError = ex.Message });
            }
        }

        /// <summary>
        /// Deleting a text file from the database.
        /// </summary>
        /// <param name="key">Url file key.</param>
        /// <returns>Response status.</returns>
        [HttpDelete("delete/{key}")]
        public async Task<IActionResult> DeleteAsync(string key)
        {
            Guid id;
            try
            {
                id = _textFileService.ConvertKeyToId(key);
                await _textFileService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { messageError = ex.Message });
            }
        }
    }
}
