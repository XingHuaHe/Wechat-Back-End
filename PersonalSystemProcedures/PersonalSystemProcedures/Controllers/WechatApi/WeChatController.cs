using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.CO2NET.HttpUtility;
using PersonalSystemProcedures.Services;
using Senparc.Weixin.MP.MvcExtension;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
/*
 * 修改日期：2019/8/6
 * 
 * 该控制器的主要作用有两个：
 * （1）处理微信公众平台提交的服务器认证。
 * （2）响应订阅者与公众号互动。
 */

namespace PersonalSystemProcedures.Controllers
{
    public class WeChatController : Controller
    {

        public static readonly string Token = "ResearchGroup";//微信端服务器配置：Token
        public static readonly string EncodingAESKey = "R69tZKSMVWOgXdK7tCXZA0ono3qos3SQFT24xmiTgXv";//微信端服务器配置：EncodingAESKey
        public static readonly string AppId = "wx4255d34ce90babd8";//微信端服务器配置：AppId

        /// <summary>
        /// 公众号服务器配置认证
        /// </summary>
        /// <param name="postModel"></param>
        /// <param name="echostr"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Token")]
        public ActionResult Get(PostModel postModel, string echostr)
        {
            if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
            {
                return Content(echostr); //返回随机字符串则表示验证通过
            }
            else
            {
                return Content("failed:" + postModel.Signature + "," + CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, Token) + "。" +
                    "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
            }
        }

        /// <summary>
        /// 响应微信公众号用户信息
        /// </summary>
        /// <param name="postModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Token")]
        public ActionResult Post(PostModel postModel)
        {
            if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
            {
                return Content("参数错误！");
            }

            #region 打包 PostModel 信息

            postModel.Token = Token;//根据自己后台的设置保持一致
            postModel.EncodingAESKey = EncodingAESKey;//根据自己后台的设置保持一致
            postModel.AppId = AppId;//根据自己后台的设置保持一致（必须提供）

            #endregion

            //v4.2.2之后的版本，可以设置每个人上下文消息储存的最大数量，防止内存占用过多，如果该参数小于等于0，则不限制
            //var maxRecordCount = 10;

            //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
            //var messageHandler = new CustomMessageHandler(Request.GetRequestMemoryStream(), postModel, maxRecordCount);
            var messageHandler = new CustomMessageHandler(Request.GetRequestMemoryStream(), postModel);
            #region 设置消息去重

            /* 如果需要添加消息去重功能，只需打开OmitRepeatedMessage功能，SDK会自动处理。
             * 收到重复消息通常是因为微信服务器没有及时收到响应，会持续发送2-5条不等的相同内容的RequestMessage*/
            messageHandler.OmitRepeatedMessage = true;//默认已经开启，此处仅作为演示，也可以设置为false在本次请求中停用此功能

            #endregion

            try
            {
                //messageHandler.SaveRequestMessageLog();//记录 Request 日志（可选）

                messageHandler.Execute();//执行微信处理过程（关键）

                //messageHandler.SaveResponseMessageLog();//记录 Response 日志（可选）

                //return Content(messageHandler.ResponseDocument.ToString());//v0.7-
                //return new WeixinResult(messageHandler);//v0.8+
                return new FixWeixinBugWeixinResult(messageHandler);//为了解决官方微信5.0软件换行bug暂时添加的方法，平时用下面一个方法即可
            }
            catch (Exception ex)
            {
                return Content("回复出问题了！");
            }
        }
    }
}
