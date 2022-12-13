using Microsoft.Extensions.Configuration;
using SecretsSharing.Data.Models;
using SecretsSharing.Data.Repository;
using SecretsSharing.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretsSharing.Service
{
    public class TextFileService
    {
        private readonly TextFileRepository _textFileRepository;
        private IConfiguration _iconfiguration;
        public TextFileService(TextFileRepository textFileRepository, IConfiguration iconfiguration)
        {
            _textFileRepository = textFileRepository;
            _iconfiguration = iconfiguration;
        }

        public TextDTO GetTextFile(String url)
        {
            string guid = Encoding.UTF8.GetString(Convert.FromBase64String(url));
            if (!string.IsNullOrEmpty(guid))
            {
                Guid id = Guid.Parse(guid);
                var textFile = _textFileRepository.GetTextFile(id);
                return new TextDTO
                {
                    Text = textFile.Text,
                };
            }
            return null;
        }

        public IEnumerable<TextFileDTO> GetTextFileAllByUserId(Guid userId)
        {
            return _textFileRepository.GetTextFileAllByUserId(userId).Select(t => new TextFileDTO
            {
                Name = t.Name,
                Text = t.Text,
            });

        }

        public UrlResponse CreateTextFile(TextFileDTO textFileDTO)
        {
            var textFile = new TextFile()
            {
                Id = Guid.NewGuid(),
                Name = textFileDTO.Name,
                Text = textFileDTO.Text,
            };
            _textFileRepository.CreateTextFile(textFile);
            // пока так
            string url = _iconfiguration.GetValue<string>("UrlTextFile")
                       + Convert.ToBase64String(Encoding.UTF8.GetBytes(Convert.ToString(textFile.Id)));
            return new UrlResponse()
            {
                Url = url
            };
        }

    }
}
