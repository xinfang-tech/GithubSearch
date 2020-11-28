using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace GithubSearch.Common
{
    public class AppSettings
    {
        private static IConfigurationSection appSections = null;

        public static string AppSetting(string key)
        {
            string str = "";
            if (appSections.GetSection(key) != null)
            {
                str = appSections.GetSection(key).Value;
            }
            return str;
        }


        public static void SetAppSetting(IConfigurationSection section)
        {
            appSections = section;
        }
    }
}
