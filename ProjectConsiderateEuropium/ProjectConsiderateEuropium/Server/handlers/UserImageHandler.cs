using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ProjectConsiderateEuropium.Shared.Communication;
using File = ProjectConsiderateEuropium.Shared.Models.File;
using PathString = Microsoft.AspNetCore.Http.PathString;

namespace ProjectConsiderateEuropium.Server.handlers
{
    public interface IUserImageHandler
    {
        public bool ValidateImage(File image);
        public string GetRandomImageFileName(File image);

        public File CreateUserImagePath(File image);

        public void WriteImage(File image);

    }

    public class UserImageHandler : IUserImageHandler
    {
        private readonly IConfiguration _configuration;

        public UserImageHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetRandomImageFileName(File image)
        {
            if (!ValidateImage(image))
                return null;

            var extension = image.Extension;
            var fileName = $"img-{Guid.NewGuid()}{extension}";//ex: img-fasdf-456-asd98.jpg
            return fileName;
        }

        public File CreateUserImagePath(File image)
        {
            //path
            var path = _configuration.GetSection("SystemPaths").GetValue<string>("PathToImgStorage");

            //name
            var name = GetRandomImageFileName(image);

            //full path
            path = Path.GetFullPath(path, name);

            //add
            image.Path = path;

            //return
            return image;
        }

        public void WriteImage(File image)
        {
            if (string.IsNullOrWhiteSpace(image.Path))
                return;

            using var fs = new FileStream(image.Path, FileMode.CreateNew);
            if (!fs.CanWrite)
                throw new IOException("can not write image");

            fs.Write(image.Data, 0, image.Data.Length);
        }

        public bool ValidateImage(File image)
        {
            if (image?.Data == null)
                return false;

            if (!image.HasExtension)
                return false;

            switch (image.Extension)
            {
                case ".jpg":
                case ".png":
                case ".svg":
                    return true;
                default:
                    return false;
            }
        }
    }
}
