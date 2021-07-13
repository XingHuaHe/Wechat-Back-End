using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalSystemProcedures.Models.WechatModels
{
    public class Admin
    {
        [Key]
        public string name { get; set; }
        public string AdminPasswd { get; set; }
        public string AdminNumber { get; set; }
    }
}
