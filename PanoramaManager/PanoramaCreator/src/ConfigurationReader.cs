using System;
using System.Configuration;

namespace DimitriVranken.PanoramaCreator
{
    class ConfigurationReader
    {
        // TODO: Implement

        private static string GetValue(string key, string defaultValue)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            string AppSetting = ConfigurationManager.AppSettings.Get(key);
            if (AppSetting == null)
            {
                throw new Exception(key);
            }

            return AppSetting;
        }

        private static bool GetValue(string key, int defaultValue)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            return Convert.ToBoolean(GetValue(key, string.Empty));
        }

        private static bool GetBoolean(string key, bool defaultValue)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            return Convert.ToBoolean(GetValue(key, string.Empty));
        }
    }
}
