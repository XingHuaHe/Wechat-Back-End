using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalSystemProcedures.Models.MobileModels
{
    /// <summary>
    /// 接收用户登录
    /// </summary>
    public class UserLogin
    {
        [Required]
        public string UserID { get; set; }

        [Required]
        public string UserPassword { get; set; }
    }
}
