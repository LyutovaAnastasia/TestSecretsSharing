using Microsoft.Extensions.Configuration;
using SecretsSharing.Data.Models;
using SecretsSharing.Data.Repository;
using SecretsSharing.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSharing.Service.impl
{
    public class TextFileService
    {
        private readonly IFileRepository<TextFile> _repository;
        private readonly IConfiguration _iconfiguration;
        private readonly UserRepository _userRepository;
        public TextFileService(IFileRepository<TextFile> repository, IConfiguration iconfiguration,
                               UserRepository userRepository)
        {
            _repository = repository;
            _iconfiguration = iconfiguration;
            _userRepository = userRepository;   
        }

        public async Task<TextFileDTO> DownloadAsync(string key)
        {
            string guid = Encoding.UTF8.GetString(Convert.FromBase64String(key));
            if (!string.IsNullOrEmpty(guid))
            {
                Guid id = Guid.Parse(guid);
                var textFile = await _repository.GetByIdAsync(id);
                return new TextFileDTO
                {
                    Name = textFile.Name,
                    Text = textFile.Text,
                    AddDate = textFile.AddDate,
                };
            }
            return null;
        }

        public async Task<IEnumerable<TextFileDTO>> GetFilesAsync(Guid userId)
        {
            var sourse = await _repository.GetAllByUserIdAsync(userId);
            return sourse.Select(t => new TextFileDTO
            {
                Name = t.Name,
                Text = t.Text,
                AddDate = t.AddDate,
            });
        }

        public async Task<string> UploadAsync(Guid userId, TextFileRequestDTO textFileRequestDTO)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                Console.WriteLine("throw");
            }

            var textFile = new TextFile()
            {
                Id = Guid.NewGuid(),
                Name = textFileRequestDTO.Name,
                Text = textFileRequestDTO.Text,
                AddDate = DateTime.Now,
                User= user,
            };
            await _repository.CreateAsync(textFile);
            string url = _iconfiguration.GetValue<string>("UrlTextFile")
                       + Convert.ToBase64String(Encoding.UTF8.GetBytes(Convert.ToString(textFile.Id)));
            return url;
        }

        public async Task DeleteAsync(string key)
        {
            string guid = Encoding.UTF8.GetString(Convert.FromBase64String(key));
            if (!string.IsNullOrEmpty(guid))
            {
                var textFile = new TextFile
                {
                    Id = Guid.Parse(guid)
                };
                await _repository.DeleteAsync(textFile);
            }
        }
    }
}
