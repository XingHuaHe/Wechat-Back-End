using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalSystemProcedures.Models.MobileModels
{
    /// <summary>
    /// 标记失物已经寻回
    /// </summary>
    public class ToStateLostInformation
    {
        public string UserID { get; set; }  //失物人
        public string LostInfoID { get; set; }// 失物ID
    }
}
