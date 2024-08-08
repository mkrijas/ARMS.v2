using ArmsModels.BaseModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Views.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using ArmsServices.DataServices;
using ArmsServices;
using Microsoft.AspNetCore.Authorization;
using System.Security.Permissions;
using Microsoft.JSInterop;
using System.Collections.Generic;
using Core.IDataServices.Finance.DayOpen;

namespace Views.Data
{
    [Authorize]
    public abstract class DialogFormComponentBase<T> : ComponentBase where T : TransactionBaseModel
    {
        [Inject] protected AuthenticationStateProvider auth { get; set; }
        [Inject] protected ISnackbar snackbar { get; set; }
        [Inject] protected IDialogService dialogService { get; set; }
        [Inject] protected IRoleService<RoleModel> Irole { get; set; }
        [Inject] protected IOutstandingBillsService OBIservice { get; set; }
        [Inject] protected IPartyService partyService { get; set; }
        [Inject] protected IInterBranchMappingService interbranchService { get; set; }
        [Inject] protected IGstUsageIDService usageCodeService { get; set; }
        [Inject] protected IPushNotificationService notiService { get; set; }
        [Inject] protected IJSRuntime JsRuntime { get; set; }
        [Inject] protected IBranchService branchService { get; set; }
        [Inject] protected IbaseInterface<T> baseInterface { get; set; }
        [Inject] protected IDayOpenService DayOpenService { get; set; }

        [Parameter]
        public bool ReadOnly { get; set; }
        [Parameter]
        public virtual T model { get; set; }
        [Parameter]
        public EventCallback OnApproved { get; set; }

        protected bool ViewPermission { get; set; }
        protected bool EditPermission { get; set; }
        protected bool DeletePermission { get; set; }
        protected bool ApprovePermission { get; set; }

        protected bool Local;
        protected bool IsTaxable;
        protected BranchModel OtherBranch = new();

        protected EditContext editContext;
        protected DialogForm dialogForm;
        protected int? BranchID = null;
        protected string UserID = null;

        protected string DocNumber { get; set; }
        protected DateTime? DocDate { get; set; }
        protected List<InterBranchTransactionTypeModel> InterBranchTranTypes = new();
        protected bool _busy;

        protected abstract DocumentInfoModel DocInfo { get; set; }


        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await setPermissions();

            var authprov = await auth.GetAuthenticationStateAsync();
            BranchID = int.Parse(authprov.User.Claims.First(x => x.Type == "BranchID").Value);
            var user = authprov.User;
            UserID = user.Identity.Name;
            model.BranchID = BranchID;

            model.OtherBranchID = model.IsInterBranch ? model.OtherBranchID : BranchID;
            if (model.OtherBranchID != null)
            {
                OtherBranch = branchService.SelectByID(model.OtherBranchID);
            }
            InterBranchTranTypes = interbranchService.GetTypes().ToList();
        }
        protected override void OnParametersSet()
        {
            editContext = new EditContext(model);
        }
        async Task setPermissions()
        {
            CancellationTokenSource ctc = new CancellationTokenSource();
            ViewPermission = await Irole.HasClaim(DocInfo.DocumentTypeID.ToString(), "View", ctc.Token);
            EditPermission = await Irole.HasClaim(DocInfo.DocumentTypeID.ToString(), "Edit", ctc.Token);
            DeletePermission = await Irole.HasClaim(DocInfo.DocumentTypeID.ToString(), "Delete", ctc.Token);
            ApprovePermission = await Irole.HasClaim(DocInfo.DocumentTypeID.ToString(), "Approve", ctc.Token);
        }

        protected void OtherBranchSelected(BranchModel branch)
        {
            OtherBranch = branch;
            model.OtherBranchID = branch?.BranchID;
        }

        protected abstract Task<int> Update(T editModel);
        protected abstract int UpdateApproval(DataApprovedStatus aprvd);

        public async Task<bool> validate(T model)
        {
            if (!DayOpenService.ValidateDayOpen(model.DocumentDate, DocInfo.DocumentTypeID, model.BranchID).Value)
            {
                bool? result = await dialogService.ShowMessageBox(
                    "Oops !",
                    new MarkupString("The specified DocumentDate : <b>" + model.DocumentDate.Value.ToString("dd/MM/yyyy") + "</b> is closed for this branch ! <br>Please create a request to open the date."));
                return false;
            }
            if (model.TotalAmount <= 0)
            {
                snackbar.Add("Total amount should be greater than zero!", Severity.Error);
                return false;
            }
            return true;
        }

        protected async Task OnValidSubmit(EditContext context)
        {
            if (_busy)
            {
                snackbar.Add("Please wait while form is being submitted!", Severity.Warning);
                return;
            }
            _busy = true;
            if (await validate(model))
            {
                if (!EditPermission)
                {
                    bool? result = await dialogService.ShowMessageBox("Permission denied!", "You dont have permission to Edit !.");
                }
                else
                {
                    try
                    {
                        model.UserInfo.UserID = UserID;
                        int ID = await Update(model);
                        if (ID != 0)
                        {
                            notiService.CreateAuthNotifications(BranchID.Value, DocInfo.DocumentTypeID.Value, ID);
                            snackbar.Add("Submitted Successfully", Severity.Success);
                            dialogForm.Close(model);
                        }
                    }
                    catch (Exception ex)
                    {
                        snackbar.Add(ex.Message, Severity.Error);
                    }

                }
            }
            await Task.Delay(200);
            _busy = false;
        }

        protected async Task Approve(DataApprovedStatus aprvd)
        {
            if (ApprovePermission)
            {
                if (aprvd.IsApprove)
                {
                    try
                    {
                        model.UserInfo.UserID = UserID;
                        UpdateApproval(aprvd);
                        notiService.AcknowledgeAuthNotification(99, DocInfo.DocumentTypeID.Value, DocInfo.DocumentID.Value, UserID);
                        snackbar.Add("Approved Successfully", Severity.Success);
                        await OnApproved.InvokeAsync();
                    }
                    catch (Exception ex)
                    {
                        snackbar.Add(ex.Message, Severity.Error);
                    }
                }
            }
            else
            {
                bool? result = await dialogService.ShowMessageBox(
                    "Permission denied!",
                    string.Format("You dont have permission to Approve {0} !.", DocInfo.DocumentName));
            }
        }

        public async Task RemoveFileChange(bool val)
        {
            if (val)
            {
                var authprov = await auth.GetAuthenticationStateAsync();
                var user = authprov.User;
                UserID = user.Identity.Name;

                baseInterface.RemoveFile(DocInfo.DocumentID, UserID);
                model.FileName = null;
                StateHasChanged();
                snackbar.Add("File removed Successfully", Severity.Success);
            }
        }

    }
}
