using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalSystemProcedures.Models.WechatModels
{
    public class UserInfo_OAuth2
    {
        [Key]
        public string openid { get; set; }//用户的唯一标识
        public string nickname { get; set; }//用户昵称
        public string sex { get; set; }//用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        public string province { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string headimgurl { get; set; }
        public string privilege { get; set; }//用户特权信息，json 数组，如微信沃卡用户为（chinaunicom）
        public string unionid { get; set; }//只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段。
    }
}
