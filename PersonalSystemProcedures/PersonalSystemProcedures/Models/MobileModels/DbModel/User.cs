using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalSystemProcedures.Models.MobileModels.DbModel
{
    public class User
    {
        /// <summary>
        /// 用户表
        /// </summary>
        [Key]
        public string UserID { get; set; }      //用户账号
        public string UserPasswd { get; set; }      //密码
        public string UserName { get; set; }        //姓名
        public bool UserOnline { get; set; }        //状态
        public string UserCollege { get; set; }     //学院
        public string UserSex { get; set; }     //性别
        public string UserEmail { get; set; }       //邮箱
        public string UserIndiSignature { get; set; }        //个性签名
        public string UserHPortraitURL { get; set; }     //头像URL
        public string UserPhoneNum { get; set; }
    }
}
