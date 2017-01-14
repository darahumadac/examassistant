using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamAssistant.Models
{
    public interface IConfigurationReader
    {
        string GetSetting(string key);
    }

    public class WebConfigReader : IConfigurationReader
    {
        public string GetSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

    }

    public static class ExamConfiguration
    {
        private static readonly IConfigurationReader _configurationReader = new WebConfigReader();

        public static string GetSetting(string key)
        {
            return _configurationReader.GetSetting(key);
        }
    }
}
