using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    public class InterBranchMappingModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<InterBranchMappingModel>(Json);
        }
        public InterBranchMappingModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }
        public int? ID { get; set; }
        [Required]
        public int? TransactionTypeID { get; set; }
        public virtual string TransactionTypeName { get; set; }
        [Required]
        public int? BranchID { get; set; }
        public virtual string BranchName { get; set; }
        public string InterBranchArdCode { get; set; }
        [Required]
        public int? CoaID { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

    public class InterBranchTransactionTypeModel
    {
        public int? ID { get; set; }
        public string TransactionTypeName { get; set; }
    }
}