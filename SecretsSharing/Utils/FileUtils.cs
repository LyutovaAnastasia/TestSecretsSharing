using Microsoft.AspNetCore.Http;
using System.IO;
using System;

namespace SecretsSharing.Util
{
    public static class FileUtils
    {
        //public static bool ValidateFile(IFormFile file)
        //{
        //    // Null validation
        //    if (file.Length <= 0)
        //    {
        //        //_logger.LogInformation("Error upload file. The file is Empty.");
        //        return false;
        //    }

        //    var untrustedFileName = Path.GetFileName(file.FileName);
        //    var ext = Path.GetExtension(untrustedFileName).ToLowerInvariant();

        //    // File extension validation
        //    if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
        //    {
        //        //_logger.LogInformation("Error upload file. The extension of file is invalid.");
        //        return false;
        //    }

        //    // Size validation
        //    if (file.Length > _fileSizeLimit)
        //    {
        //        //_logger.LogInformation("Error upload file. The file is too large.");
        //        return false;
        //    }

        //    return true;
        //}

        //public static string CreateFilePath(IFormFile file)
        //{
        //    var untrustedFileName = Path.GetFileName(file.FileName);
        //    var ext = Path.GetExtension(untrustedFileName).ToLowerInvariant();
        //    var filename = Guid.NewGuid().ToString() + ext;
        //    return Path.Combine(_iconfiguration["StoredFilesPackage"], filename);
        //}
    }
}
