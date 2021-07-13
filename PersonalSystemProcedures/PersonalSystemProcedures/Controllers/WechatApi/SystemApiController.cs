using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using PersonalSystemProcedures.DBContext;
using PersonalSystemProcedures.Models.WechatModels;
using Senparc.Weixin.CommonAPIs;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.Helpers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PersonalSystemProcedures.Controllers.SystemApi
{
    [Produces("application/json")]
    [Route("Wechat/api/SystemApi")]
    public class SystemApiController : Controller
    {
        string appId = "xxxxxxx";//测试号appId
        string secret = "asdasdasd";//APPSECRET

        private readonly WeChatContext _weChatContext;
        public SystemApiController(WeChatContext context)
        {
            _weChatContext = context;
        }

        [HttpPost]
        [Route("Login")]
        public bool Login()
        {
            IFormCollection req = Request.Form;
            StringValues id, passwd;
            System.Collections.Generic.ICollection<string> t3;

            t3 = req.Keys;
            string[] tt = new string[20];
            int i = 0;
            foreach (string x in t3)
            {
                tt[i++] = x;
            }
            if (i != 2)
                return false;

            req.TryGetValue(tt[0], out id);//ID
            req.TryGetValue(tt[1], out passwd);//password

            if ((string.Compare(id, null) == 0) || (string.Compare(passwd, null) == 0))
            {
                return false;
            }
            bool tableempty = true;
            foreach (var ty in _weChatContext.Admins)
            {
                tableempty = false;
                break;
            }
            if (tableempty)             //当管理员表为空时  登录账号 密码为SuperAdministrator SuperAdministrator
            {
                if ((string.Compare((string)id, "SuperAdministrator") == 0) && (string.Compare((string)passwd, "SuperAdministrator") == 0))
                    return true;
                else return false;
            }

            var db = _weChatContext.Admins.Find(id);

            if (db == null)
                return false;
            else
            {
                if (string.Compare(db.AdminPasswd, passwd) == 0)
                {
                    var linq = (from obj in _weChatContext.Admins
                                where obj.AdminNumber == id
                                select obj).SingleOrDefault();

                    //linq.AdminOnline = true;
                    //linq.AdminLoginTime = System.DateTime.Now;

                    int row = _weChatContext.SaveChanges();
                    if (row > 0)
                    {
                        return true;
                    }
                    else return false;

                }
                else return false;
            }
        }

        /// <summary>
        /// 预定模块
        /// </summary>
        /// <returns></returns>
        /********预定*******/
        [HttpPost]
        [Route("PreInsert")]
        public string PreInsert()
        {
            //OpenIDPra.UserOpenID = "123";

            IFormCollection req = Request.Form;
            String[] val = new String[20];
            System.Collections.Generic.ICollection<string> t3;

            t3 = req.Keys;
            string[] nam = new string[20];
            int i = 0;
            StringValues te;
            foreach (string x in t3)
            {
                nam[i] = x;
                req.TryGetValue(x, out te);
                val[i] = te;
                i++;
            }

            if (i != 5)
            {
                return "0";
            }
            /*******产生随机码******/
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = "";
            str += "0123456789";
            str += "abcdefghijklmnopqrstuvwxyz";
            str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (int j = 0; j < 4; j++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }

            /***********用户航拍照片文件夹***********/
            string target = "wwwroot/Picture/";// + OpenIDPra.UserOpenID;

            try
            {
                // Determine whether the directory exists.
                if (!Directory.Exists(target))
                {
                    // Create the directory it does not exist.
                    Directory.CreateDirectory(target);
                }
            }
            catch (Exception e)
            {
                return "0";
            }


            /***********用户信息表***********/
            int row_info = 0;
            var linq = (from obj in _weChatContext.UserInfos
                        where obj.openid == "123"//OpenIDPra.UserOpenID
                        select obj).SingleOrDefault();

            if (object.Equals(linq, null))
            {
                var dest_info = new UserInfo
                {
                    nickname = "12312"
                    /*
                    UserNumber = Guid.NewGuid(),
                    UserName = "" + val[0] + "",                 //姓名
                    UserSex = "" + null + "",                    //性别
                    UserWechatName = "" + WechatNamePra.UserWechatName + "",
                    UserId = "" + null + "",
                    UserContactPhone = "" + val[1] + "",            //手机号

                    UserOpenId = "" + OpenIDPra.UserOpenID + "",
                    UserContactEmail = "" + null + "",
                    UserFacepict = "" + null + "",
                    UserPicTime = System.DateTime.Now,
                    Remark = "" + null + ""
                    */
                };
                _weChatContext.UserInfos.Add(dest_info);
                row_info = _weChatContext.SaveChanges();
            }
            else
            {
                //linq.UserName = val[0];
                //linq.UserWechatName = WechatNamePra.UserWechatName;
                //linq.UserContactPhone = val[1];
                //linq.UserId = AccessTokenPra.UserAccessToken;
                row_info = _weChatContext.SaveChanges();
            }
            return "0";

            /***********用户航拍表***********/
            /*
            var dest_pic = new Useraerial
            {
                UserOpenId = "" + OpenIDPra.UserOpenID + "",
                UserName = "" + val[0] + "",                    //姓名
                UserChoosePict = "" + null + "",
                Numbers = "" + val[4] + "",                     //拍照张数
                ProcessState = "" + "reserDone" + "",

                Randomstr = "" + s + "",
                UserReservationPos = "" + val[2] + "",          //航拍地点
                UserReservationTime = "" + val[3] + "",         //航拍时间
                PicStorePath = "" + target + "",
                UserPicTime = System.DateTime.Now,
                Remark = "" + null + ""
            };
            _weChatContext.Useraerials.Add(dest_pic);
            int row_pic = _weChatContext.SaveChanges();

            if (row_info > 0 && row_pic > 0)
            {
                var sendstr = "您已经成功预定" + val[2] + "," + val[3] + "的航拍，验证码为"
                    + s + "," + "航拍张数：" + val[4] + "。进入个人中心->我的航拍，可查看历史订单和航拍照片。祝您旅行愉快！";
                //SendMsg(sendstr, AccessTokenPra.UserAccessToken);//发送信息到微信公众号
                return s;
            }
            else return "0";
            */
        }


        /***********预定成功向用户发送消息**********/
        private static WxJsonResult SendMsg(string contentText, string accMsg)
        {

            string URL_FORMAT = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}";
            //定义JSON数据  
            var data = new
            {
                //touser = OpenIDPra.UserOpenID,
                msgtype = "text",
                text = new
                {
                    content = contentText
                }
            };
            return CommonJsonSend.Send(accMsg, URL_FORMAT, data);
        }

        /********判断用户是否关注公众号，返回0时为未关注，1为关注*******/
        [HttpPost]
        [Route("Subscribe")]
        public int Subscribe()
        {
            /*
            if (HttpContext.Session.GetString("re") == null)
            {
                return 2;
            }
            UserInfoJson usermes = null;
            var accMsg = AccessTokenContainer.TryGetAccessToken(appId, secret);
            AccessTokenPra.UserAccessToken = accMsg;
            usermes =UserApi.Info("wxc222968bebebe075", OpenIDPra.UserOpenID, Language.zh_CN);
            return usermes.subscribe;*/
            return 0;
        }

        /// <summary>
        /// 预览模块
        /// </summary>
        /// <returns></returns>
        /********预览*******/
        [HttpPost]
        [Route("Preview")]
        public string Preview()
        {
            /*
            if (HttpContext.Session.GetString("pr") == null)
            {
                return "codenull";
            }
            
            var query = from obj in _weChatContext.Useraerials
                        where obj.UserOpenId == OpenIDPra.UserOpenID
                        select new Useraerial
                        {
                            UserOpenId = obj.UserOpenId,
                            UserName = obj.UserName,
                            UserPicTime = obj.UserPicTime,
                            Remark = obj.Remark,
                            UserChoosePict = obj.UserChoosePict,

                            Numbers = obj.Numbers,
                            ProcessState = obj.ProcessState,
                            Randomstr = obj.Randomstr,
                            UserReservationPos = obj.UserReservationPos,
                            UserReservationTime = obj.UserReservationTime,

                            PicStorePath = obj.PicStorePath
                        };

            var res = query.ToList();
            if (res.Count == 0)
            {
                return "none";
            }
            for (int j = 0; j < res.Count; j++)
            {
                if (res[j].ProcessState == "takephotoDone")
                {
                    return res[j].ProcessState;
                }
            }
            return res[0].ProcessState;
            */
            return "0";
        }

        /**********获取图片名称**************/
        [HttpPost]
        [Route("Previewpic")]
        public JArray Previewpic()
        {
            JArray picname = new JArray();
            string target = "wwwroot/Picture/";// + OpenIDPra.UserOpenID;
            string[] files = Directory.GetFiles(target, "*.jpg");
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Contains("\\"))
                {
                    files[i] = files[i].Replace("\\", "/");
                }
                if (files[i].Contains("wwwroot/Picture"))
                {
                    files[i] = files[i].Replace("wwwroot/Picture", "Picture");
                }
                picname.Add(files[i]);
            }
            return picname;
        }


        /// <summary>
        /// 个人设置模块
        /// </summary>
        /// <returns></returns>
        /********个人设置*******/
        [HttpPost]
        [Route("PsonSet")]
        public JObject PsonSet()
        {
            if (HttpContext.Session.GetString("pson") == null)
            {
                return null;
            }

            JObject perinfo = new JObject();
            var query = from obj in _weChatContext.UserInfos
                        where obj.openid == "123123"//OpenIDPra.UserOpenID
                        select new UserInfo
                        {
                            nickname = "12312"
                            /*
                            UserNumber = obj.UserNumber,
                            UserOpenId = obj.UserOpenId,
                            UserName = obj.UserName,
                            UserSex = obj.UserSex,
                            UserWechatName = obj.UserWechatName,
                            UserId = obj.UserId,

                            UserContactPhone = obj.UserContactPhone,
                            UserContactEmail = obj.UserContactEmail,
                            UserFacepict = obj.UserFacepict,
                            UserPicTime = obj.UserPicTime,
                            Remark = obj.Remark,
                            */
                        };

            var res = query.ToList();

            if (res.Count == 0)
            {
                perinfo.Add(new JProperty("record", "" + "no_record" + ""));
            }
            else
            {
                perinfo.Add(new JProperty("record", "" + "has_record" + ""));
                //perinfo.Add(new JProperty("UserName", "" + res[0].UserName + ""));
                //perinfo.Add(new JProperty("UserSex", "" + res[0].UserSex + ""));
                //perinfo.Add(new JProperty("UserId", "" + res[0].UserId + ""));
                //perinfo.Add(new JProperty("UserContactPhone", "" + res[0].UserContactPhone + ""));
                //perinfo.Add(new JProperty("UserContactEmail", "" + res[0].UserContactEmail + ""));
            }
            return perinfo;
        }

        [HttpPost]
        [Route("PsonSetUpdata")]
        public bool PsonSetUpdata()
        {
            IFormCollection req = Request.Form;
            String[] val = new String[20];
            System.Collections.Generic.ICollection<string> t3;

            t3 = req.Keys;
            string[] nam = new string[20];
            int i = 0;
            StringValues te;
            foreach (string x in t3)
            {
                nam[i] = x;
                req.TryGetValue(x, out te);
                val[i] = te;
                i++;
            }

            if (i != 6)
            {
                return false;
            }

            if (val[0] == "0")
            {
                /***********建立用户信息表***********/
                var dest_info = new UserInfo
                {
                    nickname = "qeqwe"
                    /*
                    UserNumber = Guid.NewGuid(),
                    UserName = "" + val[1] + "",                    //姓名
                    UserSex = "" + val[2] + "",
                    UserWechatName = "" + WechatNamePra.UserWechatName + "",
                    UserId = "" + val[3] + "",
                    UserContactPhone = "" + val[4] + "",            //手机号

                    UserOpenId = "" + OpenIDPra.UserOpenID + "",
                    UserContactEmail = "" + val[5] + "",
                    UserFacepict = "" + null + "",
                    UserPicTime = System.DateTime.Now,
                    Remark = "" + null + ""
                    */
                };
                _weChatContext.UserInfos.Add(dest_info);
                int row_info = _weChatContext.SaveChanges();
                return true;
            }
            else
            {
                /***********更新用户信息表***********/
                var linq = (from obj in _weChatContext.UserInfos
                            where obj.openid == "sdasd"//OpenIDPra.UserOpenID
                            select obj).SingleOrDefault();

                if (object.Equals(linq, null))
                {
                    return false;
                }

                //linq.UserName = val[1];
                //linq.UserSex = val[2];
                //linq.UserWechatName = WechatNamePra.UserWechatName;
                //linq.UserId = val[3];
                //linq.UserContactPhone = val[4];
                //linq.UserContactEmail = val[5];

                int row = _weChatContext.SaveChanges();
                return true;
            }
        }


        /// <summary>
        /// 我的航拍模块
        /// </summary>
        /// <returns></returns>
        /********我的航拍*******/
        /*
        [HttpPost]
        [Route("Myaerial")]
        public JArray Myaerial()
        {
            OpenIDPra.UserOpenID = "123";
            
             if (HttpContext.Session.GetString("myaerial") == null)
             {
                 return null;
             }
            
            JObject staff1 = new JObject();
            JArray staff = new JArray();

            var query = from obj in _weChatContext.Useraerials
                        where obj.UserOpenId ==  OpenIDPra.UserOpenID
                        select new Useraerial
                        {
                            UserName = obj.UserName,
                            Numbers = obj.Numbers,
                            ProcessState = obj.ProcessState,
                            Randomstr = obj.Randomstr,
                            UserReservationPos = obj.UserReservationPos,
                            UserReservationTime = obj.UserReservationTime,
                            timeconsuming = obj.timeconsuming,
                            moneyconsuming = obj.moneyconsuming,
                        };
            var res = query.ToList();

            if (res.Count == 0)
            {
                staff1.Add(new JProperty("order", "" + "no_order" + ""));
                staff.Add(new JObject(staff1));
                staff1.RemoveAll();
            }
            else
            {
                for (int i = 0; i < res.Count; i++)
                {
                    staff1.Add(new JProperty("order", "" + "has_order" + ""));
                    staff1.Add(new JProperty("Numbers", "" + res[i].Numbers + ""));
                    staff1.Add(new JProperty("ProcessState", "" + res[i].ProcessState + ""));
                    staff1.Add(new JProperty("Randomstr", "" + res[i].Randomstr + ""));
                    staff1.Add(new JProperty("UserReservationPos", "" + res[i].UserReservationPos + ""));
                    staff1.Add(new JProperty("UserReservationTime", "" + res[i].UserReservationTime + ""));
                    staff1.Add(new JProperty("timeconsuming", "" + res[i].timeconsuming + ""));
                    staff1.Add(new JProperty("moneyconsuming", "" + res[i].moneyconsuming + ""));

                    staff.Add(new JObject(staff1));
                    staff1.RemoveAll();
                }
            }

            return staff;
        }

        [HttpPost]
        [Route("Myaerial_del")]
        public bool Myaerial_del()
        {
            OpenIDPra.UserOpenID = "123";

            var flag = 1;
            IFormCollection req = Request.Form;
            String[] val = new String[20];
            System.Collections.Generic.ICollection<string> t3;

            t3 = req.Keys;
            int i = 0;
            StringValues te;
            foreach (string x in t3)
            {
                req.TryGetValue(x, out te);
                val[i] = te;
                i++;
            }
            if (i != 1)
                return false;

            int num = 0;
            var str = val[0];
            num = Regex.Matches(str, ",").Count;
            StringValues[] b = new StringValues[num];
            int row = 0;

            for (int j = 0; j < num; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    b[j] = b[j] + str[j * 5 + k];
                }

                var linq = (from obj in _context.Useraerials
                            where obj.Randomstr == b[j] && obj.UserOpenId == OpenIDPra.UserOpenID
                            select obj).First();
                //var linq = _context.Useraerials.Find(b[j]);

                if (object.Equals(linq, null))
                {
                    flag = 2;
                }
                else
                {
                    linq.ProcessState = "" + "cancelDone" + "";
                    row = _context.SaveChanges();
                    if (row <= 0)
                    {
                        flag = 2;
                    }
                }
            }
            if (flag == 1)
                return true;
            else
                return false;
        }
        

        [HttpPost]
        [Route("ScenicIntro")]
        public JArray ScenicIntro()
        {
            //JArray[] staff = new JArray[10];//暂时无法进行动态生成;
            JArray staff = new JArray();
            JObject staff1 = new JObject();

            String[] val = new String[20];
            val[0] = "001";
            val[1] = "002";
            val[2] = "003";
            val[3] = "004";
            val[4] = "005";

            int k = 0;
            foreach (string x in val)
            {
                //staff[k] = new JArray();

                var ling = from obj in _context.Scenics
                           where obj.ScenicName == val[k]
                           select new Scenic
                           {
                               ScenicName = obj.ScenicName,
                               ScenicId = obj.ScenicId,
                               ScenicFeeCoefficient = obj.ScenicFeeCoefficient,
                               ScenicFeeLevel = obj.ScenicFeeLevel,
                               ScenicIntroduction = obj.ScenicIntroduction
                           };
                var res = ling.ToList();
                if (res.Count == 0) continue;

                String[] temp = new String[2];
                for (int i = 0; i < res.Count; i++)
                {
                    //将景区景点分开且储存
                    temp = Regex.Split(res[i].ScenicId, " ");
                    staff1 = new JObject();
                    staff1.Add(new JProperty("ScenicArea", "" + temp[0] + ""));
                    staff1.Add(new JProperty("ScenicPos", "" + temp[1] + ""));
                    staff1.Add(new JProperty("ScenicFeeCoefficient", "" + res[i].ScenicFeeCoefficient + ""));
                    staff1.Add(new JProperty("ScenicFeeLevel", "" + res[i].ScenicFeeLevel + ""));
                    staff1.Add(new JProperty("ScenicIntroduction", "" + res[i].ScenicIntroduction + ""));

                    staff.Add(new JObject(staff1));
                    staff1.RemoveAll();
                }
                k++;
            }

            return staff;

        }*/

        /// <summary>
        /// config接口
        /// </summary>
        /// <returns></returns>
        /********config接口*******/
        [HttpPost]
        [Route("ConfigApi")]
        public JsSdkUiPackage ConfigApi()
        {
            IFormCollection req = Request.Form;
            System.Collections.Generic.ICollection<string> t3;
            t3 = req.Keys;
            StringValues te;
            foreach (string x in t3)
            {
                req.TryGetValue(x, out te);
            }
            var jssdkUiPackage = JSSDKHelper.GetJsSdkUiPackage(appId, secret, te);
            return jssdkUiPackage;
        }

    }
}
