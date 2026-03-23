using ArmsModels.BaseModels;
using ArmsServices.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Components.Web;

namespace Views.Data
{
    [Authorize]
    public abstract class ViewComponentBase<T> : ComponentBase where T : class
    {
        [Inject] protected AuthenticationStateProvider auth { get; set; }
        [Inject] protected ISnackbar snackbar { get; set; }
        [Inject] protected IDialogService dialogService { get; set; }
        [Inject] protected IRoleService<RoleModel> Irole { get; set; }
        [Inject] protected IPushNotificationService notiService { get; set; }
        [Inject] protected IbaseInterface<T> Ibase { get; set; }

        [Parameter]
        public int? id { get; set; } = null;
        [Parameter]
        public bool interbranch { get; set; } = false;

        protected abstract DocumentInfoModel DocInfo { get; set; }
        public bool ShowApproved { get; set; } = false;        
        protected MudTable<T> _table;
        protected abstract List<T> _collection { get; set; }

        private int numberOfRecords = 100;
        protected string searchTerm = "";
        protected bool _busy;
        protected bool _isLoading { get; set; } = true;
        protected bool ViewPermission { get; set; }
        protected bool EditPermission { get; set; }
        protected bool DeletePermission { get; set; }
        protected bool ApprovePermission { get; set; }
        protected int? BranchID = null;
        protected string UserID = null;     

        protected async Task HandleSearchKeyUp(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                //LoadData(ShowApproved, NumberOfRecords);
                await _table.ReloadServerData();
            }
        }

        protected async Task ToggleData(bool value)
        {
            ShowApproved = value;            
            //await InvokeAsync(StateHasChanged);
            await  _table.ReloadServerData();           
        }

        protected int NumberOfRecords
        {
            get => numberOfRecords;
            set
            {
                if (numberOfRecords != value)
                {
                    numberOfRecords = value;
                    //LoadData(ShowApproved, numberOfRecords);
                }
            }
        }
        
        // protected abstract void LoadData(bool val, int numberOfRecords);

        protected abstract Task OpenForm(T editModel, bool ReadOnly, bool DisableAddition = false);

        protected virtual async Task Delete(int? ID)
        {
            if (DeletePermission)
            {
                bool? result = await dialogService.ShowMessageBoxAsync(
                    "Warning !",
                    "Are you sure you want to delete this Document ?");
                if (result == true)
                {
                    Ibase.Delete(ID, UserID);
                    snackbar.Add("Deleted Successfully", Severity.Success);
                }
                
                await _table.ReloadServerData();
            }
            else
            {
                bool? result = await dialogService.ShowMessageBoxAsync(
                    "Permission denied!",
                    "You don't have any permission to Delete Payment.");
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await setPermissions();

            var authprov = await auth.GetAuthenticationStateAsync();
            BranchID = int.Parse(authprov.User.Claims.First(x => x.Type == "BranchID").Value);
            var user = authprov.User;
            UserID = user.Identity.Name;            
        }

        private async Task setPermissions()
        {
            CancellationTokenSource ctc = new CancellationTokenSource();
            ViewPermission = await Irole.HasClaim(DocInfo.DocumentTypeID.ToString(), "View", ctc.Token);
            EditPermission = await Irole.HasClaim(DocInfo.DocumentTypeID.ToString(), "Edit", ctc.Token);
            DeletePermission = await Irole.HasClaim(DocInfo.DocumentTypeID.ToString(), "Delete", ctc.Token);
            ApprovePermission = await Irole.HasClaim(DocInfo.DocumentTypeID.ToString(), "Approve", ctc.Token);
        }      

        protected virtual async Task<TableData<T>> LoadServerData(TableState state, CancellationToken token)
        {
            await Task.Yield();
            var list = Ibase.SelectAll(BranchID,  state.Page, state.PageSize, searchTerm, ShowApproved);
            _collection = list.Items; 
            return new TableData<T>
            {
                Items = _collection,
                TotalItems = list.TotalRecords ?? 0
            };
        }
    }
}