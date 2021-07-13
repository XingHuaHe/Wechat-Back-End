using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalSystemProcedures.Models.MobileModels.DbModel
{
    public class Admin
    {
        /// <summary>
        /// 管理员表
        /// </summary>
        [Key]
        [Required]
        public string AdminId { get; set; }         //身份证号

        [Required]
        public string AdminPasswd { get; set; }       //密码

        [Required]
        public string AdminName { get; set; }       //姓名
        //public string AdminGrade { get; set; }      //权限等级
        //public bool AdminOnline { get; set; }          //状态
        //public DateTime AdminLoginTime { get; set; }    //时间
        //public string AdminHPortraitURL { get; set; }       //头像
    }
}
