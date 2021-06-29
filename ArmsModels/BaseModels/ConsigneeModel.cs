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
        }

        public int ConsigneeID { get; set; }
        [Required]
        public string ConsigneeName { get; set; }
        public string Mobile { get; set; }
        [Required]
        public int PlaceID { get; set; }
        [Required]
        public int OrderID { get; set; }
        public bool Consignor { get; set; }
        public int AddressID { get; set; }
        public OrderModel Order { get; set; }
        [ValidateComplexType]
        public AddressModel Address { get; set; }
        public PlaceModel Place { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }
}
