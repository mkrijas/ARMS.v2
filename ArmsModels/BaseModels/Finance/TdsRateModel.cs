using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace ArmsModels.BaseModels
{
    public class TdsRateModel
    {
        public int? TdsRateID { get; set; }
        public NatureOfPaymentModel TdsNP { get; set; }        
        public AssesseeTypeModel AssesseeType { get; set; }       
        public DateTime? PeriodFrom { get; set; }
        public DateTime? PeriodTo { get; set; }
        public decimal? TaxRate { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }


    public class NatureOfPaymentModel
    {
        public int? TdsNPID { get; set; }        
        public string NatureOfPayment { get; set; }
    }
    public class AssesseeTypeModel
    {
        public int? AssesseeTypeID { get; set; }
        public string AssesseeTypeName { get; set; }
    }
}

