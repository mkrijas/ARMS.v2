using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;


namespace ArmsModels.BaseModels
{
    public class InterBranchAccountMappingModel
    {
        public InterBranchAccountMappingModel()
        {

            UserInfo = new SharedModels.UserInfoModel();
        }
        public int? ID { get; set; }
        public int? TransactionTypeID { get; set; }
        public int? BranchID { get; set; }
        public virtual string BranchName { get; set; }
        public string InterBranchArdCode { get; set; }
        public int? CoaID { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }
    public class InterBranchTransactionTypeModel
    {
        public int? ID { get; set; }
        public string TransactionTypeName { get; set; }
    }
}
