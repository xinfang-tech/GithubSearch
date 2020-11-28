using GithubSearch.Common;
using GithubSearch.Common.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GithubSearch.CaptureWork
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var githubToken = ApplicationEnvironments.Site.GithubToken;
            if (githubToken == null || githubToken.Length == 0)
            {
                _logger.LogInformation("GithubToken Not Set;未设置");
                Console.ReadLine();
                return;
            }

            MongoContext context = new MongoContext();
            var runWorkerLog = context.DataBase.GetCollection<BsonDocument>("RunWorkerLog");
            while (!stoppingToken.IsCancellationRequested)
            {
                runWorkerLog.InsertOne(BsonDocument.Parse("{\"date\":\"" + DateTime.Now + "\",\"msg\":\"" + "开始" + "\"}"));
                var list = ApplicationEnvironments.Site.CaptureList;
                foreach (var codes in list)
                {
                    runWorkerLog.InsertOne(BsonDocument.Parse("{\"date\":\"" + DateTime.Now + "\",\"msg\":\"" + "codes:" + codes + "\"}"));
                    string match_codes = codes;
                    await SearchGitHubCodeAsync(match_codes);
                }

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000 * 60 * 60 * 4, stoppingToken);//4个小时一次
            }
            runWorkerLog.InsertOne(BsonDocument.Parse("{\"date\":\"" + DateTime.Now + "\",\"msg\":\"" + "结束" + "\"}"));
        }

        private static async Task SearchGitHubCodeAsync(string match_codes, int page = 1, string dbstr = "")
        {
            string githubToken = ApplicationEnvironments.Site.GithubToken;
            string strUrl = "https://api.github.com/search/code?page=" + page + "&q=" + match_codes + "+in:file";
            var res = await GetUrlBody(githubToken, strUrl);

            if (string.IsNullOrWhiteSpace(res))
            {
                Console.WriteLine("Error:" + match_codes + "-" + page);
                return;
            }
            Console.WriteLine("INFO:" + match_codes + "-" + page);

            JObject jObject = JObject.Parse(res);

            long totalCount = Int64.Parse(jObject["total_count"].ToString());
            int totalPage = (int)Math.Floor(totalCount / 30.0) + 1;
            Console.WriteLine(totalCount + "/" + totalPage);


            int itemsLength = jObject["items"].Count();

            var items = jObject["items"];
            Console.WriteLine(itemsLength);


            MongoContext context = new MongoContext(dbstr);
            var TabCodeModel = context.DataBase.GetCollection<CodeModel>("CodeModel");

            for (int i = 0; i < itemsLength; i++)
            {
                var codeModel = new CodeModel()
                {
                    url = items[i]["html_url"].ToString(),
                    hash = items[i]["sha"].ToString(),
                    match_codes = match_codes,
                    path = items[i]["path"].ToString(),
                    repository = items[i]["repository"].ToString(),
                };


                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("hash", codeModel.hash);
                var query = new BsonDocument(dict);

                var oldList = TabCodeModel.FindSync<CodeModel>(query).ToList();

                if (oldList.Any())
                {
                    continue;
                }


                string codeContent = await GetUrlBody(githubToken, codeModel.url);
                if (string.IsNullOrWhiteSpace(res))
                {
                    Console.WriteLine("Error02:" + codeModel.url);
                    continue;
                }
                codeModel.code = codeContent;
                await TabCodeModel.InsertOneAsync(codeModel);
                string strMsg = "Codes:" + codeModel.match_codes + "" + " \nPath:" + codeModel.path + " \nURL:" + codeModel.url;
                MessageHelper.SendWxMsg(strMsg);

            }

            if (totalPage > page)
            {
                Console.WriteLine(totalPage + ":" + page);
                await SearchGitHubCodeAsync(match_codes, page + 1);
            }
        }

        private static async Task<string> GetUrlBody(string githubToken, string url)
        {
            try
            {
                Console.WriteLine(DateTime.Now.ToString() + ":" + url);
                WebClientPro clientContent = new WebClientPro(5000);//请求超时时间 time out set
                clientContent.Encoding = Encoding.UTF8;
                clientContent.Headers.Add("Authorization", "token " + githubToken);
                clientContent.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36");
                var codeContent = await clientContent.DownloadStringTaskAsync(new Uri(url));
                Thread.Sleep(2000);
                return codeContent;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }

        }
    }
}
