using System.IO;
using NUnit.Framework;
using OpenQA.Selenium;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SeleniumBotStyle.VisualRegression
{
    public static class Compare
    {
        private static readonly string BaseDirectory = Configuration.Configuration.Base;

        public static ComparisonResult Differences(string imageName, By by, IWebDriver driver,
            ComparisonOptions options = null)
        {
            var baseImageFile = $"{BaseDirectory}{imageName}.png";

            Image<Rgba32> baseImage;
            if (File.Exists(baseImageFile))
            {
                baseImage = Image.Load(baseImageFile);
            }
            else
            {
                baseImage = Screenshotter.GetElementScreenShot(driver, by, Screenshotter.GetFullPageScreenshot(driver));
                baseImageFile = baseImage.SaveAs(ImageType.Base, imageName);
            }

            TestContext.AddTestAttachment(baseImageFile, "base image");

            var actualImage = Screenshotter.GetElementScreenShot(driver, by, Screenshotter.GetFullPageScreenshot(driver));
            var actualImageFile = actualImage.SaveAs(ImageType.Actual, imageName);
            TestContext.AddTestAttachment(actualImageFile);

            return Differences(imageName, baseImage, actualImage, options);
        }

        public static ComparisonResult Differences(string imageName, IWebDriver driver,
            ComparisonOptions options = null)
        {
            var baseImageFile = $"{BaseDirectory}{imageName}.png";
            Image<Rgba32> baseImage;
            if (File.Exists(baseImageFile))
            {
                baseImage = Image.Load(baseImageFile);
            }
            else
            {
                baseImage = Screenshotter.GetFullPageScreenshot(driver);
                baseImageFile = baseImage.SaveAs(ImageType.Base, imageName);
            }

            TestContext.AddTestAttachment(baseImageFile, "base image");

            var actualImage = Screenshotter.GetFullPageScreenshot(driver);
            var actualImageFile = actualImage.SaveAs(ImageType.Actual, imageName);

            TestContext.AddTestAttachment(actualImageFile);

            return Differences(imageName, baseImage, actualImage, options);
        }

        private static ComparisonResult Differences(string imageName, Image<Rgba32> expected, Image<Rgba32> actual,
            ComparisonOptions options = null)
        {
            if (options == null) options = new ComparisonOptions();

            var differences = CompareCore.GetDifferenceMatrix(expected, actual, options.Threshold);
            var diffPixels = CompareCore.CountDifferingPixels(differences);
            var differencePercentage = diffPixels / (float)differences.Length;

            var result = new ComparisonResult { Match = diffPixels == 0, DifferencePercentage = differencePercentage };

            if (!result.Match && options.CreateDifferenceImage)
            {
                var diffImage = CompareCore.GetDifferenceImage(expected, differences);
                result.DifferenceImage = diffImage;
                diffImage.SaveAs(ImageType.Diff, imageName);
            }

            return result;
        }
    }
}