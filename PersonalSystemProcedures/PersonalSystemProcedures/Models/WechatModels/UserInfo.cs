using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalSystemProcedures.Models.WechatModels
{
    public class UserInfo
    {
        [Key]
        public string openid { get; set; }//用户的标识，对当前公众号唯一
        public string subscribe { get; set; }//用户是否订阅该公众号标识，值为0时，代表此用户没有关注该公众号，拉取不到其余信息
        public string nickname { get; set; }//用户的昵称
        public string sex { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string province { get; set; }
        public string language { get; set; }//用户的语言，简体中文为zh_CN
        public string headimgurl { get; set; }
        public string subscribe_time { get; set; }//用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间
        public string unionid { get; set; }//只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段
        public string remark { get; set; }//公众号运营者对粉丝的备注，公众号运营者可在微信公众平台用户管理界面对粉丝添加备注
        public string groupid { get; set; }//用户所在的分组ID（兼容旧的用户分组接口）
        public string tagid_list { get; set; }//用户被打上的标签ID列表
        public string subscribe_scene { get; set; }//返回用户关注的渠道来源
        public string qr_scene { get; set; }//二维码扫码场景（开发者自定义）
        public string qr_scene_str { get; set; }//二维码扫码场景描述（开发者自定义）
    }
}
