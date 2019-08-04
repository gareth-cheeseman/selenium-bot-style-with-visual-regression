using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace SeleniumBotStyle.StandardActions
{
    public class Bot : IDisposable
    {
        public IWebDriver Driver { get; }

        public Bot(IWebDriver driver)
        {
            this.Driver = driver;
        }

        public Bot Visit(string url)
        {
            var redirectChecker = new RedirectChecker(Driver.Url);
            Driver.AreRedirectsStopped(d => redirectChecker.RedirectsAreStopped(d));
            Driver.Navigate().GoToUrl(url.GetFullUrl());
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="isInvisible"></param>
        /// <returns></returns>
        public Element Get(string selector, bool isInvisible = false)
        {
            var by = By.CssSelector(selector);
            var element = isInvisible
                ? Driver.Attempt(by, ExpectedConditions.ElementExists(by), foundElement => foundElement)
                : Driver.Attempt(by, ExpectedConditions.ElementIsVisible(by), foundElement => foundElement);
            return new Element(element);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="isInvisible"></param>
        /// <returns></returns>
        public IEnumerable<Element> GetMany(string selector, bool isInvisible = false)
        {
            var by = By.CssSelector(selector);
            var elements = isInvisible
                ? Driver.Attempt(by, ExpectedConditions.PresenceOfAllElementsLocatedBy(by), foundElements => foundElements)
                : Driver.Attempt(by, ExpectedConditions.VisibilityOfAllElementsLocatedBy(by), foundElements => foundElements);
            return elements.Select(e => new Element(e));
        }

        public Bot WaitFor(By by, int? timespan = null)
        {
            var timeout = timespan ?? Configuration.Configuration.DefaultTimeout;
            new WebDriverWait(Driver, TimeSpan.FromSeconds((double)timeout)).Until(
                ExpectedConditions.ElementIsVisible(by));
            return this;
        }

        public void Dispose()
        {
            Driver?.Quit();
            Driver?.Dispose();
        }
    }
}
