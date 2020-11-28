using System;
using System.Collections.Generic;
using System.Text;

namespace GithubSearch.Common
{
    public class SiteConfig
    {
        public string MongoDB { get; set; }

        public string MongoDB_Develop { get; set; }
        public string WeiXin_NoticeKey { get; set; }

        public string[] CaptureList { get; set; }
        public string GithubToken { get; set; }
    }
}