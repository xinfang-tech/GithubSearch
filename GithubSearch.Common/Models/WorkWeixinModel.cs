using System;
using System.Collections.Generic;
using System.Text;

namespace GithubSearch.Common.Models
{
    public class WorkWeixinModel
    {
        public string msgtype { get; set; }
        public WorkWeixinTextModel text { get; set; }
    }
    public class WorkWeixinTextModel
    {
        public string content { get; set; }
    }
}
