using ArmsModels.BaseModels;
using ArmsServices.DataServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Views.Pages.Gc
{
    public partial class GcDialog : ComponentBase
    {
        [Inject]
        IGcService Iservice { get; set; }
        [Inject] IRouteService Iroute { get; set; }
        [Inject] IOrderService Iorder { get; set; }
        [Inject] IConsigneeService Iconsignee { get; set; }

        [Inject] MudBlazor.ISnackbar snackbar { get; set; }
        [Inject] AuthenticationStateProvider auth { get; set; }
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public GcSetModel model { get; set; }
        [Parameter] public OrderModel Order { get; set; }




        private void Cancel()
        {
            MudDialog.Cancel();
        }

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


        protected override async Task OnInitializedAsync()
        {
            var e = await auth.GetAuthenticationStateAsync();
            string BranchIDString = e.User.Claims.First(x => x.Type == "BranchID").Value;
            int BranchID = int.Parse(BranchIDString);

            IsLoading = true;
            GcTypes = new();
            foreach (var item in Iservice.SelectGcTypes())
            {
                GcTypes.Add(item);
            }
            await foreach (var item in Iorder.Select(0))
            {
                Orders.Add(item);
            }

            if (Order == null)
            {
                if (model?.GcSetID > 0)
                    Order = Orders.Find(x => x.OrderID == model.OrderID);
                else
                if (Orders.Count == 1) Order = Orders.First();
            }
            else
            {
                model = new() { BranchID = BranchID };
            }

            await OrderChanged(Order);

            if (model?.GcSetID > 0)
            {
                Route = Routes.Find(x => x.RouteID == model.RouteID);
                Consignor = Consignees.Find(x => x.ConsigneeID == model.ConsignorID);
                Consignee = Consignees.Find(x => x.ConsigneeID == model.ConsigneeID);
            }

            if (model.Gcs.Count == 0)
            {
                model.Gcs.Add(new GcModel());
            }
            IsLoading = false;

        }

        private async Task OrderChanged(OrderModel Order)
        {
            IsLoading = true;
            Routes.Clear();
            Consignees.Clear();
            Route = null;
            Consignor = null;
            Consignee = null;
            model.ConsignorID = null;
            model.ConsigneeID = null;
            model.RouteID = null;

            if (Order != null)
            {
                await foreach (var item in Iroute.SelectByOrder(Order?.OrderID))
                {
                    Routes.Add(item);
                }

                await foreach (var item in Iconsignee.SelectByOrder(Order.OrderID))
                {
                    Consignees.Add(item);
                }
                if (Routes.Count() == 1)
                {
                    Route = Routes.First();
                    model.RouteID = Route.RouteID;
                }
                if (Consignees.Where(x => x.Consignor == true).Count() == 1)
                {
                    Consignor = Consignees.First(x => x.Consignor == true);
                    model.ConsignorID = Consignor.ConsigneeID;
                }
                if (Consignees.Where(x => x.Consignor == false).Count() == 1)
                {
                    Consignee = Consignees.First(x => x.Consignor == false);
                    model.ConsigneeID = Consignee.ConsigneeID;
                }
            }

            GetFreight(model);
            IsLoading = false;
        }

        private void GetFreight(GcSetModel GcSet)
        {
            GcSet.Gcs.ForEach(x => x.Freight = Iservice.GetFreight(GcSet.OrderID, GcSet.RouteID, null, x.BillQuantity));
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
            if (!Orders.Select(x => x.OrderName).Contains(Order.OrderName))
            {
                snackbar.Add("Invalid Order", Severity.Error);
                return;
            }
            else
                 if (!Routes.Select(x => x.RouteName).Contains(Route.RouteName))
            {
                snackbar.Add("Invalid Route", Severity.Error);
                return;
            }

            model.OrderID = Order.OrderID;
            model.RouteID = Route.RouteID;
            model.ConsignorID = Consignor.ConsigneeID;
            model.ConsigneeID = Consignee.ConsigneeID;

            var authprov = await auth.GetAuthenticationStateAsync();

            foreach (var item in model.Gcs)
            {
                item.UserInfo.UserID = authprov.User.Identity.Name;
            }
            try
            {
                model = Iservice.Update(model);
                snackbar.Add("Saved Successfully", Severity.Success);
                MudDialog.Close(DialogResult.Ok(model));

                model = new GcSetModel();
                Route = new RouteModel();
                Consignor = new ConsigneeModel();
                Consignee = new ConsigneeModel();

            }
            catch (Exception ex)
            {
                snackbar.Add(ex.Message, Severity.Warning);
            }
        }

        private async Task OrderSelected(OrderModel obj)
        {
            Order = obj;
            model.OrderID = obj?.OrderID;
            await OrderChanged(Order);
        }

        private void RouteChanged(RouteModel obj)
        {
            Route = obj;
            model.RouteID = obj?.RouteID;
            GetFreight(model);
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
            GetFreight(model);
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

    }
}