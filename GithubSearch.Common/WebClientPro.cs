using System;
using System.Net;

namespace GithubSearch.Common
{
    public class WebClientPro : WebClient
    {
        public int Timeout { get; set; }
        public WebClientPro(int timeout = 2000)
        {
            Timeout = timeout;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            request.Timeout = Timeout;
            request.ReadWriteTimeout = Timeout;
            return request;
        }
    }
}
