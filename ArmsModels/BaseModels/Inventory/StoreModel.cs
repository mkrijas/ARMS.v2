using ArmsModels.SharedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace ArmsModels.BaseModels
{
    public class StoreModel
    {
        public StoreModel()
        {
            UserInfo = new();
        }

        public int? StorID { get; set; }
        [Required]
        [StringLength(100)]
        public string StoreName { get; set; }
        [Required]
        public int? BranchID { get; set; }
        public virtual string BranchName { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }


    public class InventoryBatchModel
    {
        public int BatchID { get; set; }
        public int StoreID { get; set; }
        public int ItemID { get; set; }
        public decimal ItemRate { get; set; }

    }
}
