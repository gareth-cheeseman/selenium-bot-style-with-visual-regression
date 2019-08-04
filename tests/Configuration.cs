using System;
using static SeleniumBotStyle.Configuration.Configuration;

namespace tests
{
    public class Configuration
    {
        public static Configuration Instance => new Configuration();

        private Configuration()
        {
            DefaultTimeout = 5;
            DefaultSettleTime = 250;
            ReportingFolder = System.IO.Path.GetFullPath($"{AppDomain.CurrentDomain.BaseDirectory}/ApprovalFiles");
            Image = $"{ReportingFolder}/Image";
            Error = $"{Image}/Error/";
            Base = $"{Image}/Base/";
            Actual = $"{Image}/Actual/";
            Diff = $"{Image}/Diff/";
        }
    }
}