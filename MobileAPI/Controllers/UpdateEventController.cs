using ArmsModels.BaseModels;
using ArmsServices.DataServices;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IRoleService _roleService;
        private readonly IGcService _Igc;
        public UpdateEventController(IRoleService roleService, IGcService igc, IEventService Ievent)
        {
            _roleService = roleService;
            _Igc = igc;
        }
        public bool HasPermissionEventServiceEdit { get; set; } = false;
        public int DocTypeID = 48;
        private EventTypeModel SelectedEvent = new();
        CancellationTokenSource ctc = new CancellationTokenSource();

        [HttpPost]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]

        //public async Task<string> CheckPermission(EventModel model)
        //{
        //    string result = "";
        //    HasPermissionEventServiceEdit = await _roleService.HasClaim(DocTypeID.ToString(), "Edit", ctc.Token);
        //    if (HasPermissionEventServiceEdit)
        //    {
        //        if (model.EventTime <= DateTime.Now)
        //        {
        //            List<GcSetModel> gcSetList = _Igc.SelectToUnload(model.TripID);
        //            if (gcSetList != null && gcSetList.Count() != 0)
        //            {
        //                if (gcSetList.Any() && model.EventTypeID == 6)
        //                {
        //                    //snackbar.Add("The trip is not unloaded", Severity.Warning);
        //                    result = "The trip is not unloaded";
        //                    return result;
        //                }
        //            }
        //            if (isSubmitting)
        //                return;
        //            isSubmitting = true;
        //            try
        //            {
        //                SelectedEvent = Ievent.GetEventType(model.EventTypeID);
        //                if (model.TruckEventID == null && SelectedEvent.IsDriverRequired == true && AssignedDriver == null)
        //                {
        //                    return  "Cannot start this event without Driver";
        //                }
        //                model.EventTypeID = SelectedEvent.EventTypeID;
        //                if (model.EventTypeID == 5 && model.TruckEventID == null)
        //                {
        //                    List<GcSetModel> GcsToUnload = new();
        //                    GcsToUnload = _Igc.SelectedUnloadEvent(model.TripID);
        //                    if (GcsToUnload.Count == 0)
        //                    {
        //                        return "No Consignment being Unloaded";
        //                    }

        //                    foreach (var item in GcsToUnload)
        //                    {
        //                        if (item.SetUnloadQuantity == null)
        //                        {
        //                            item.SetUnloadQuantity = item.TotalBillQuantity;
        //                        }
        //                    }

        //                    DialogParameters parms = new DialogParameters();
        //                    parms.Add("GcsToUnload", GcsToUnload);
        //                    var dialog = DialogService.Show<UnloadQuantityDialog>("Unloading Consignment", parms);
        //                    var result = await dialog.Result;
        //                    if (!result.Canceled)
        //                    {
        //                        List<GcSetModel> gcs = (List<GcSetModel>)result.Data;
        //                        foreach (var item in gcs)
        //                        {
        //                            Igc.UpdateUnloadingQuantity(item);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        return result;
        //                    }
        //                }
        //                if (model.EventTypeID == 6)
        //                {
        //                    bool IsbranchSetting = branchSettingsService.IsEnabled(model.BranchID, 109);
        //                    if (IsbranchSetting)
        //                    {
        //                        DialogOptions DialogWidth = new DialogOptions() { MaxWidth = MaxWidth.Small, FullWidth = true };
        //                        var dialog = DialogService.Show<AcceptedKMDialog>("Update Accepted KM", DialogWidth);
        //                        var result = await dialog.Result;
        //                        if (!result.Canceled)
        //                        {
        //                            int AcceptedKM = (int)result.Data;
        //                            model.AcceptedKM = AcceptedKM;
        //                        }
        //                        else
        //                        {
        //                            return result;
        //                        }
        //                    }
        //                }
        //                if (!ValidateEvent(model))
        //                {
        //                    return result;
        //                }
        //                if (SelectedEvent.EventTypeID == 4 && model.TruckEventID == null)
        //                {
        //                    if (!SelectedGCs.Any())
        //                    {
        //                        return result = "No GCs selected to Unload!", Severity.Warning);
        //                    }
        //                }

        //                model = Ievent.Update(model);

        //                if (SelectedEvent.EventTypeID == 4)
        //                {
        //                    foreach (GcSetModel item in SelectedGCs)
        //                    {
        //                        _Igc.BeginUnload(CurrentTrip.TripID, item.GcSetID);
        //                    }
        //                }
        //                MudDialog.Close(DialogResult.Ok(model));
        //            }
        //            catch (Exception ex)
        //            {
        //                result = ex.Message;
        //            }
        //            finally
        //            {
        //                isSubmitting = false;
        //                await Task.Delay(200);
        //                _busy = false;
        //            }
        //        }
        //        else
        //        {
        //            result = "EventDate cannot be a greater than today's date.";
        //        }
        //    }
        //    else
        //    {
        //        result = "Permission denied! You dont have any permission to Add or Edit Event.";
        //    }
        //    return result;
        //}
    }
}
