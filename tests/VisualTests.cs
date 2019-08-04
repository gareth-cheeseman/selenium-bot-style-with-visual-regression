using System;
using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumBotStyle.VisualRegression;

namespace tests
{
    public class VisualTests : TestBase
    {
        [Test]
        public void TestForFullPageMatch()
        {
            Bot.Visit($"file:///{AppDomain.CurrentDomain.BaseDirectory}/ApprovalFiles/TestPage.html");

            //testing image comparison
            Bot.PageShouldMatch("FullPageShot");
        }

        [Test]
        public void TestForElementMatch()
        {
            Bot.Visit($"file:///{AppDomain.CurrentDomain.BaseDirectory}/ApprovalFiles/TestPage.html");

            //testing element image comparison
            Bot.ElementShouldMatch("ElementShot", By.Id("id5"));
        }

        [Test]
        public void TestForFullPageMatchFail()
        {
            Bot.Visit($"file:///{AppDomain.CurrentDomain.BaseDirectory}/ApprovalFiles/TestPage.html");

            //testing image comparison
            var comparisonResult = Compare.Differences("FullPageShotFail", Bot.Driver);
            Assert.False(comparisonResult.Match);
        }

        [Test]
        public void TestForElementMatchFail()
        {
            Bot.Visit($"file:///{AppDomain.CurrentDomain.BaseDirectory}/ApprovalFiles/TestPage.html");

            //testing element image comparison
            var comparisonResult = Compare.Differences("ElementShotFail", By.Id("id5"), Bot.Driver);
            Assert.False(comparisonResult.Match);
        }
    }
}