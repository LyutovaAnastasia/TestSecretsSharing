using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SecretsSharing.DTO;
using System.Collections.Generic;
using SecretsSharing.Util;
using SecretsSharing.Service;
using System;
using Microsoft.AspNetCore.Authorization;

namespace SecretsSharing.Controllers
{
    /// <summary>
    /// Class controller for document file.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("rest/document")]
    public class DocumentFileController : ControllerBase
    {

        private readonly IDocumentService _documentFileService;
        private readonly IUserServise _userService;
        private readonly FileUtils _fileUtils;

        public DocumentFileController(IDocumentService service,
                                      FileUtils fileUtils, IUserServise userService)
        {
            _documentFileService = service;
            _fileUtils = fileUtils;
            _userService = userService;
        }

        /// <summary>
        /// Downloading a file from the storage.
        /// </summary>
        /// <param name="key">Url file key.</param>
        /// <returns>Document file.</returns>
        [AllowAnonymous]
        [HttpGet("download/{key}")]
        public async Task<IActionResult> DownloadAsync(string key)
        {
            try
            {
                var id = _documentFileService.ConvertKeyToId(key);

                // Downloads from database.
                var result = await _documentFileService.GetByIdAsync(id);

                // Downloads  from storage.
                var content = await _fileUtils.DownloadFileAsync(result.FilePath);
                var file = File(content, "application/octet-stream", result.Name);

                var userSettingsDTO = await _userService.GetUserSettingsByIdAsync(result.UserId);
                if (userSettingsDTO.AutoDeleteDocument)
                {
                    // Deleting from the storage.
                    _fileUtils.DeleteFile(result.FilePath);

                    // Deleting from the database.
                    await _documentFileService.DeleteAsync(id);

                }
                return file;
            }
            catch (Exception ex)
            {
                return BadRequest(new { messageError = ex.Message });
            }
        }

        /// <summary>
        /// Getting all the user's document files.
        /// </summary>
        /// <param name="userId">List of user's document files.</param>
        /// <returns>List of user's document files.</returns>
        [HttpGet("getAll/{userId}")]
        public async Task<IEnumerable<DocumentFileDTO>> GetFilesAsync(int userId)
        {
            return await _documentFileService.GetFilesAsync(userId);
        }

        /// <summary>
        /// Uploading a file to the database.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="file">The file to be uploaded.</param>
        /// <returns>File access url.</returns>
        [HttpPost("upload/{userId}")]
        public async Task<IActionResult> UploadAsync(int userId, IFormFile file)
        {
            try
            {
                // Upload file in the storage.
                var fileInfo = await _fileUtils.UploadFileAsync(file);
                // Create file in database.
                var result = await _documentFileService.CreateAsync(userId, fileInfo);
                return Ok(new { result });
            }
            catch(Exception ex)
            {
                return BadRequest(new { messageError = ex.Message });
            }
        }

        /// <summary>
        /// Deleting a document file from the database.
        /// </summary>
        /// <param name="key">Url file key.</param>
        /// <returns>Response status.</returns>
        [HttpDelete("delete/{key}")]
        public async Task<IActionResult> DeleteAsync(string key)
        {
            try
            {
                var id = _documentFileService.ConvertKeyToId(key);

                var result = await _documentFileService.GetByIdAsync(id);
                // Deleting from storage.
                _fileUtils.DeleteFile(result.FilePath);

                // Deleting from the database.
                await _documentFileService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { messageError = ex.Message });
            }
        }
    }
}
