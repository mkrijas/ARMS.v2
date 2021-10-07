using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ArmsModels.SharedModels;

namespace ArmsModels.BaseModels
{
    public class AlertModel
    {
        public int? AlertID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int? Severity { get; set; }
        public string OriginRef { get; set; }
        public UserInfoModel UserInfo { get; set; }
        
    }
}



