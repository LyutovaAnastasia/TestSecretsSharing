using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using SecretsSharing.Data.Repository;
using SecretsSharing.Data.Models;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace SecretsSharing.Controllers
{
    [ApiController]
    [Route("rest/[controller]")]
    public class UserController : ControllerBase // or : Controller ?
    {
        //private readonly TextFileRepository _textFileRepository;

        //public UserController(TextFileRepository textFileRepository)
        //{
        //    _textFileRepository = textFileRepository;
        //}

        //[HttpGet]
        //public IEnumerable<TextFile> GetTextFileAll()
        //{
        //    string a = Convert.ToBase64String(Encoding.UTF8.GetBytes("26c85722-7ace-11ed-a1eb-0242ac120002"));
        //    Console.WriteLine(a);
        //    return _textFileRepository.GetTextFile();
        //}
    }
}
