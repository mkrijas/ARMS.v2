using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    public class DriverSalaryPayableModel : TransactionBaseModel, IValidatableObject, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<DriverSalaryPayableModel>(Json);
        }
        public DriverSalaryPayableModel()
        {
            NatureOfTransaction = "DriverSalaryPayable";
        }
        public int? ID { get; set; }
        [Required]
        public DateTime? FromDate { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        [Required]
        public DateTime? ToDate { get; set; } = DateTime.Today;
        public List<DriverSalaryPayableListModel> DriversLists { get; set; } = new();
    }

    public class DriverSalaryPayableListModel
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<DriverSalaryPayableListModel>(Json);
        }
        public int? ID { get; set; }
        public int? HeaderID { get; set; }
        [Required]
        public int? DriverID { get; set; }
        [Required]
        public decimal? WorkDays { get; set; }
        [Required]
        public int? NoOfTrips { get; set; }
        [Required]
        public int? RunKM { get; set; }
        [Required]
        public decimal? GrossSalary { get; set; }
        [Required]
        public decimal? Deduction { get; set; }
        [Required]
        public decimal? NetSalary { get; set; }
        public string Reference { get; set; }
        public virtual string DriverCode { get; set; }
        public virtual string DriverName { get; set; }
    }

    public class DriverPendingSalaryModel
    {
        public DriverModel Driver { get; set; }
        public string EntryRef { get; set; }
        public decimal? Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DocumentDate { get; set; }
        public string ArdCode { get; set; }
        public virtual bool IsChecked { get; set; }
        public virtual string DocNum { get; set; }
        public virtual string BranchName { get; set; }

    }

}