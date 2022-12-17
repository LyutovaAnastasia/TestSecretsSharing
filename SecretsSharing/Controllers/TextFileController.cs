using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using SecretsSharing.DTO;
using System.Threading.Tasks;
using SecretsSharing.Service.impl;
using SecretsSharing.Service;

namespace SecretsSharing.Controllers
{
    [ApiController]
    [Route("rest/text")]
    public class TextFileController : ControllerBase
    {
        private readonly TextFileService _service;
        // user 58e707b7-22ec-478e-8f0b-124a35e98b65

        public TextFileController(TextFileService service)
        {
            _service = service;
        }

        [HttpGet("download/{key}")]
        public async Task<TextFileDTO> DownloadAsync(string key)
        {    
            return await _service.DownloadAsync(key);
        }

        [HttpGet("getAll/{userId}")]
        public async Task<IEnumerable<TextFileDTO>> GetFilesAsync(Guid userId)
        {
            return await _service.GetFilesAsync(userId);
        }

        [HttpPost("upload/{userId}")]
        public async Task<ActionResult> UploadAsync(Guid userId, TextFileRequestDTO textFileRequestDTO)
        {
            var result = await _service.UploadAsync(userId, textFileRequestDTO);
            return result != null ? Ok( new { result }) : BadRequest();
        }

        [HttpDelete("delete/{key}")]
        public async Task DeleteAsync(string key)
        {
           await _service.DeleteAsync(key);
        }

    }
}
