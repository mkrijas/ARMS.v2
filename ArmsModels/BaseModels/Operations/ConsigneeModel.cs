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

        public int? ConsigneeID { get; set; }
        [Required]
        public string ConsigneeName { get; set; }
        public virtual string ArdCode { get; set; }
        public string Mobile { get; set; }       
        public int? PlaceID { get; set; }        
        public int? OrderID { get; set; }
        public bool Consignor { get; set; }
        public int? AddressID { get; set; }
        public string OrderName { get; set; }
        [ValidateComplexType]
        public AddressModel Address { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }
}
