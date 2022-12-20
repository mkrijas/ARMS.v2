using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
namespace ArmsModels.BaseModels
{
    public class OutstandingBillsModel : TransactionBaseModel
    {
        public int? BoID { get; set; }
        public PartyModel PartyInfo { get; set; }
        public int  ? CoaID { get; set; }
        public decimal? InitialAmount { get; set; }
        public virtual string BranchName { get; set; }
        public virtual int? BranchID { get; set; }
       
        public string ReferenceDocNo { get; set; }
        public DateTime? ReferenceDocDate { get; set; }
    }
}
