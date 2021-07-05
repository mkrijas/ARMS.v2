using ArmsModels.SharedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class TariffModel
    {
        public TariffModel()
        {            
            UserInfo = new UserInfoModel();
        }

        public int TariffID { get; set; }
        [Required][StringLength(maximumLength:200)]
        public string TariffName { get; set; }
        [Required]
        public int OrderID { get; set; }
        [Required]
        public int RouteID { get; set; }
        [Required]
        public short TariffTypeID { get; set; }
        [Required]
        public short TariffFormulaID { get; set; }
        [Required]
        public decimal TariffRate { get; set; }
        public byte TruckAxles { get; set; }
        public string OrderName { get; set; }
        public string RouteName { get; set; }
        public string Formula { get; set; }
        public string TariffTypeName { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }

    public class TariffFormulaModel
    {
        public TariffFormulaModel()
        {
            UserInfo = new UserInfoModel();
        }

        public short FormulaID { get; set; }
        [Required]
        public string Formula { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }

    public class TariffTypeModel
    {
        public TariffTypeModel()
        {
            UserInfo = new UserInfoModel();
        }
        public short TariffTypeID { get; set; }
        [Required]
        public string TariffTypeName { get; set; }
        public bool IsExpense { get; set; } = false;
        public bool IsIncome { get; set; } = true;
        public UserInfoModel UserInfo { get; set; }
    }
}
