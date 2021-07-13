using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalSystemProcedures.Models.MobileModels
{
    /// <summary>
    /// 获取失物信息
    /// </summary>
    public class ToLostInfo
    {
        [Required]
        public string UserID { get; set; }  //失物人
        [Required]
        public string UserName { get; set; }
        [Required]
        public string LostName { get; set; }    //失物名
        [Required]
        public string PhoneNumber { get; set; } //失物人电话
        [Required]
        public string LostInfos { get; set; } //失物信息
        [Required]
        public string LostInfoPic { get; set; } //失物照片
    }
}
