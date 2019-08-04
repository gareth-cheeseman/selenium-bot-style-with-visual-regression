using System.Collections.Generic;
using System.Linq;

namespace SeleniumBotStyle.StandardActions
{
    public static class ElementExtensions
    {
        public static Element Contains(this IEnumerable<Element> elements, string text)
        {
            return elements.First(e => e.WebElement.Text.Contains(text));
        }
    }
}