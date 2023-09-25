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
        [Required]
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
        public int? TdsNPID { get; set; }
        [Required]
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
        [Required]
        public int? CoaID { get; set; }
        public virtual string AccountName { get; set; }
        [Required]
        public int? TdsNPID { get; set; }
        public virtual string NatureOfPayment { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }

    }


    public class TdsThresholdLimitModel
    {
        public TdsThresholdLimitModel()
        {
            this.UserInfo = new SharedModels.UserInfoModel();
        }
        public int? TdsTLID { get; set; }
        [ValidateComplexType]
        [Required]
        public NatureOfPaymentModel NatureOfPayment { get; set; }

        [Required]
        public DateTime? PeriodFrom { get; set; }

        [Required]
        public DateTime? PeriodTo { get; set; }

        [Required]
        public decimal? ThresholdLimitSingle { get; set; }

        [Required]
        public decimal? ThresholdLimitPeriod { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }


}

