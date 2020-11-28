using System;
using System.Collections.Generic;
using System.Text;

namespace GithubSearch.Common
{
    public static class ApplicationEnvironments
    {
        private static SiteConfig site;

        public static SiteConfig Site
        {
            get
            {
                return site;
            }
            set
            {
                site = value;
            }
        }
    }
}
