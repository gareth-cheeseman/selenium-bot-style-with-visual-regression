using System;
using System.Runtime.CompilerServices;
using OpenQA.Selenium;

namespace SeleniumBotStyle.StandardActions
{
    public static class ExceptionMessaging
    {

        internal static void ExceptionMessage(By by, Exception exception, [CallerMemberName] string memberName = "", int? timeout = null)
        {
            if (timeout == null) timeout = Configuration.Configuration.DefaultTimeout;
            Console.WriteLine(
                $"Could not perform \"{memberName}\" on element identified by \"{by}\" after {timeout} seconds. Exception: {exception.Message}");
        }

        internal static void ExceptionMessage(IWebElement element, Exception exception, [CallerMemberName] string memberName = "", int? timeout = null)
        {
            if (timeout == null) timeout = Configuration.Configuration.DefaultTimeout;
            Console.WriteLine(
                $"Could not perform \"{memberName}\" on element with tag \"{element.TagName}\" after {timeout} seconds. Exception: {exception.Message}");
        }

        internal static void ExceptionMessage(string message, [CallerMemberName] string memberName = "", int? timeout = null)
        {
            if (timeout == null) timeout = Configuration.Configuration.DefaultTimeout;
            Console.WriteLine($"Could not perform \"{memberName}\" after {timeout} seconds {message}");
        }

    }
}