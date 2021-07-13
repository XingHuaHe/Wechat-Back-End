using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin;
using Senparc.CO2NET.Extensions;

using PersonalSystemProcedures.DBContext;
using PersonalSystemProcedures.Models;
using Senparc.Weixin.Entities;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PersonalSystemProcedures.Controllers
{
    public class OAuth2Controller : Controller
    {
        public string appId = "wx4255d34ce90babd8"; //测试号appid
        public string secret = "b113d5a45aa4fb8a25a77aa2c63efa57"; //测试号APPSECRET
        public string returnUrl = string.Empty; //存储向微信提交申请地址（获得code）;
        OAuthAccessTokenResult accesstoken = null; //存储code换取access_token返回的JSON数据
        OAuthUserInfo usermessage = null; //存储openid access_token换取用户JSON数据

        //定义微信数据库
        private readonly WeChatContext _weChatContext;

        public OAuth2Controller(WeChatContext context)
        {
            _weChatContext = context;
        }


        /// <summary>
        /// 回调处理函数：将重定向地址 redirectUrl 添加到微信特点网站的参数，发送给微信平台，微信平台回验证其他参数后，自动定向到 redirectUrl
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            var state = "JeffreySu-" + DateTime.Now.Millisecond;//随机数，用于识别请求可靠性；
            HttpContext.Session.SetString("State", state);
            string redirectUrl = "http://120.78.222.9/OAuth2/UserAccessToken"; //回调地址,通常为需要授权业务网页（域名与微信后台一致）
            //string code_url = OAuthApi.GetAuthorizeUrl(appId, redirectUrl, state, Senparc.Weixin.MP.OAuthScope.snsapi_userinfo);
            string code_url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state={2}#wechat_redirect",
                appId, redirectUrl.UrlEncode(), state);
            return Redirect(code_url);
        }

        /// <summary>
        /// 微信平台重 定向 redirectUrl 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult UserAccessToken(string code, string state)
        {
            //获取用户access_token;
            if (string.IsNullOrEmpty(code))
            {
                return Content("codenullorempty"); //若没有授权，跳转到snsapi_base方式访问
            }
            if (state != HttpContext.Session.GetString("State"))
            {
                //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下，
                //HttpContext.Session.Remove("State");
                return Content("验证失败！请从正规途径进入！");
            }
            try
            {
                accesstoken = OAuthApi.GetAccessToken(appId, secret, code);
            }
            catch (Exception ex)
            {
                //return Content(ex.Message);
                return Content("This");
            }

            if (accesstoken.errcode != ReturnCode.请求成功)
            {
                return Content("错误：" + accesstoken.errmsg);
            }

            //return Content("OK!");
            
            //判断access_token是否有效，刷新access_token
            WxJsonResult result = OAuthApi.Auth(accesstoken.access_token, accesstoken.openid);
            if (result.errcode == ReturnCode.access_token超时)
            {
                accesstoken = OAuthApi.RefreshToken(appId, accesstoken.access_token);
            }

            //获取用户信息
            try
            {
                usermessage = OAuthApi.GetUserInfo(accesstoken.access_token, accesstoken.openid);
            }
            catch
            {
                return Content("GetUserInfo error!");
            }
            //OpenIDPra.UserOpenID = usermessage.openid;
            //WechatNamePra.UserWechatName = usermessage.nickname;
            //return Redirect("http://www.qingshuihe.xyz/Reservation/reser.html");
            return Content(usermessage.openid.ToString());
        }

        /// <summary>
        /// 预览菜单入口
        /// </summary>
        /// <returns></returns>
        public ActionResult Index_pre()
        {
            var state = "JeffreySu-" + DateTime.Now.Millisecond;
            HttpContext.Session.SetString("pr", state);
            string redirectUrl = "";//回调地址，通常为需要授权的业务网页
            return Redirect(redirectUrl);
        }
    }
}
