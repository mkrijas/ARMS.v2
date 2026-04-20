using ArmsModels.BaseModels;
using ArmsServices.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MobileAPI.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly IInventoryGrnService _inventoryGrnService;
        private readonly IInventoryItemService _InventoryItemService;

        public InventoryController(IPurchaseOrderService purchaseOrderService,
                                   IInventoryGrnService inventoryGrnService,
                                   IInventoryItemService inventoryItemService)
        {
            _purchaseOrderService = purchaseOrderService;
            _inventoryGrnService = inventoryGrnService;
            _InventoryItemService = inventoryItemService;
        }       

        //Pending PO Select
        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<PurchaseOrderModel> SelectPendingPurchaseOrder(int BranchID)
        {
            IEnumerable<PurchaseOrderModel> PurchaseOrderCollection;
            PurchaseOrderCollection = _purchaseOrderService.SelectPending(BranchID).Where(x => x.AuthLevelID != 99 && x.AuthLevelID != 100).ToList();
            return PurchaseOrderCollection;
        }

        //Pending PO Select
        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<PurchaseOrderModel> SelectApprovedPurchaseOrder(int BranchID)
        {
            IEnumerable<PurchaseOrderModel> PurchaseOrderCollection;
            PurchaseOrderCollection = _purchaseOrderService.SelectPending(BranchID).Where(x => x.AuthLevelID == 99).ToList();
            return PurchaseOrderCollection;
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<InventoryItemEntryModel> SelectPurchaseOrderEntries(int POID)
        {
            IEnumerable<InventoryItemEntryModel> PurchaseOrderEntries;
            PurchaseOrderEntries = _purchaseOrderService.GetItemEntriesPO(POID).ToList();
            return PurchaseOrderEntries;
        }

        //Pending GRN Select
        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<InventoryGrnModel> SelectPendingGrn(int BranchID)
        {
            IEnumerable<InventoryGrnModel> GrnCollection;
            GrnCollection = _inventoryGrnService.SelectPending(BranchID).Where(x => (x.AuthLevelID != 99 && x.AuthLevelID != 100) && x.UsedInventory == 0).ToList();
            return GrnCollection;
        }

        //Pending GRN Select
        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<InventoryGrnModel> SelectApprovedGrn(int BranchID)
        {
            IEnumerable<InventoryGrnModel> GrnCollection;
            GrnCollection = _inventoryGrnService.SelectPending(BranchID).Where(x => (x.AuthLevelID == 99 || x.AuthLevelID == 100) && x.UsedInventory == 0).ToList();
            return GrnCollection;
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<InventoryItemEntryModel> SelectGrnEntries(int GrnID)
        {
            IEnumerable<InventoryItemEntryModel> GrnEntries;
            GrnEntries = _inventoryGrnService.GetItemEntries(GrnID).ToList();
            return GrnEntries;
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<InventoryItemStockModel> GetCurrentStock(int? StoreID, int? ItemID)
        {
            IEnumerable<InventoryItemStockModel> InventoryItemStockCollection;
            InventoryItemStockCollection = _InventoryItemService.GetCurrentStock(StoreID, ItemID).ToList();
            return InventoryItemStockCollection;
        }
    }
}