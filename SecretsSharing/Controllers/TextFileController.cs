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
        private readonly IService<TextFileDTO, UrlResponse> _service;
        //MjZjODU3MjItN2FjZS0xMWVkLWExZWItMDI0MmFjMTIwMDAy

        public TextFileController(IService<TextFileDTO, UrlResponse> service)
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

        [HttpPost("upload")]
        public async Task<UrlResponse> UploadAsync(TextFileDTO textFileDTO)
        {
            return await _service.UploadAsync(textFileDTO);
        }

        [HttpDelete("delete/{key}")]
        public async Task DeleteAsync(string key)
        {
           await _service.DeleteAsync(key);
        }

    }
}
