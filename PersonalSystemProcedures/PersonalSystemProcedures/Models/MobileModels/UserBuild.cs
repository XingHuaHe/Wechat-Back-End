using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalSystemProcedures.Models.MobileModels
{
    public class UserBuild
    {
        /// <summary>
        /// 客户端发送的建立用户信息
        /// </summary>
        [Required]
        public string UserID { get; set; }      //用户账号
        [Required]
        public string UserPasswd { get; set; }      //密码
        [Required]
        public string UserName { get; set; }        //姓名
        [Required]
        public string UserCollege { get; set; }
        [Required]
        public string UserSex { get; set; }
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string UserPhoneNum { get; set; }
    }
}
