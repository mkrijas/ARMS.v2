using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    // Model representing a TDS (Tax Deducted at Source) rate
    public class TdsRateModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<TdsRateModel>(Json);
        }
        public TdsRateModel()
        {
            this.UserInfo = new SharedModels.UserInfoModel();
            this.AssesseeType = new();
            this.TdsNP = new();
        }
        public int? TdsRateID { get; set; } // Unique identifier for the TDS rate
        [ValidateComplexType]
        public NatureOfPaymentModel TdsNP { get; set; } 
        [ValidateComplexType]
        [Required]
        public AssesseeTypeModel AssesseeType { get; set; } // Assessee type associated with the TDS rate
        [Required]
        public DateTime? PeriodFrom { get; set; }
        [Required]
        public DateTime? PeriodTo { get; set; }
        [Required]
        public decimal? TaxRate { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

    // Model representing the nature of payment for TD
    public class NatureOfPaymentModel
    {
        public int? TdsNPID { get; set; } // Model representing the nature of payment for TD
        [Required]
        public string NatureOfPayment { get; set; }
        public string SectionCode { get; set; }
    }

    // Model representing the assessee type for TDS 
    public class AssesseeTypeModel
    {
        [Required]
        public int? AssesseeTypeID { get; set; } // Unique identifier for the assessee type
        public string AssesseeTypeName { get; set; }
    }

    // Model representing TDS account mapping
    public class TdsAccountMappingModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<TdsAccountMappingModel>(Json);
        }
        public TdsAccountMappingModel()
        {
            this.UserInfo = new SharedModels.UserInfoModel();
        }
        public int? TdsAccountMappedID { get; set; } // Unique identifier for the TDS account mapping
        [Required]
        public int? CoaID { get; set; }
        public virtual string AccountName { get; set; }
        [Required]
        public int? TdsNPID { get; set; }
        public virtual string NatureOfPayment { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

    // Model representing TDS threshold limits
    public class TdsThresholdLimitModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<TdsThresholdLimitModel>(Json);
        }
        public TdsThresholdLimitModel()
        {
            this.UserInfo = new SharedModels.UserInfoModel();
        }
        public int? TdsTLID { get; set; } // Unique identifier for the TDS threshold limit
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