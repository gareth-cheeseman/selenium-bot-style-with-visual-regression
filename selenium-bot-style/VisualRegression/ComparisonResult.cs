using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SeleniumBotStyle.VisualRegression
{
    public class ComparisonResult
    {
        public bool Match { get; set; }
        public float DifferencePercentage { get; set; }
        public Image<Rgba32> DifferenceImage { get; set; }
    }
}