using Microsoft.Extensions.Configuration;
using SecretsSharing.Data.Models;
using SecretsSharing.Data.Repository;
using SecretsSharing.DTO;
using SecretsSharing.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSharing.Service
{
    /// <summary>
    /// Class service for text files.
    /// </summary>
    public class TextFileService
    {
        private readonly IFileRepository<TextFile> _textFileRepository;
        private readonly IConfiguration _iconfiguration;
        private readonly UserRepository _userRepository;
        public TextFileService(IFileRepository<TextFile> repository, IConfiguration iconfiguration,
                               UserRepository userRepository, FileUtils fileUtils)
        {
            _textFileRepository = repository;
            _iconfiguration = iconfiguration;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Getting text from the database by id.
        /// </summary>
        /// <param name="id">Text file id.</param>
        /// <returns>Text file.</returns>
        public async Task<TextFileDTO> GetByIdAsync(Guid id)
        {
            var textFile = await _textFileRepository.GetByIdAsync(id);
            var result = new TextFileDTO
            {
                Name = textFile.Name,
                Text = textFile.Text,
                AddDate = textFile.AddDate,
            };

            var user = await _userRepository.GetByIdAsync(textFile.UserId);
            if (user.AutoDeleteText)
            {
                await _textFileRepository.DeleteAsync(textFile);
            }

            return result;
        }

        /// <summary>
        /// Getting all the user's text files.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns>List of user's text files.</returns>
        public async Task<IEnumerable<TextFileDTO>> GetFilesAsync(int userId)
        {
            var sourse = await _textFileRepository.GetAllByUserIdAsync(userId);
            return sourse.Select(t => new TextFileDTO
            {
                Name = t.Name,
                Text = t.Text,
                AddDate = t.AddDate,
            });
        }

        /// <summary>
        /// Creating a new text file and saving it to the database.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="textFileRequestDTO"></param>
        /// <returns>File access url.</returns>
        public async Task<string> CreateAsync(int userId, TextFileRequestDTO textFileRequestDTO)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            var textFile = new TextFile()
            {
                Id = Guid.NewGuid(),
                Name = textFileRequestDTO.Name,
                Text = textFileRequestDTO.Text,
                AddDate = DateTime.Now,
                User = user,
            };
            await _textFileRepository.CreateAsync(textFile);

            return textFile.GetUrl(_iconfiguration["UrlTextFile"], textFile.Id);
        }

        /// <summary>
        /// Deleting a text file from the database.
        /// </summary>
        /// <param name="id">Text file id.</param>
        public async Task DeleteAsync(Guid id)
        {
            var textFile = new TextFile
            {
                Id = id
            };
            await _textFileRepository.DeleteAsync(textFile);
        }

        /// <summary>
        /// Converts the file key to the file id.
        /// </summary>
        /// <param name="key">The key of the file from the link.</param>
        /// <returns>File Id.</returns>
        public Guid ConvertKeyToId(string key)
        {
            try
            {
                string guid = Encoding.UTF8.GetString(Convert.FromBase64String(key));
                return Guid.Parse(guid);
            }
            catch
            {
                throw new Exception("Failed to conver key.");
            }
        }

    }
}
