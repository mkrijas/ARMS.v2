using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class ConsigneeModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ConsigneeModel>(Json);
        }
        public ConsigneeModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
            Address = new AddressModel();
        }
        string _name;
        public int? ConsigneeID { get; set; }
        [Required]
        public string ConsigneeName { get { return _name; } set { _name = value; this.Address.AddresseeName = value; } }
        public virtual string ArdCode { get; set; }
        public string Mobile { get; set; }
        [Required]
        public int? PlaceID { get; set; }
        [Required]
        public int? OrderID { get; set; }
        public bool Consignor { get; set; } = false;
        public int? AddressID { get; set; }
        public string OrderName { get; set; }
        [ValidateComplexType]
        public AddressModel Address { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

    public class StockistFreightReceivableModel
    {
        public ConsigneeModel Consignee { get; set; }
        public string EntryRef { get; set; }
        public decimal? Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DocumentDate { get; set; }
        public string InvoiceNo { get; set; }
    }
}