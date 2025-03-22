using ArmsModels.BaseModels;
using ArmsServices;
using ArmsServices.DataServices;
using ArmsServices.DataServices.Finance.Transactions;
using Core.BaseModels.Finance.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MobileAPI.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class FinanceTransactionController : ControllerBase
    {
        private readonly IPaymentInitiatedService _paymentInitiatedService;
        private readonly IPaymentService _paymentService;
        private readonly IMileageShortageReceiptService _mileageShortageReceiptService;
        private readonly IInventoryReleaseService _inventoryReleaseService;
        private readonly IStoreService _storeService;

        public FinanceTransactionController(IPaymentInitiatedService paymentInitiatedService,
                                            IPaymentService paymentService,
                                            IMileageShortageReceiptService mileageShortageReceiptService,
                                            IInventoryReleaseService inventoryReleaseService,
                                            IStoreService storeService)
        {
            _paymentInitiatedService = paymentInitiatedService;
            _paymentService = paymentService;
            _mileageShortageReceiptService = mileageShortageReceiptService;
            _inventoryReleaseService = inventoryReleaseService;
            _storeService = storeService;
        }
        
        //Payment Initiated List Select
        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<PaymentInitiatedModel> SelectPaymentInitiated(int? BranchID, string searchterm = "")
        {
            IEnumerable<PaymentInitiatedModel> PaymentInitiatedList;
            PaymentInitiatedList = _paymentInitiatedService.PendingForCompletion( BranchID, searchterm).ToList();
            return PaymentInitiatedList;
        }

        //Payment Initiated Select
        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<PaymentMemoModel> SelectInitiated(int? PaymentInitiatedID)
        {
            return _paymentService.SelectInitiated(PaymentInitiatedID);
        }

        //Payment Memo List Select
        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<PaymentMemoModel> SelectPaymentMemoByUnapproved(int? BranchID, int? numberOfRecords, bool IsInterBranch, string searchterm = "")
        {
            IEnumerable<PaymentMemoModel> PaymentMemoList;
            PaymentMemoList = _paymentService.SelectByUnapproved(BranchID, numberOfRecords, IsInterBranch, searchterm).ToList();
            return PaymentMemoList;
        }

        //Payment Memo Select
        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public PaymentMemoModel SelectPaymentMemo(int? ID)
        {
            return _paymentService.SelectByID(ID);
        }

        //Mileage shortage List Select
        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<MileageShortageReceiptModel> SelectMileageShortageByUnapproved(int? BranchID, int? numberOfRecords, string searchterm = "")
        {
            IEnumerable<MileageShortageReceiptModel> MIleageShortageList;
            MIleageShortageList = _mileageShortageReceiptService.SelectByUnapproved(BranchID, numberOfRecords, searchterm).ToList();
            return MIleageShortageList;
        }

        //Mileage shortage Select
        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public MileageShortageReceiptModel SelectMileageShortage(int? ID)
        {
            return _mileageShortageReceiptService.SelectByID(ID);
        }

        //Inventory Release List Select
        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<InventoryReleaseModel> SelectInventoryReleaseByUnApproved(int? StoreID, int? BranchID)
        {
            IEnumerable<InventoryReleaseModel> InventoryReleaseList;
            InventoryReleaseList = _inventoryReleaseService.SelectByStoreAndBranchIDUnApproved(StoreID, BranchID).ToList();
            return InventoryReleaseList;
        }

        //Inventory Release Select
        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public InventoryReleaseModel SelectInventoryRelease(int? ID)
        {
            return _inventoryReleaseService.SelectByID(ID);
        }

        //Inventory Release Sub List Select
        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<InventoryReleaseSubViewModel> SelectInventoryReleaseSub(int? ID, int? StoreID)
        {
            IEnumerable<InventoryReleaseSubViewModel> InventoryReleaseSubList;
            InventoryReleaseSubList = _inventoryReleaseService.GetRequstSubReadOnly(ID, StoreID).ToList();
            return InventoryReleaseSubList;
        }

        //Store List Select
        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<StoreModel> SelectStoreByBranch(int BranchID)
        {
            IEnumerable<StoreModel> StoreList;
            StoreList = _storeService.SelectByBranch(BranchID).ToList();
            return StoreList;
        }
    }
}