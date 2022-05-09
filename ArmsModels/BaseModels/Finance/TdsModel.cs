using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace ArmsModels.BaseModels
{
    public class TdsRateModel
    {
        public TdsRateModel()
        {
         
            this.UserInfo = new SharedModels.UserInfoModel();
            this.AssesseeType = new();
            this.TdsNP = new();
        }
        public int? TdsRateID { get; set; }
        [ValidateComplexType]
        public NatureOfPaymentModel TdsNP { get; set; }  
        [ValidateComplexType]
        public AssesseeTypeModel AssesseeType { get; set; }
        [Required]
        public DateTime? PeriodFrom { get; set; }
        [Required]
        public DateTime? PeriodTo { get; set; }
        [Required]
        public decimal? TaxRate { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }


    public class NatureOfPaymentModel
    {
        [Required]
        public int? TdsNPID { get; set; }        
        public string NatureOfPayment { get; set; }
    }
    public class AssesseeTypeModel
    {
        [Required]
        public int? AssesseeTypeID { get; set; }
        public string AssesseeTypeName { get; set; }
    }


    public class TdsAccountMappingModel
    {
        public TdsAccountMappingModel()
        {

            this.UserInfo = new SharedModels.UserInfoModel();
          
        }
        public int? TdsAccountMappedID { get; set; }
        
        public int? CoaID { get; set; }
        public virtual string AccountName { get; set; }
        [Required]
        public int? TdsNPID { get; set; }
        public virtual string NatureOfPayment { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }

    }
}

