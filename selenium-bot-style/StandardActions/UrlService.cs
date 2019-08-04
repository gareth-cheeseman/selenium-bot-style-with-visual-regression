using System;

namespace SeleniumBotStyle.StandardActions
{
    public static class UrlService
    {
        public static string GetFullUrl(this string url)
        {
            void CheckParameter(string s, string message)
            {
                if (string.IsNullOrWhiteSpace(s))
                {
                    throw new Exception(message);
                }
            }

            CheckParameter(url, "No path provided");
            if (url.Contains("http") || url.Contains("file:"))
            {
                return url;
            }

            var baseUrl = Configuration.Configuration.BaseUrl;
            CheckParameter(baseUrl, "No base url provided");
            return baseUrl + url;
        }
    }
}