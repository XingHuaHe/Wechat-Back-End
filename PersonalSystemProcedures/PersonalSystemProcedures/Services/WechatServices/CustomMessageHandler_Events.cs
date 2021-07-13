using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Senparc.NeuChar.Entities;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.Media;

namespace PersonalSystemProcedures.Services
{
    public partial class CustomMessageHandler
    {//此处类名与另外一个函数一样，实际是将一个类所包含的功能分开两个.cs文件编写，使不同类型的函数分开，便于开发

        public static readonly string AppId = "wx4255d34ce90babd8";
        public string secret = "b113d5a45aa4fb8a25a77aa2c63efa57"; //测试号APPSECRET

        /// <summary>
        /// 订阅回复的信息
        /// </summary>
        /// <returns></returns>
        private string GetWelcomeInfo()
        {
            return string.Format(@"欢迎关注我们的课题组微信公众号。
为了方便我们管理，请在公众号回复以下关键字：
【学生】
【教师】
【企业人员】
【普通订阅者】
注意：默认情况下为普通订阅者，可能将影响我们后续为您提供的信息的服务。");
        }

        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageText>();//定义回复信息实例
            responseMessage.Content = this.GetWelcomeInfo();//添加内容
            return responseMessage;
        }

        /// <summary>
        /// 取消订阅事件
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_UnsubscribeRequest(RequestMessageEvent_Unsubscribe requestMessage)
        {
            return base.OnEvent_UnsubscribeRequest(requestMessage);
        }

        /// <summary>
        /// 点击文本事件
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnTextOrEventRequest(RequestMessageText requestMessage)
        {
            // 预处理文字或事件类型请求。
            // 这个请求是一个比较特殊的请求，通常用于统一处理来自文字或菜单按钮的同一个执行逻辑，
            // 会在执行OnTextRequest或OnEventRequest之前触发，具有以下一些特征：
            // 1、如果返回null，则继续执行OnTextRequest或OnEventRequest
            // 2、如果返回不为null，则终止执行OnTextRequest或OnEventRequest，返回最终ResponseMessage
            // 3、如果是事件，则会将RequestMessageEvent自动转为RequestMessageText类型，其中RequestMessageText.Content就是RequestMessageEvent.EventKey

            if (requestMessage.Content == "OneClick")
            {
                var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                strongResponseMessage.Content = "您点击了底部按钮。\r\n为了测试微信软件换行bug的应对措施，这里做了一个——\r\n换行";
                return strongResponseMessage;
            }
            return null;//返回null，则继续执行OnTextRequest或OnEventRequest
        }

        /// <summary>
        /// 打开网站事件
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_ViewRequest(RequestMessageEvent_View requestMessage)
        {
            //说明：这条消息只作为接收，下面的responseMessage到达不了客户端，类似OnEvent_UnsubscribeRequest
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "您点击了view按钮，将打开网页：" + requestMessage.EventKey;
            return responseMessage;
        }

        /// <summary>
        /// 响应菜单点击事件
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            IResponseMessageBase reponseMessage = null;
            ResponseMessageText responseMessage = CreateResponseMessage<ResponseMessageText>();
            int flag = 0;
            switch (requestMessage.EventKey)
            {
                case "1_1"://在研项目
                    {
                        AccessTokenResult accessToken = CommonApi.GetToken(AppId, secret);
                        MediaList_NewsResult imgeText = MediaApi.GetNewsMediaList(accessToken.access_token, 0, 1);
                        var result = CustomApi.SendMpNews(accessToken.access_token, "oYD_Q54tfI6RAZP19n8euugXy0qM", imgeText.item[0].media_id);
                        break;
                    }
                case "1_2"://文章
                    {
                        break;
                    }
                case "1_3"://专利
                    {
                        AccessTokenResult accessToken = CommonApi.GetToken(AppId, secret);
                        var menuContentList = new List<SendMenuContent>()
                        {
                            new SendMenuContent("s:101","学生"),
                            new SendMenuContent("s:102","教师"),
                            new SendMenuContent("s:103","企业人员"),
                            new SendMenuContent("s:104","普通人员")
                        };
                        CustomApi.SendMenuAsync(accessToken.access_token, requestMessage.FromUserName, "请对 Senparc.Weixin SDK 给出您的评价", menuContentList, "感谢您的参与！");
                        //responseMessage.Content = requestMessage.FromUserName;
                        break;
                    }
                case "1_4"://软件著作
                    {
                        break;
                    }
                case "1_5"://科研产品
                    {
                        break;
                    }
                case "2_1"://科研动态
                    {
                        break;
                    }
                case "2_2"://学生简介
                    {
                        break;
                    }
                case "2_3"://文体活动
                    {
                        break;
                    }
                case "2_4"://通知公告
                    {
                        break;
                    }
                case "2_5"://会议交流
                    {
                        break;
                    }
                case "3_1"://国家项目
                    {
                        break;
                    }
                case "3_2"://省级项目
                    {
                        break;
                    }
                case "3_3"://横向课题
                    {
                        break;
                    }
                case "3_4"://合作单位
                    {
                        break;
                    }
                case "3_5"://联系我们
                    {
                        flag = 1;
                        string context = string.Format(@"商务咨询、转载、科研合作，请联系：88888888
产品技术请访问：http://www.tecsonde.cn
专利请访问：

感谢您对电子科技大学人工智能及高端装备研究中心的支持！
");
                        responseMessage.Content = context;
                        break;
                    }
            }
            if (flag == 0)
            {
                reponseMessage = new ResponseMessageNoResponse();
                return reponseMessage;
            }
            else
            {
                return responseMessage;
            }
        }
    }
}
