using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalSystemProcedures.Models.MobileModels
{
    /// <summary>
    /// 用户基本信息（接收、返回）
    /// </summary>
    public class UserInfo
    {
        [Required]
        public string UserID { get; set; }      //用户账号【唯一】
        [Required]
        public string UserName { get; set; }        //姓名
        [Required]
        public string UserCollege { get; set; }     //学院
        [Required]
        public string UserSex { get; set; }     //性别
        [Required]
        public string UserEmail { get; set; }       //邮箱
        [Required]
        public string UserIndiSignature { get; set; }        //个性签名
        [Required]
        public string UserHPortraitURL { get; set; }    //头像URL
        [Required]
        public string UserPhoneNum { get; set; }  //联系手机号
    }

    /// <summary>
    /// 头像
    /// </summary>
    public class UserHPortrait
    {
        [Required]
        public string UserID { get; set; }
        [Required]
        public string UserHPortraitURL { get; set; }
    }
}
