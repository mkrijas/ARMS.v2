using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class ConsigneeModel
    {
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
}
