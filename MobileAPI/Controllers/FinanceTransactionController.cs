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
    public class FinanceTransactionController : ControllerBase
    {
        private readonly IPaymentInitiatedService _paymentInitiatedService;
        private readonly IPaymentService _paymentService;
        private readonly IMileageShortageReceiptService _mileageShortageReceiptService;
        private readonly IInventoryReleaseService _inventoryReleaseService;
        private readonly IStoreService _storeService;
        private readonly IDataAuthorizationService _dataAuthorizationService;
        private readonly IUserService _userService;
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly IInventoryGrnService _inventoryGrnService;
        private readonly IMobileNotificationService _mobileNotificationService;

        public FinanceTransactionController(IPaymentInitiatedService paymentInitiatedService,
                                            IPaymentService paymentService,
                                            IMileageShortageReceiptService mileageShortageReceiptService,
                                            IInventoryReleaseService inventoryReleaseService,
                                            IStoreService storeService,
                                            IDataAuthorizationService dataAuthorizationService,
                                            IUserService userService,
                                            IPurchaseOrderService purchaseOrderService,
                                            IInventoryGrnService inventoryGrnService,
                                            IMobileNotificationService mobileNotificationService)
        {
            _paymentInitiatedService = paymentInitiatedService;
            _paymentService = paymentService;
            _mileageShortageReceiptService = mileageShortageReceiptService;
            _inventoryReleaseService = inventoryReleaseService;
            _storeService = storeService;
            _dataAuthorizationService = dataAuthorizationService;
            _userService = userService;
            _purchaseOrderService = purchaseOrderService;
            _inventoryGrnService = inventoryGrnService;
            _mobileNotificationService = mobileNotificationService;
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

        //Get Payment Memo Bills
        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<BillsPaidModel> SelectPaymentMemoBills(int? ID)
        {
            IEnumerable<BillsPaidModel> PaymentMemoBills;
            PaymentMemoBills = _paymentService.GetBills(ID).ToList();
            return PaymentMemoBills;
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

        // Define a private readonly dictionary for DocType to DocTypeID mapping
        private readonly Dictionary<string, int> _docTypeIDDictionary = new Dictionary<string, int>
        {
            { "INITIATE_PAYMENT", 15 },
            { "PAYMENT_MEMO", 7 },
            { "MILEAGE_SHORTAGE_RECEIPT", 30 },
            { "INVENTORY_RELEASE", 97 },
            { "INVENTORY_GRN", 16 },
            { "PURCHASE_ORDER", 17 }
        };

        //Data auth status list
        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<DataAuthorizationModel> GetAuthStatus(string DocType, int? DocumentID)
        {
            int DocTypeID;

            if (!_docTypeIDDictionary.TryGetValue(DocType, out DocTypeID))
            {
                Console.WriteLine("Invalid DocType provided.");
                return Enumerable.Empty<DataAuthorizationModel>();
            }

            IEnumerable<DataAuthorizationModel> AuthStatusList;
            AuthStatusList = _dataAuthorizationService.GetAuthStatus(DocTypeID, DocumentID).OrderBy(x => x.AuthLevelID).ToList();
            return AuthStatusList;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Approve([FromBody] DataAuthorizationModel model, int? BranchID, string DocType)
        {
            int DocTypeID;
            bool HasPermissionApprove = false;

            // Check if DocType is valid
            if (!_docTypeIDDictionary.TryGetValue(DocType, out DocTypeID))
            {
                return BadRequest(new { Status = "Invalid DocType provided." });
            }

            // If AuthLevelID is not 99, update the model
            if (model.AuthLevelID != 99)
            {
                model = _dataAuthorizationService.Update(model);
                return Ok(new
                {
                    Status = "Verified",
                    UpdatedModel = model
                });
            }
            else
            {
                //Check permission before approving
                CancellationTokenSource ctc = new CancellationTokenSource();
                HasPermissionApprove = _userService.GetClaimsAsync(model.UserInfo.UserID, DocTypeID.ToString(), "Approve", BranchID, ctc.Token);

                // Deny access if user does not have permission
                if (!HasPermissionApprove)
                {
                    return Forbid(); // 403 Forbidden
                }

                switch (DocTypeID)
                {
                    case 15: // INITIATE_PAYMENT
                        _paymentInitiatedService.Approve(model.DocumentID, model.UserInfo.UserID, model.Remarks);
                        break;
                    case 7: // PAYMENT_MEMO
                        _paymentService.Approve(model.DocumentID, model.UserInfo.UserID, model.Remarks);
                        break;
                    case 30: // MILEAGE_SHORTAGE_RECEIPT
                        _mileageShortageReceiptService.Approve(model.DocumentID, model.UserInfo.UserID, model.Remarks);
                        break;
                    case 97: // INVENTORY_RELEASE
                        _inventoryReleaseService.Approve(model.DocumentID, model.UserInfo.UserID, model.Remarks);
                        break;
                    case 16: // INVENTORY_GRN
                        _inventoryGrnService.Approve(model.DocumentID.Value, model.UserInfo.UserID, model.Remarks);
                        break;
                    case 17: // PURCHASE_ORDER
                        _purchaseOrderService.Approve(model.DocumentID.Value, model.UserInfo.UserID, model.Remarks);
                        break;
                    default:
                        return BadRequest(new { Status = "Invalid DocTypeID" });
                }

                return Ok(new
                {
                    Status = "Approved",
                    ApprovedModel = model
                });
            }
        }

        [HttpPost("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]        
        public async Task<IActionResult> UpdateMonthlyIncentive(MonthlyIncentiveModel updateModel)
        {            
            if (updateModel.UserInfo.UserID == "HASEEBRAHMAN" || updateModel.UserInfo.UserID == "SOUMYA")
            {
                var returnModel = _mobileNotificationService.UpdateMonthlyIncentive(updateModel);
                if (returnModel != null)
                {
                    return Ok("Saved Successfully.");
                }
            }
            return BadRequest("Permission denied! You don't have any permission to Edit Route.");                   
        }

    }
}