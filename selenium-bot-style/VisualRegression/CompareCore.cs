using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using RectangleF = SixLabors.Primitives.RectangleF;

namespace SeleniumBotStyle.VisualRegression
{
    public static class CompareCore
    {
        private const int CellSize = 16;

        public static int CountDifferingPixels(byte[,] differences)
        {
            var diffPixels = 0;
            foreach (var cellValue in differences)
                if (cellValue > 0)
                    diffPixels++;
            return diffPixels;
        }

        public static Image<Rgba32> GetDifferenceImage(Image<Rgba32> expected, byte[,] differences)
        {
            var differenceImage = expected.Clone();
            IBrush<Rgba32> brush = Brushes.Percent20(Rgba32.Magenta);

            for (var y = 0; y < differences.GetLength(1); y++)
            {
                for (var x = 0; x < differences.GetLength(0); x++)
                {
                    var cellValue = differences[x, y];
                    if (cellValue > 0)
                    {
                        var cellRectangle =
                            new RectangleF((float)x * CellSize, (float)y * CellSize, CellSize, CellSize);
                        differenceImage.Mutate(i => i.Fill(new GraphicsOptions(), brush, cellRectangle));
                    }
                }
            }

            return differenceImage;
        }

        public static byte[,] GetDifferenceMatrix(Image<Rgba32> expected, Image<Rgba32> actual, byte threshold)
        {
            var width = expected.Width / CellSize;
            var height = expected.Height / CellSize;
            var differences = new byte[width, height];
            var expectedSmall = expected.Clone(i => i.Resize(width, height));
            var actualSmall = actual.Clone(i => i.Resize(width, height));

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var cellValue1 = expectedSmall[x, y].R;
                    var cellValue2 = actualSmall[x, y].R;
                    var cellDifference = (byte)Math.Abs(cellValue1 - cellValue2);
                    if (cellDifference < threshold) cellDifference = 0;
                    differences[x, y] = cellDifference;
                }
            }


            return differences;
        }
    }
}