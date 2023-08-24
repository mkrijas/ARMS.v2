using ArmsModels.BaseModels;

namespace Core.BaseModels.Inventory
{
    public class InventoryQtyManageModel
    {
        public int? QtyManageID { get; set; }
        public StoreModel Store { get; set; }
        public InventoryItemModel Item { get; set; }
        public decimal? MinQty { get; set; }
        public decimal? ReOrderQty { get; set; }
        public decimal? InhandQty { get; set; }

    }
}
