using Microsoft.AspNetCore.Mvc;
using SecretsSharing.Data.Models;
using SecretsSharing.Data.Repository;
using System.Collections.Generic;
using System.Text;
using System;
using SecretsSharing.Service;
using SecretsSharing.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace SecretsSharing.Controllers
{
    [ApiController]
    [Route("rest/text")]
    public class TextFileController : ControllerBase
    {
        private readonly TextFileService _textFileService;
        //MjZjODU3MjItN2FjZS0xMWVkLWExZWItMDI0MmFjMTIwMDAy

        public TextFileController(TextFileService textFileService)
        {
            _textFileService = textFileService;
        }

        [HttpGet("{url}")]
        public TextDTO GetTextFile(string url)
        {    
            return _textFileService.GetTextFile(url);
        }

        [HttpGet("getfiles/{userId}")]
        public IEnumerable<TextFileDTO> GetTextFileAllByUserId(Guid userId)
        {
            return _textFileService.GetTextFileAllByUserId(userId);
        }

        [HttpPost("create")]
        public UrlResponse CreateTextFile(TextFileDTO textFileDTO)
        {
            return _textFileService.CreateTextFile(textFileDTO);
        }

        //[HttpDelete]
        //public IEnumerable<TextFile> DeleteTextFile()
        //{

        //    return _textFileRepository.GetTextFile();
        //}

    }
}
