using System;
namespace GithubSearch.Common.Models
{
    public class CodeModel:BaseMongoModel
    {
        public CodeModel()
        {
        }
        public string url { get; set; }
        public string match_codes { get; set; }
        public string hash { get; set; }
        public string code { get; set; }
        public string repository { get; set; }
        public string path { get; set; }

    }
}
