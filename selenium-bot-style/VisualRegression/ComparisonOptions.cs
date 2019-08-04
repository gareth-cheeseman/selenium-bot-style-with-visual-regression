namespace SeleniumBotStyle.VisualRegression
{
    public class ComparisonOptions
    {
        public byte Threshold { get; set; }
        public bool CreateDifferenceImage { get; set; }

        public ComparisonOptions()
        {
            CreateDifferenceImage = true;
        }
    }
}