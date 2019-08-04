using OpenQA.Selenium;

namespace SeleniumBotStyle.StandardActions
{
    public class RedirectChecker
    {
        private string CurrentUrl { get; set; }
        private string LastUrl { get; set; }
        private int count = 0;

        public bool RedirectsAreStopped(IWebDriver driver)
        {
            if (count <= 0)
            {
                count++;
                return false;
            }

            CurrentUrl = driver.Url;

            if (!LastUrl.Equals(CurrentUrl))
            {
                LastUrl = CurrentUrl;
                return false;
            }

            return (LastUrl.Equals(CurrentUrl) && ((IJavaScriptExecutor) driver)
                    .ExecuteScript("return document.readyState")
                    .ToString().ToLower()
                    .Equals("complete"));
        }

        public RedirectChecker(string lastUrl)
        {
            LastUrl = lastUrl;
        }
    }
}