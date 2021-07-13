修改日期：2019/8/7

一、微信公众号号：
微信公众号主要采用盛派提供的开源框架Senparc
使用前，需要导入以下几个库
（1）Senparc.Weixin
（2）Senparc.Weixin.MP
（3）Senparc.Weixin.MP.MVC

项目的主要的几个程序文件介绍：
（1）OAuth2Controller.cs：负责获取用户信息，以及重新定向业务网页（与业务网页定向有关的处理，都在这个函数上，因为该文件主要是与微信平台交流，获取用户信息）
（2）WeChatController.cs：负责微信公众号平台域名验证，以及接收、返回 用户与公众号互动信息
（3）SystemApiController.cs：处理业务信息，包括业务网站上的数据存储与查询发送
（4）Services.WechatServices.cs：处理用户与与公众号互动信息，例如：接收用户文本信息，返回特点的信息等
（5）Models.WechatServices文件夹下是数据库相应的表


二、数据库
数据库采用 postgresql
//https://www.cnblogs.com/xuqp/p/9707469.html

使用前需要导入相应的库文件，在 tools->NuGet控制台 依次输入以下指令：
（1）Install-Package Npgsql.EntityFrameworkCore.PostgreSQL
（2）Install-Package Npgsql.EntityFrameworkCore.PostgreSQL.Design
（3）Install-Package Microsoft.EntityFrameworkCore.Tools

项目主要的几个程序文件介绍：
（1）DBContext.WeChatContext.cs：定义数据库，连接相应的表
（2）Models文件夹下是数据库相应的表


Add-Migration MyFirstMigration -Context TuShanContext
Update-Database MyFirstMigration -Context TuShanContext


三、后端接口



四、手机后端接口
（1）Controllers文件夹下，MobileSystemApi文件夹下为手机登录和其他接口。
（2）DBContext文件夹下，是手机系统后端数据库。
（3）Models文件夹下MobileModels文件夹存储着数据库表以及其他模型。
（4）Services文件夹下MobileServices文件夹存储接口.cs
（5）Interfaces文件夹是接口文件