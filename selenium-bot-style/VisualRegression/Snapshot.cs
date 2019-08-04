using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumBotStyle.StandardActions;

namespace SeleniumBotStyle.VisualRegression
{
    public static class Snapshot
    {
        public static void PageShouldMatch(this Bot bot, string imageName, ComparisonOptions options = null)
        {
            Assert.True(bot.Driver.AttemptSnapShot(imageName, (driver, s) =>
            {
                var result = Compare.Differences(imageName, driver, options);
                return result.Match;
            }));
        }

        public static void ElementShouldMatch(this Bot bot, string imageName, By by, ComparisonOptions options = null)
        {
            Assert.True(bot.Driver.AttemptSnapShot(imageName, (driver, s) =>
            {
                var result = Compare.Differences(imageName, by, driver, options);
                return result.Match;
            }));
        }
    }
}