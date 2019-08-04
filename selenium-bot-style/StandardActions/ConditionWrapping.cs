using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumBotStyle.StandardActions
{
    public static class ConditionWrapping
    {
        private static WebDriverWait Wait(this IWebDriver driver, int? timeout = null)
        {
            if (timeout == null) timeout = Configuration.Configuration.DefaultTimeout;
            return new WebDriverWait(driver, TimeSpan.FromSeconds((double)timeout));
        }

        internal static IWebElement Condition(IWebDriver driver, Func<IWebDriver, IWebElement> condition, int? timeout = null)
        {
            return driver.Wait(timeout).Until(condition);
        }

        internal static IEnumerable<IWebElement> Condition(IWebDriver driver, Func<IWebDriver, IEnumerable<IWebElement>> condition, int? timeout = null)
        {
            return driver.Wait(timeout).Until(condition);
        }

        internal static void Condition(IWebDriver driver, Func<IWebDriver, bool> condition, int? timeout = null)
        {
            driver.Wait(timeout).Until(condition);
        }
    }
}