using ArmsModels.BaseModels;
using ArmsServices.DataServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Views.Pages.Operations.Place;
using static NuGet.Packaging.PackagingConstants;


namespace Views.Pages.Operations.Gc
{
    public partial class GcDialog : ComponentBase
    {
        [Inject] IRoleService<RoleModel> Irole { get; set; }
        [Inject] IGcService Iservice { get; set; }
        [Inject] IRouteService Iroute { get; set; }
        [Inject] IOrderService Iorder { get; set; }
        [Inject] ITariffService ITariff { get; set; }
        [Inject] IConsigneeService Iconsignee { get; set; }
        [Inject] MudBlazor.ISnackbar snackbar { get; set; }
        [Inject] AuthenticationStateProvider auth { get; set; }
        [Inject] IDialogService DialogService { get; set; }
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public GcSetModel model { get; set; } = new GcSetModel();
        [Parameter] public OrderModel Order { get; set; }

        private bool IsLoading = false;
        private RouteModel Route = new();
        private ConsigneeModel Consignor = new();
        private ConsigneeModel Consignee = new();

        private List<RouteModel> Routes = new();
        private List<OrderModel> Orders = new();
        private List<GcTypeModel> GcTypes = new();
        private List<ConsigneeModel> Consignees = new();

        private int _tabIndex = 0;
        private bool _tabAdded = false;
        private bool _busy;
        string UserID;int BranchID;
        public bool HasPermissionGcServiceEdit { get; set; } = false;
        public int DocTypeID = 46;

        protected async override Task OnInitializedAsync()
        {
            CancellationTokenSource ctc = new CancellationTokenSource();
            HasPermissionGcServiceEdit = await Irole.HasClaim(DocTypeID.ToString(), "Edit", ctc.Token);

            var e = await auth.GetAuthenticationStateAsync();
            string BranchIDString = e.User.Claims.First(x => x.Type == "BranchID").Value;
             BranchID = int.Parse(BranchIDString);
            UserID =   e.User.Identity.Name;

            GcTypes = Iservice.SelectGcTypes().ToList();
            Orders = await Iorder.SelectByBranch(BranchID).ToListAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            IsLoading = true;
            Order = Order == null ? Orders.FirstOrDefault(x => x.OrderID == model.OrderID) : Order;
            await OrderSelected(Order);

            if (model.GcSetID != null)
            {
                Route = Routes.FirstOrDefault(x => x.RouteID == model.RouteID);
                Consignor = Consignees.FirstOrDefault(x => x.ConsigneeID == model.ConsignorID);
                Consignee = Consignees.FirstOrDefault(x => x.ConsigneeID == model.ConsigneeID);
            }
            if (model.Gcs.Count == 0)
            {
                model.Gcs.Add(new GcModel());
            }
            IsLoading = false;
        }
        private void Cancel()
        {
            MudDialog.Cancel();
        }
        private GcSetModel GetFreight(GcSetModel GcSet)
        {
            GcSet.Gcs.Where(x => (x.Freight??0) == 0 ).ToList().ForEach(x => x.Freight = ITariff.GetPrimaryFreight(GcSet.OrderID, GcSet.RouteID, null, x.BillQuantity, x.Freight));
            return GcSet;
        }

        private async Task<IEnumerable<ConsigneeModel>> SearchConsignee(string searchString)
        {
            return await Task.FromResult(Consignees.Where(x => x.Consignor == false).Where(x => x.ConsigneeName.Contains(searchString == null ? string.Empty : searchString, StringComparison.InvariantCultureIgnoreCase)));
        }

        private async Task<IEnumerable<ConsigneeModel>> SearchConsignor(string searchString)
        {
            return await Task.FromResult(Consignees.Where(x => x.Consignor == true).Where(x => x.ConsigneeName.Contains(searchString == null ? string.Empty : searchString, StringComparison.InvariantCultureIgnoreCase)));
        }

        private async Task<IEnumerable<OrderModel>> SearchOrders(string searchString)
        {
            return await Task.FromResult(Orders.Where(x => x.OrderName.Contains(searchString == null ? string.Empty : searchString, StringComparison.InvariantCultureIgnoreCase)));
        }

        private async Task<IEnumerable<RouteModel>> SearchRoutes(string searchString)
        {
            return await Task.FromResult(Routes.Where(x => x.RouteName.Contains(searchString == null ? string.Empty : searchString, StringComparison.InvariantCultureIgnoreCase)));
        }

