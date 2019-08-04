using System.Collections.Generic;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace SeleniumBotStyle.VisualRegression
{
    public static class Screenshotter
    {
        public static Image<Rgba32> GetFullPageScreenshot(IWebDriver driver)
        {
            // Get the total size of the page
            var totalWidth =
                (int)(long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.offsetWidth");
            var totalHeight =
                (int)(long)((IJavaScriptExecutor)driver).ExecuteScript(
                    "return  document.body.parentNode.scrollHeight");

            // Get the size of the viewport
            var viewportWidth =
                (int)(long)((IJavaScriptExecutor)driver).ExecuteScript("return document.documentElement.clientWidth");
            var viewportHeight = (int)(long)((IJavaScriptExecutor)driver).ExecuteScript("return window.innerHeight");

            // We only care about taking multiple images together if it doesn't already fit
            if (totalWidth <= viewportWidth && totalHeight <= viewportHeight)
            {
                return driver.TakeScreenshot().ScreenShotToImage();
            }

            // Split the screen in multiple Rectangles
            var rectangles = new List<Rectangle>();
            // Loop until the totalHeight is reached
            for (var y = 0; y < totalHeight; y += viewportHeight)
            {
                var newHeight = viewportHeight;
                // Fix if the height of the element is too big
                if (y + viewportHeight > totalHeight) newHeight = totalHeight - y;
                // Loop until the totalWidth is reached
                for (var x = 0; x < totalWidth; x += viewportWidth)
                {
                    var newWidth = viewportWidth;
                    // Fix if the Width of the Element is too big
                    if (x + viewportWidth > totalWidth) newWidth = totalWidth - x;
                    // Create and add the Rectangle
                    var currRect = new Rectangle(x, y, newWidth, newHeight);
                    rectangles.Add(currRect);
                }
            }

            // Build the Image
            var stitchedImage = new Image<Rgba32>(totalWidth, totalHeight);

            //Ensure viewport is at top of page
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0,0)");

            // Get all Screenshots and stitch them together
            var previous = Rectangle.Empty;
            foreach (var rectangle in rectangles)
            {
                // Calculate the scrolling (if needed)
                if (previous != Rectangle.Empty)
                {
                    var xDiff = rectangle.Right - previous.Right;
                    var yDiff = rectangle.Bottom - previous.Bottom;
                    // Scroll
                    ((IJavaScriptExecutor)driver).ExecuteScript(string.Format("window.scrollBy({0}, {1})", xDiff,
                        yDiff));
                }


                var screenShotImage = ScreenShotToImage(driver.TakeScreenshot());

                // Calculate the source Rectangle
                var sourceRectangle = new Rectangle(viewportWidth - rectangle.Width, viewportHeight - rectangle.Height,
                    rectangle.Width, rectangle.Height);
                // Crop screenShotImage to source Rectangle
                screenShotImage.Mutate(i => i.Crop(sourceRectangle));

                // Copy the Image
                stitchedImage.Mutate(i => i.DrawImage(screenShotImage, new Point(rectangle.X, rectangle.Y), 1));

                // Set the Previous Rectangle
                previous = rectangle;
            }

            return stitchedImage;
        }


        public static Image<Rgba32> GetElementScreenShot(IWebDriver driver, By by, Image<Rgba32> fullPageScreenshot)
        {
            var element = driver.FindElement(by);
            var rectangle = new Rectangle(element.Location.X, element.Location.Y, element.Size.Width,
                element.Size.Height);
            var elementScreenshot = fullPageScreenshot.Clone(i => i.Crop(rectangle));
            return elementScreenshot;
        }


        private static Image<Rgba32> ScreenShotToImage(this Screenshot screenshot)
        {
            Image<Rgba32> screenShotImage;
            using (var memStream = new MemoryStream(screenshot.AsByteArray))
            {
                screenShotImage = Image.Load(memStream);
            }

            return screenShotImage;
        }
    }
}