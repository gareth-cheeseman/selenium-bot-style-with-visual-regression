using System;
using System.IO;
using NUnit.Framework;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SeleniumBotStyle.VisualRegression
{
    public static class FileSaverLoader
    {
        public static string SaveAs(this Image<Rgba32> image, ImageType imageType, string imageName = null)
        {
            var directory = GetDirectoryFor(imageType);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var fileName = GetFileNameFor(imageType, imageName);
            var fullPath = directory + fileName;
            image.Save($"{fullPath}.png");
            return $"{fullPath}.png";
        }

        private static string GetDirectoryFor(ImageType type)
        {
            switch (type)
            {
                case ImageType.Error: return Configuration.Configuration.Error;
                case ImageType.Base: return Configuration.Configuration.Base;
                case ImageType.Actual: return Configuration.Configuration.Actual;
                case ImageType.Diff: return Configuration.Configuration.Diff;
                default: throw new ArgumentException("Incorrect image save directory passed");
            }
        }

        private static string GetFileNameFor(ImageType imageType, string imageName)
        {
            string fileName;
            if (imageType is ImageType.Error)
            {
                fileName = Guid.NewGuid().ToString();
            }
            else
            {
                fileName = imageName ?? TestContext.CurrentContext.Test.Name;
            }

            return fileName;
        }
    }
}