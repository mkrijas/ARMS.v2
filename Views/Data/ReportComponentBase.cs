using ArmsModels.BaseModels;
using ArmsServices.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Views.Shared.Selectors;

namespace Views.Data
{
    [Authorize]
    public abstract class ReportComponentBase : ComponentBase
    {
        [Inject] protected AuthenticationStateProvider auth { get; set; }
        [Inject] protected ISnackbar snackbar { get; set; }
        [Inject] protected IDialogService dialogService { get; set; }
        [Inject] protected IRoleService<RoleModel> Irole { get; set; }
        [Inject] protected IPushNotificationService notiService { get; set; }

        protected int? BranchID = null;
        protected string UserID = null;

        private int? HoID = 7;
        protected List<int> SelectedBranches { get; set; } = new();

        protected Dictionary<string, string> ReportParameters { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {   
            var authprov = await auth.GetAuthenticationStateAsync();
            BranchID = int.Parse(authprov.User.Claims.First(x => x.Type == "BranchID").Value);
            SelectedBranches.Add(BranchID.Value);
            var user = authprov.User;
            UserID = user.Identity.Name;           
            await Task.Delay(100);
        }

        protected async Task SelectBranches()
        {
            DialogParameters parms = new DialogParameters
            {
                { "SelectedBranches", SelectedBranches },
                { "EnableSelector" , HoID == BranchID }
            };
            var dialog = dialogService.Show<MultiBranchSelector>("Select Branches", parms, new DialogOptions() { MaxWidth = MaxWidth.Large });
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                SelectedBranches = (List<int>)result.Data;
            }
        }

    }
}
