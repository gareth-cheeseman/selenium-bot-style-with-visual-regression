using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using OpenQA.Selenium;

namespace SeleniumBotStyle.StandardActions
{
    public static class ExceptionHandling
    {
        public static IEnumerable<IWebElement> Attempt(this IWebDriver driver, By by,
            Func<IWebDriver, IEnumerable<IWebElement>> condition,
            Func<IEnumerable<IWebElement>, IEnumerable<IWebElement>> actionBody,
            [CallerMemberName] string memberName = "", int? timeout = null,
            int retries = 0)
        {
            IEnumerable<IWebElement> elements = new List<IWebElement>();

            try
            {
                elements = actionBody(ConditionWrapping.Condition(driver, condition, timeout));
            }
            catch (Exception exception) when (exception is StaleElementReferenceException && retries < 1)
            {
                retries++;
                Attempt(driver, by, condition, actionBody, memberName, retries: retries);
            }
            catch (Exception exception)
            {
                ExceptionMessaging.ExceptionMessage(by, exception, memberName, timeout);

                Assert.Fail();
            }

            return elements;
        }

        public static IWebElement Attempt(this IWebDriver driver, By by, Func<IWebDriver, IWebElement> condition,
            Func<IWebElement, IWebElement> actionBody, [CallerMemberName] string memberName = "", int? timeout = null, int retries = 0)
        {
            IWebElement element = null;

            try
            {
                element = actionBody(ConditionWrapping.Condition(driver, condition, timeout));
            }
            catch (Exception exception) when (exception is StaleElementReferenceException && retries < 1)
            {
                retries++;
                Attempt(driver, by, condition, actionBody, memberName, retries: retries);
            }
            catch (Exception exception)
            {
                ExceptionMessaging.ExceptionMessage(by, exception, memberName, timeout);
                Assert.Fail();
            }
            return element;
        }

        public static IWebElement Attempt(this IWebElement element,
            Func<IWebElement, IWebElement> actionBody, [CallerMemberName] string memberName = "", int retries = 0)
        {
            IWebElement actionElement = null;
            try
            {
                actionElement = actionBody(element);
            }
            catch (Exception exception) when (exception is StaleElementReferenceException && retries < 1)
            {
                retries++;
                Attempt(element, actionBody, memberName, retries);
            }
            catch (Exception exception)
            {
                ExceptionMessaging.ExceptionMessage(element, exception, memberName);
                Assert.Fail(exception.Message);
            }

            return actionElement;
        }

        public static void AreRedirectsStopped(this IWebDriver driver,
            Func<IWebDriver, bool> condition, int? timeout = null)
        {
            try
            {
                ConditionWrapping.Condition(driver, condition);
            }
            catch (Exception e)
            {
                ExceptionMessaging.ExceptionMessage("redirects did not settle");
                Assert.Fail(e.Message);
            }
        }

        public static bool AttemptSnapShot(this IWebDriver driver, string name, Func<IWebDriver, string, bool> func)
        {
            bool snapShotsMatch = false;
            try
            {
                snapShotsMatch = func(driver, name);

            }
            catch (Exception exception)
            {
                Assert.Fail(exception.Message);
            }

            return snapShotsMatch;
        }
    }


}
