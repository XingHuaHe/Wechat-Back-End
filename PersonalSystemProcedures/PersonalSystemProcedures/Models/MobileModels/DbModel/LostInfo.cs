using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalSystemProcedures.Models.MobileModels.DbModel
{
    /// <summary>
    /// 失物信息
    /// </summary>
    public class LostInfo
    {
        [Key]
        public string LostInfoID { get; set; } //失物唯有ID
        public string UserID { get; set; }  //失物人
        public string UserName { get; set; } //失物人名
        public string LostName { get; set; }    //失物名
        public string PhoneNumber { get; set; } //失物人电话
        public string LostInfos { get; set; } //失物信息
        public string LostInfoPic { get; set; } //失物照片
        public DateTime LostTime { get; set; }  //丢失时间
        public bool States { get; set; }    //false为未寻回，ture为已寻回
    }
}
