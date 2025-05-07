using ArmsModels.BaseModels;
using ArmsServices;
using ArmsServices.DataServices;
using ArmsServices.DataServices.Finance.Transactions;
using Core.BaseModels.Finance.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileAPI.Services;
using System;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MobileAPI.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly IInventoryGrnService _inventoryGrnService;

        public InventoryController(IPurchaseOrderService purchaseOrderService,
                                   IInventoryGrnService inventoryGrnService)
        {
            _purchaseOrderService = purchaseOrderService;
            _inventoryGrnService = inventoryGrnService;
        }       

        //Pending PO Select
        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<PurchaseOrderModel> SelectPendingPurchaseOrder(int BranchID)
        {
            IEnumerable<PurchaseOrderModel> PurchaseOrderCollection;
            PurchaseOrderCollection = _purchaseOrderService.SelectPending(BranchID).Where(x => x.AuthLevelID != 99).ToList();
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
            GrnCollection = _inventoryGrnService.SelectPending(BranchID).Where(x => (x.AuthLevelID == 99 && x.AuthLevelID == 100) && x.UsedInventory == 0).ToList();
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
    }
}