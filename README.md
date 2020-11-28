# Github Search
## 基于GitHub Token 查询关键字
Git代码泄露检查，接入企业微信


## 资料
### Github Token 设置
https://github.com/settings/tokens/

### 企业微信机器人(Webhook URL)
```
1.准备一个群。进入企业微信，你需要先准备一个群才能建立一个群机器人
2.添加群机器人。使用鼠标右键群，在弹出菜单选择“添加群机器人”
3.成功添加机器人。添加成功，会在你选择的群展示你添加的机器
4.编辑或发布机器人,可以看到URL，取出URL的key 参数，可以配置到“GithubSearch.CaptureWork/appsettings.json”的"WeiXin_NoticeKey"中
```
