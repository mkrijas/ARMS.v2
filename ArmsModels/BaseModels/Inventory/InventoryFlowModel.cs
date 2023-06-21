using ArmsModels.SharedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace ArmsModels.BaseModels
{  
    public class InventoryflowModel : InventoryBaseModel
    {
        public InventoryflowModel(string _invtranNo)
        {
            InvTranNo = _invtranNo;           
        }
        public int? GrnID { get; set; } 
        public string InvTranNo { get; }          
    }

}
