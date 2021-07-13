using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalSystemProcedures.Models.MobileModels
{
    /// <summary>
    /// 获取个人失物信息（单个）
    /// </summary>
    public class ToGetLostInfo
    {
        [Required]
        public string UserID { get; set; }
        [Required]
        public string LostInfoID { get; set; }// 失物ID
    }

    public class ToGetAllInfos
    {
        [Required]
        public string UserID { get; set; }
    }
}
