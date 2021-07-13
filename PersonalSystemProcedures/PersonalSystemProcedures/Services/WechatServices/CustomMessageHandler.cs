using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.MessageHandlers;
using System.IO;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.NeuChar.Entities;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.AdvancedAPIs.UserTag;

namespace PersonalSystemProcedures.Services
{
    public partial class  CustomMessageHandler : MessageHandler<CustomMessageContext>
    {
        public CustomMessageHandler(Stream inputStream, PostModel postModel) : base(inputStream, postModel)
        {

        }

        /// <summary>
        /// 触发请求前执行
        /// </summary>
        public override void OnExecuting()
        {
            /*
            if (RequestMessage.FromUserName == null)
            {
                CancelExcute = true;
                var responseMessage = CreateResponseMessage<ResponseMessageText>();
                responseMessage.Content = "你已经被拉黑！";
                ResponseMessage = responseMessage;
            }
            */
            base.OnExecuting();
        }

        /// <summary>
        /// 触发请求后执行
        /// </summary>
        public override void OnExecuted()
        {
            base.OnExecuted();
        }

        /// <summary>
        /// 默认回复信息
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            var resopnseMessage = base.CreateResponseMessage<ResponseMessageText>();
            resopnseMessage.Content = "This message come from defaultResponseMessage";
            return resopnseMessage;
        }

