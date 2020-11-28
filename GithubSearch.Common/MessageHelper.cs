using GithubSearch.Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace GithubSearch.Common
{
    public class MessageHelper
    {
        public static void SendWxMsg(string strMsg)
        {
            string WeiXinNoticeKey = ApplicationEnvironments.Site.WeiXin_NoticeKey;
            if (string.IsNullOrWhiteSpace(WeiXinNoticeKey))
            {
                Console.WriteLine("Not Set WeiXinNoticeKey;  没有配置企业微信‘消息机器人’");
                return;
            }
            Thread.Sleep(2000); //消息机器人每分钟限制发送条数，增加延时

            string wxUrl = "https://qyapi.weixin.qq.com/cgi-bin/webhook/send?key=" + WeiXinNoticeKey;

            WorkWeixinModel msg = new WorkWeixinModel()
            {
                msgtype = "text",
                text = new WorkWeixinTextModel()
                {
                    content = strMsg
                }
            };

            var stJson = JsonConvert.SerializeObject(msg);
            WebClientPro webClient = new WebClientPro();
            webClient.Encoding = Encoding.UTF8;
            webClient.UploadString(wxUrl, stJson);


        }
    }
}
