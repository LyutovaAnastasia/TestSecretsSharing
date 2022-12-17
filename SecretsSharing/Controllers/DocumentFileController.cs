using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SecretsSharing.Service.impl;
using SecretsSharing.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SecretsSharing.Data.Repository;

namespace SecretsSharing.Controllers
{
    [ApiController]
    [Route("rest/document")]
    public class DocumentFileController : ControllerBase
    {

        private readonly DocumentFileService _service;
        private readonly IConfiguration _iconfiguration;

        // user 58e707b7-22ec-478e-8f0b-124a35e98b65

        public DocumentFileController(DocumentFileService service, IConfiguration iconfiguration)
        {
           _service = service;
            _iconfiguration = iconfiguration;
        }

        [HttpGet("download/{key}")]
        public async Task<IActionResult> DownloadAsync(string key)
        {
            var result = await _service.DownloadAsync(key);
            if (result != null)
            {
                var filePath = Path.Combine(_iconfiguration["VirtualPathParticle"], result.FilePath);
                return File(filePath, "application/octet-stream", result.Name);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("getAll/{userId}")]
        public async Task<IEnumerable<DocumentFileDTO>> GetFilesAsync(Guid userId)
        {
            return await _service.GetFilesAsync(userId);
        }

        [HttpPost("upload/{userId}")]
        public async Task<IActionResult> UploadAsync(Guid userId, IFormFile file)
        {
            var result = await _service.UploadAsync(userId, file);
            return result != null ? Ok(new { result }) : BadRequest(new { message = "Empty file" });  
        }


        [HttpDelete("delete/{key}")]
        public async Task DeleteAsync(string key)
        {
            await _service.DeleteAsync(key);
        }
    }
}
