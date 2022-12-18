using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using SecretsSharing.DTO;
using System.Threading.Tasks;
using SecretsSharing.Service;

namespace SecretsSharing.Controllers
{
    /// <summary>
    /// Class controller for text files.
    /// </summary>
    [ApiController]
    [Route("rest/text")]
    public class TextFileController : ControllerBase
    {
        private readonly TextFileService _service;

        public TextFileController(TextFileService service)
        {
            _service = service;
        }

        /// <summary>
        /// Downloading a file from the database.
        /// </summary>
        /// <param name="key">Url file key.</param>
        /// <returns>Response status.</returns>
        [HttpGet("download/{key}")]
        public async Task<IActionResult> DownloadAsync(string key)
        {
            Guid id;
            try
            {
                id = _service.ConvertKeyToId(key);
                return Ok(await _service.GetByIdAsync(id));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
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
            return await _service.GetFilesAsync(userId);
        }

        /// <summary>
        /// Uploading a file to the database.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="textFileRequestDTO"></param>
        /// <returns>Response status.</returns>
        [HttpPost("upload/{userId}")]
        public async Task<ActionResult> UploadAsync(int userId, [FromBody] TextFileRequestDTO textFileRequestDTO)
        {
            try
            {
                var result = await _service.CreateAsync(userId, textFileRequestDTO);
                return Ok(new { result });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
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
                id = _service.ConvertKeyToId(key);
                await _service.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
