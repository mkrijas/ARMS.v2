using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Core.BaseModels.Finance;

namespace ArmsModels.BaseModels
{
    // Model representing an opening balance
    public class OpeningBalanceModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<OpeningBalanceModel>(Json);
        }
        public int? OpeniongBalanceID { get; set; } // Unique identifier for the opening balance  
        public int? PeriodID { get; set; }          
        public string PeriodDescription { get; set; }       
        public int? BranchID { get; set; }        
        public string BranchName { get; set; }        
        public int? CoaID { get; set; }
        public string AccountName { get; set; }
        public decimal? Amount { get; set; } 
        public string ArdCode { get; set; } 
        public string SubArdCode { get; set; }
        public bool hideTextField = false;
        public SharedModels.UserInfoModel UserInfo { get; set; }

    }

    // Model representing a period
    public class PeriodModel
    {
        public int? PeriodID { get; set; } // Unique identifier for the period
        public string PeriodDescription { get; set; }

    }
    }
