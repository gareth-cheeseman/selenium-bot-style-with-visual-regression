using System.Drawing;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumBotStyle.StandardActions
{
    public class Element
    {
        public IWebElement WebElement { get; set; }

        public Element(IWebElement webElement)
        {
            WebElement = webElement;
        }

        public Element Submit()
        {
            WebElement.Submit();
            return this;
        }

        public Element Click()
        {
            WebElement = WebElement.Attempt(e =>
            {
                if (e.Enabled && e.Displayed)
                {
                    e.Click();
                }
                else
                {
                    Assert.Fail($"element with tag:{e.TagName} and classes: {e.GetAttribute("class")} not clickable");
                }

                return e;
            });
            return this;
        }

        public string Attribute(string attributeName)
        {
            return WebElement.GetAttribute(attributeName);
        }

        public string Property(string propertyName)
        {
            return WebElement.GetProperty(propertyName);
        }

        public string CssValue(string propertyName)
        {
            return WebElement.GetCssValue(propertyName);
        }

        public Element Select(string text)
        {
            WebElement = WebElement.Attempt(e =>
            {
                new SelectElement(e).SelectByText(text);
                return e;
            });
            return this;
        }

        public Element EnterText(string text)
        {
            WebElement = WebElement.Attempt(e =>
            {
                e.Clear();
                e.SendKeys(text);
                return e;
            });

            return this;
        }

        public Element Keystroke(string key)
        {
            WebElement = WebElement.Attempt(e =>
            {
                e.SendKeys(key);
                return e;
            });
            return this;
        }

        public string TagName => WebElement.TagName;
        public string Text => WebElement.Text;
        public bool Enabled => WebElement.Enabled;
        public bool Selected => WebElement.Selected;
        public Point Location => WebElement.Location;
        public Size Size => WebElement.Size;
        public bool Displayed => WebElement.Displayed;
    }
}
