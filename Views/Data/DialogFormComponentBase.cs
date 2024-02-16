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
        [Inject] protected IPushNotificationService notiService { get; set; } 


        [Parameter]
        public bool ReadOnly { get; set; }


        [Parameter]
         public virtual T model { get; set; }



        protected bool ViewPermission { get; set; }
        protected bool EditPermission { get; set; }
        protected bool DeletePermission { get; set; }
        protected bool ApprovePermission { get; set; }

        protected EditContext editContext;
        protected DialogForm dialogForm;
        protected int? BranchID = null;
        protected string UserID = null;

        protected string DocNumber { get; set; }
        protected DateTime? DocDate { get; set; }       
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


        protected abstract int Update(T editModel);
        protected abstract int UpdateApproval(DataApprovedStatus aprvd);
        

        protected virtual async Task OnValidSubmit(EditContext context)
        {
            if (_busy)
            {
                snackbar.Add("Please wait while form is being submitted!", Severity.Warning);
                return;
            }
            _busy = true;
            if (model.TotalAmount <= 0)
            {
                snackbar.Add("Total amount should be greater than zero!", Severity.Error);
                _busy = false;
                return;
            }
            if(!EditPermission)
            {
                bool? result = await dialogService.ShowMessageBox("Permission denied!", "You dont have permission to Edit Payment!.");
            }    
            try
            {
                model.UserInfo.UserID = UserID;
                int ID = Update(model);                
                
                notiService.CreateAuthNotifications(BranchID.Value, DocInfo.DocumentTypeID.Value, DocInfo.DocumentID.Value);
                snackbar.Add("Submitted Successfully", Severity.Success);
                dialogForm.Close(model);
            }
            catch (Exception ex)
            {
                snackbar.Add(ex.Message, Severity.Error);
            }

            await Task.Delay(200);
            _busy = false;
        }

       

        protected virtual async Task Approve(DataApprovedStatus aprvd)
        {
            if (ApprovePermission)
            {
                if (aprvd.IsApprove)
                {
                    try
                    {
                        model.UserInfo.UserID = UserID;
                        UpdateApproval(aprvd);
                        notiService.AcknowledgeAuthNotification(99,DocInfo.DocumentTypeID.Value,DocInfo.DocumentID.Value,UserID);
                        snackbar.Add("Approved Successfully", Severity.Success);
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
                    string.Format( "You dont have permission to Approve {0} !.",DocInfo.DocumentName));
            }
        }

    }
    
}