        /// <summary>
        /// 接收文本信息
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            AccessTokenResult accessToken = CommonApi.GetToken(AppId, secret);//获取accessToken，从而获取用户列表
            var tags_list = UserTagApi.Get(accessToken.access_token);
            Dictionary<string, int> tags_dict = new Dictionary<string, int>();
            foreach (var i in tags_list.tags)
            {
                tags_dict[i.name] = i.id;
            }
            switch (requestMessage.Content)
            {
                case "学生":
                    {
                        //判断是否已经存在标签
                        if (tags_dict.ContainsKey("学生"))
                        {
                            List<string> list = new List<string>();
                            list.Add(requestMessage.FromUserName);
                            var result = UserTagApi.BatchTagging(accessToken.access_token, tags_dict["学生"], list);
                            responseMessage.Content = "我们将标记您为【学生】身份，日后本公众号将为您提供专门的推送信息。";
                        }
                        else
                        {
                            CreateTagResult bt_result = UserTagApi.Create(accessToken.access_token, "学生");//创建标签
                            List<string> list = new List<string>();
                            list.Add(requestMessage.FromUserName);
                            var result = UserTagApi.BatchTagging(accessToken.access_token, tags_dict["学生"], list);
                            responseMessage.Content = "我们将标记您为【学生】身份，日后本公众号将为您提供专门的推送信息。";
                        }
                        break;
                    }
                case "教师":
                    {
                        //判断是否已经存在标签
                        if (tags_dict.ContainsKey("教师"))
                        {
                            List<string> list = new List<string>();
                            list.Add(requestMessage.FromUserName);
                            var result = UserTagApi.BatchTagging(accessToken.access_token, tags_dict["教师"], list);
                            responseMessage.Content = "我们将标记您为【教师】身份，日后本公众号将为您提供专门的推送信息。";
                        }
                        else
                        {
                            CreateTagResult bt_result = UserTagApi.Create(accessToken.access_token, "教师");//创建标签
                            List<string> list = new List<string>();
                            list.Add(requestMessage.FromUserName);
                            var result = UserTagApi.BatchTagging(accessToken.access_token, tags_dict["教师"], list);
                            responseMessage.Content = "我们将标记您为【教师】身份，日后本公众号将为您提供专门的推送信息。";
                        }
                        break;
                    }
                case "企业人员":
                    {
                        //判断是否已经存在标签
                        if (tags_dict.ContainsKey("企业人员"))
                        {
                            List<string> list = new List<string>();
                            list.Add(requestMessage.FromUserName);
                            var result = UserTagApi.BatchTagging(accessToken.access_token, tags_dict["企业人员"], list);
                            responseMessage.Content = "我们将标记您为【企业人员】身份，日后本公众号将为您提供专门的推送信息。";
                        }
                        else
                        {
                            CreateTagResult bt_result = UserTagApi.Create(accessToken.access_token, "企业人员");//创建标签
                            List<string> list = new List<string>();
                            list.Add(requestMessage.FromUserName);
                            var result = UserTagApi.BatchTagging(accessToken.access_token, tags_dict["企业人员"], list);
                            responseMessage.Content = "我们将标记您为【企业人员】身份，日后本公众号将为您提供专门的推送信息。";
                        }
                        break;
                    }
                case "普通订阅者":
                    {
                        //判断是否已经存在标签
                        if (tags_dict.ContainsKey("普通订阅者"))
                        {
                            List<string> list = new List<string>();
                            list.Add(requestMessage.FromUserName);
                            var result = UserTagApi.BatchTagging(accessToken.access_token, tags_dict["普通订阅者"], list);
                            responseMessage.Content = "我们将标记您为【普通订阅者】身份，日后本公众号将为您提供专门的推送信息。";
                        }
                        else
                        {
                            CreateTagResult bt_result = UserTagApi.Create(accessToken.access_token, "普通订阅者");//创建标签
                            List<string> list = new List<string>();
                            list.Add(requestMessage.FromUserName);
                            var result = UserTagApi.BatchTagging(accessToken.access_token, tags_dict["普通订阅者"], list);
                            responseMessage.Content = "我们将标记您为【普通订阅者】身份，日后本公众号将为您提供专门的推送信息。";
                        }
                        break;
                    }
                default:
                    {
                        //判断是否已经存在标签
                        if (tags_dict.ContainsKey("普通订阅者"))
                        {
                            List<string> list = new List<string>();
                            list.Add(requestMessage.FromUserName);
                            var result = UserTagApi.BatchTagging(accessToken.access_token, tags_dict["普通订阅者"], list);
                            responseMessage.Content = "我们将标记您为【普通订阅者】身份，日后本公众号将为您提供专门的推送信息。";
                        }
                        else
                        {
                            CreateTagResult bt_result = UserTagApi.Create(accessToken.access_token, "普通订阅者");//创建标签
                            List<string> list = new List<string>();
                            list.Add(requestMessage.FromUserName);
                            var result = UserTagApi.BatchTagging(accessToken.access_token, tags_dict["普通订阅者"], list);
                            responseMessage.Content = "我们将标记您为【普通订阅者】身份，日后本公众号将为您提供专门的推送信息。";
                        }
                        break;
                    }
            }
            return responseMessage;
            //List<string> openId_list = new List<string>();
            //openId_list.Add(requestMessage.FromUserName);
            //var result = UserTagApi.BatchTagging(AppId, 1, openId_list);
        }

        /// <summary>
        /// 接收定位信息
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            double location_X = requestMessage.Location_X;
            double location_Y = requestMessage.Location_Y;
            responseMessage.Content = string.Format("维度:{0}  经度:{1}", location_X, location_Y);
            return responseMessage;
        }

        /// <summary>
        /// 接收小视频
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnShortVideoRequest(RequestMessageShortVideo requestMessage)
        {
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "您刚才发送的是小视频";
            return responseMessage;
        }

        /// <summary>
        /// 接收图片信息
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnImageRequest(RequestMessageImage requestMessage)
        {
            //一隔一返回News或Image格式
            if (base.GlobalMessageContext.GetMessageContext(requestMessage).RequestMessages.Count() % 2 == 0)
            {
                var responseMessage = CreateResponseMessage<ResponseMessageNews>();

                responseMessage.Articles.Add(new Article()
                {
                    Title = "您刚才发送了图片信息",
                    Description = "您发送的图片将会显示在边上",
                    PicUrl = requestMessage.PicUrl,
                    Url = "http://120.78.222.9"
                });
                responseMessage.Articles.Add(new Article()
                {
                    Title = "第二条",
                    Description = "第二条带连接的内容",
                    PicUrl = requestMessage.PicUrl,
                    Url = "http://120.78.222.9"
                });

                return responseMessage;
            }
            else
            {
                var responseMessage = CreateResponseMessage<ResponseMessageImage>();
                responseMessage.Image.MediaId = requestMessage.MediaId;
                return responseMessage;
            }
        }

        /// <summary>
        /// 处理未知信息
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnUnknownTypeRequest(RequestMessageUnknownType requestMessage)
        {
            var msgType = Senparc.NeuChar.Helpers.MsgTypeHelper.GetRequestMsgTypeString(requestMessage.RequestDocument);
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "未知消息类型：" + msgType;
            //WeixinTrace.SendCustomLog("未知请求消息类型", requestMessage.RequestDocument.ToString());//记录到日志中
            return responseMessage;
        }


        /// <summary>
        /// 删除标签组的标签
        /// </summary>
        /// <param name="accessTokenOrAppId"></param>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ClearTag(string accessTokenOrAppId, int id, string name)
        {
            WxJsonResult result = UserTagApi.Delete(accessTokenOrAppId, id);
            return true;
        }
        /// <summary>
        /// 删除用户的标签
        /// </summary>
        /// <param name="accessTokenOrAppId"></param>
        /// <param name="tagid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public bool ClearUserTag(string accessTokenOrAppId, int tagid, string openid)
        {
            List<string> list = new List<string>();
            list.Add(openid);
            WxJsonResult result = UserTagApi.BatchUntagging(accessTokenOrAppId, tagid, list);
            return true;
        }
    }
}