        private async Task OnValidSubmit(EditContext context)
        {
            if (_busy)
            {
                snackbar.Add("Please wait while form is being submitted!", Severity.Warning);
                return;
            }
            if (HasPermissionGcServiceEdit)
            {
                _busy = true;
                model.OrderID = Order.OrderID;
                model.RouteID = Route.RouteID;
                model.ConsignorID = Consignor.ConsigneeID;
                model.ConsigneeID = Consignee.ConsigneeID;                
                model.UserInfo.UserID = UserID; 
                model.BranchID = BranchID;
                model.Gcs.ForEach(x => x.UserInfo = model.UserInfo);
                try
                {
                    model = Iservice.Update(model);
                    snackbar.Add("Saved Successfully", Severity.Success);
                    MudDialog.Close(DialogResult.Ok(model));
                   
                    Route = new RouteModel();
                    Consignor = new ConsigneeModel();
                    Consignee = new ConsigneeModel();
                }
                catch (Exception ex)
                {
                    snackbar.Add(ex.Message, Severity.Error);
                }
                await Task.Delay(200);
                _busy = false;
            }
            else
            {
                bool? ResultModel = await DialogService.ShowMessageBox(
                    "Permission denied!",
                    "You dont have any permission to Add or Edit GC.");
            }   
        }

        private async Task OrderSelected(OrderModel obj)
        {
            Order = obj;
            model.OrderID = obj?.OrderID;
            Routes.Clear();
            Consignees.Clear();
            Route = null;
            Consignor = null;
            Consignee = null;
            if (Order != null)
            {
                Routes = await Iroute.SelectByOrder(Order?.OrderID).ToListAsync();
                Consignees = await Iconsignee.SelectByOrder(Order.OrderID).ToListAsync();
                if (model.GcSetID == null)
                {
                    if (Routes.Count() == 1)
                    {
                        Route = Routes.First();
                        model.RouteID = Route.RouteID;
                    }
                    if (Consignees.Where(x => x.Consignor == true).Count() == 1)
                    {
                        Consignor = Consignees.FirstOrDefault(x => x.Consignor == true);
                        model.ConsignorID = Consignor.ConsigneeID;
                    }
                    if (Consignees.Where(x => x.Consignor == false).Count() == 1)
                    {
                        Consignee = Consignees.First(x => x.Consignor == false);
                        model.ConsigneeID = Consignee.ConsigneeID;
                    }
                }
                GetFreight(model);
            }
            else
            {
                model.Gcs.ForEach(x=> x.Freight = 0);
            }
            
        }

        private void RouteChanged(RouteModel obj)
        {
            Route = obj;
            model.RouteID = obj?.RouteID;
            GetFreight(model);
        }

        private async Task AddConsignee_Consignor()
        {

            DialogParameters parms = new DialogParameters();
            ConsigneeModel consigneeModel = new();
            parms.Add("model", consigneeModel.Clone());
            parms.Add("Order", Order);
            var dialog = DialogService.Show<Consineedilog>("Add/Edit Consignee", parms, new DialogOptions() { MaxWidth = MaxWidth.Large });
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                Route = Routes.FirstOrDefault(x => x.RouteID == model.RouteID);
                Consignor = Consignees.FirstOrDefault(x => x.ConsigneeID == model.ConsignorID);
                Consignee = Consignees.FirstOrDefault(x => x.ConsigneeID == model.ConsigneeID);
            }
        }

        private void ConsignorChanged(ConsigneeModel obj)
        {
            Consignor = obj;
            model.ConsignorID = obj?.ConsigneeID;
        }

        private void ConsigneeChanged(ConsigneeModel obj)
        {
            Consignee = obj;
            model.ConsigneeID = obj?.ConsigneeID;
        }

        private void QtyChanged(decimal? Qty, GcModel gc)
        {
            gc.BillQuantity = Qty;            
            gc.Freight = ITariff.GetPrimaryFreight(model.OrderID, model.RouteID, null, gc.BillQuantity, gc.Freight);  
        }

        private void FrChanged(decimal? Freight, GcModel gc)
        {
            gc.Freight = Freight;
            //if (Freight < gc.EFreight)
            //{
            //    gc.Freight = null;
            //    snackbar.Add("The Freight should not be less than that master Freight", Severity.Warning);
            //}
           
            //GetFreight(model);
            //StateHasChanged();
        }

        private void AddMoreInvoice()
        {
            model.Gcs.Add(new GcModel());
            _tabAdded = true;
        }

        private void RemoveInvoice(MudTabPanel tabPanel)
        {
            var item = (GcModel)tabPanel.Tag;
            model.Gcs.Remove(item);
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (_tabAdded)
            {
                _tabAdded = false;
                _tabIndex = model.Gcs.Count - 1;
                StateHasChanged();
            }
        }

        private async Task Delete()
        {
            bool? result = await DialogService.ShowMessageBox(
           "Warning",
           "Deleting can not be undone!",
           yesText: "Delete!", cancelText: "Cancel");
            if (result is not null && result.Value)
            {
                Iservice.DeleteSet(model.GcSetID, UserID);
                StateHasChanged();
            }
        }
    }
}