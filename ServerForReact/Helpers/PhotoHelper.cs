using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Helpers
{
    public static class PhotoHelper
    {
        public static void DeletePhoto(string pathImg)
        {
            if (pathImg != null)
            {
                var directory = Path.Combine(Directory.GetCurrentDirectory(), "images");
                var FilePath = Path.Combine(directory, pathImg);
                System.IO.File.Delete(FilePath);
            }
        }
        public static string AddPhoto(IFormFile photo)
        {
            string fileName = String.Empty;
            if (photo != null)
            {
                string randomFilename = Path.GetRandomFileName() +
                    Path.GetExtension(photo.FileName);

                string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "images");
                fileName = Path.Combine(dirPath, randomFilename);
                using (var file = System.IO.File.Create(fileName))
                {
                    photo.CopyTo(file);
                }
                return randomFilename;
            }
            return null;
        }
    }
}
