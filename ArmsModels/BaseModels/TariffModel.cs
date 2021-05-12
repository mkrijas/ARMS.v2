using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class TariffModel
    {
        public int TariffID { get; set; }
        [Required][StringLength(maximumLength:200)]
        public string TariffName { get; set; }
        [Required]
        public int OrderID { get; set; }
        [Required]
        public short TariffTypeID { get; set; }
        [Required]
        public decimal TariffRate { get; set; }
        public byte TruckAxles { get; set; }
        [Required]
        public short TariffFormulaID { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }
}
