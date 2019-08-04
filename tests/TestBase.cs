using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;
using System.Reflection;
using SeleniumBotStyle.StandardActions;

namespace tests
{
    public class TestBase
    {
        private IWebDriver driver;
        internal Bot Bot;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Configuration configuration = Configuration.Instance;
        }

        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions();
            options.AddArgument("window-size=1400,975");
            options.AddArgument("headless");
            driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), options);
            Bot = new Bot(driver);
        }

        [TearDown]
        public void TearDown()
        {
            Bot?.Dispose();
        }
    }
}