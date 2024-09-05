using ArmsModels.BaseModels;
using ArmsServices.DataServices;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileAPI.Services;
using System;
using System.Threading;

namespace MobileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateEventController : ControllerBase
    {
        private readonly ITruckService _truckService;
        public UpdateEventController(ITruckService truckService)
        {
            _truckService = truckService;
        }
        public bool HasPermissionEventServiceEdit { get; set; } = false;

        [HttpGet]
        public IEnumerable<TruckModel> SelectByBranch(int? BranchID, string Filer, string HomeOrOperation = "Operation")
        {
            return _truckService.SelectByBranch(BranchID, Filer, HomeOrOperation);
        }
        //[HttpPost]
        //public IEnumerable<UserBranchRoleModel> EventUpdate()
        //{
        //    CancellationTokenSource ctc = new CancellationTokenSource();
        //    HasPermissionEventServiceEdit = _roleService.HasClaim("48", "Edit", ctc.Token);

        //    //if (HasPermissionEventServiceEdit)
        //    //{
        //    //    if (model.EventTime <= DateTime.Now)
        //    //    {
        //    //        List<GcSetModel> gcSetList = Igc.SelectToUnload(model.TripID);
        //    //        if (gcSetList != null && gcSetList.Count() != 0)
        //    //        {
        //    //            if (gcSetList.Any() && model.EventTypeID == 6)
        //    //            {
        //    //                snackbar.Add("The trip is not unloaded", Severity.Warning);
        //    //                return;
        //    //            }
        //    //        }
        //    //        if (isSubmitting)
        //    //            return;
        //    //        isSubmitting = true;
        //    //        _busy = true;
        //    //        try
        //    //        {
        //    //            if (model.TruckEventID == null && SelectedEvent.IsDriverRequired == true && AssignedDriver == null)
        //    //            {
        //    //                snackbar.Add("Cannot start this event without Driver", Severity.Warning);
        //    //                return;
        //    //            }
        //    //            model.EventTypeID = SelectedEvent.EventTypeID;
        //    //            if (model.EventTypeID == 5 && model.TruckEventID == null)
        //    //            {
        //    //                List<GcSetModel> GcsToUnload = new();
        //    //                GcsToUnload = Igc.SelectedUnloadEvent(model.TripID);
        //    //                if (GcsToUnload.Count == 0)
        //    //                {
        //    //                    snackbar.Add("No Consignment being Unloaded", Severity.Error);
        //    //                    return;
        //    //                }

        //    //                foreach (var item in GcsToUnload)
        //    //                {
        //    //                    if (item.SetUnloadQuantity == null)
        //    //                    {
        //    //                        item.SetUnloadQuantity = item.TotalBillQuantity;
        //    //                    }
        //    //                }

        //    //                DialogParameters parms = new DialogParameters();
        //    //                parms.Add("GcsToUnload", GcsToUnload);
        //    //                var dialog = DialogService.Show<UnloadQuantityDialog>("Unloading Consignment", parms);
        //    //                var result = await dialog.Result;
        //    //                if (!result.Canceled)
        //    //                {
        //    //                    List<GcSetModel> gcs = (List<GcSetModel>)result.Data;
        //    //                    foreach (var item in gcs)
        //    //                    {
        //    //                        Igc.UpdateUnloadingQuantity(item);
        //    //                    }
        //    //                }
        //    //                else
        //    //                {
        //    //                    return;
        //    //                }
        //    //            }
        //    //            if (model.EventTypeID == 6)
        //    //            {
        //    //                bool IsbranchSetting = branchSettingsService.IsEnabled(model.BranchID, 109);
        //    //                if (IsbranchSetting)
        //    //                {
        //    //                    DialogOptions DialogWidth = new DialogOptions() { MaxWidth = MaxWidth.Small, FullWidth = true };
        //    //                    var dialog = DialogService.Show<AcceptedKMDialog>("Update Accepted KM", DialogWidth);
        //    //                    var result = await dialog.Result;
        //    //                    if (!result.Canceled)
        //    //                    {
        //    //                        int AcceptedKM = (int)result.Data;
        //    //                        model.AcceptedKM = AcceptedKM;
        //    //                    }
        //    //                    else
        //    //                    {
        //    //                        return;
        //    //                    }
        //    //                }
        //    //            }
        //    //            if (!ValidateEvent(model))
        //    //            {
        //    //                return;
        //    //            }
        //    //            if (SelectedEvent.EventTypeID == 4 && model.TruckEventID == null)
        //    //            {
        //    //                if (!SelectedGCs.Any())
        //    //                {
        //    //                    snackbar.Add("No GCs selected to Unload!", Severity.Warning);
        //    //                    return;
        //    //                }
        //    //            }

        //    //            model = Ievent.Update(model);

        //    //            if (SelectedEvent.EventTypeID == 4)
        //    //            {
        //    //                foreach (GcSetModel item in SelectedGCs)
        //    //                {
        //    //                    Igc.BeginUnload(CurrentTrip.TripID, item.GcSetID);
        //    //                }
        //    //            }
        //    //            MudDialog.Close(DialogResult.Ok(model));
        //    //        }
        //    //        catch (Exception ex)
        //    //        {
        //    //            snackbar.Add(ex.Message, Severity.Warning);
        //    //        }
        //    //        finally
        //    //        {
        //    //            isSubmitting = false;
        //    //            await Task.Delay(200);
        //    //            _busy = false;
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        snackbar.Add("EventDate cannot be a greater than today's date.", Severity.Warning);
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    bool? ResultModel = await DialogService.ShowMessageBox(
        //    //        "Permission denied!",
        //    //        "You dont have any permission to Add or Edit Event.");
        //    //}
        //}
    }
}
