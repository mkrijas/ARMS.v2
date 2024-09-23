using ArmsModels.BaseModels;
using ArmsServices.DataServices;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileAPI.Services;

//using MobileAPI.Services;
using System;
using System.Diagnostics;
using System.Threading;

namespace MobileAPI.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class UpdateEventController : ControllerBase
    {
        private readonly IRoleService<RoleModel> _roleService;
        private readonly IGcService _Igc;
        private readonly IEventService Ievent;
        private readonly IDriverService Idriver;
        private readonly IBranchSettingsService branchSettingsService;
        private readonly IUserService _userService;
        private readonly ITripService Itrip;
        private readonly ITruckService Itruck;
        private readonly IBranchService Ibranch;
        private readonly ITruckStatusUpdateService _truckStatusUpdateService;

        public UpdateEventController(IRoleService<RoleModel> roleService, IGcService igc, IEventService events,
            IDriverService driverService, IBranchSettingsService branchSettings, IUserService userService, ITripService itrip,
            ITruckService itruck, IBranchService ibranch, ITruckStatusUpdateService truckStatusUpdateService)
        {
            _roleService = roleService;
            _Igc = igc;
            Ievent = events;
            Idriver = driverService;
            branchSettingsService = branchSettings;
            _userService = userService;
            Itrip = itrip;
            Itruck = itruck;
            Ibranch = ibranch;
            _truckStatusUpdateService = truckStatusUpdateService;
        }

        public bool HasPermissionEventServiceEdit { get; set; } = false;
        public int DocTypeID = 48;
        private EventTypeModel SelectedEvent = new();
        CancellationTokenSource ctc = new CancellationTokenSource();
        public DriverModel AssignedDriver { get; set; }
        private IEnumerable<GcSetModel> SelectedGCs = new HashSet<GcSetModel>();
        public TripModel CurrentTrip { get; set; }
        private EventModel PreEvent { get; set; }
        EventTypeModel PreEventType;
        public TruckModel SelectedTruck { get; set; }
        private EventTypeModel EventType { get; set; }
        private EventModel CurrentEvent { get; set; }
        string AssignDriverLabel;
        string TruckStatus;


        [HttpPost("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]

        public async Task<string> UpdateEvents(EventUpdateModel updateModel)
        {
            EventModel model = updateModel.Event;
            GcSetModel gcSet = updateModel.GcSet;
            ((HashSet<GcSetModel>)SelectedGCs).Add(gcSet);
            //, [FromQuery] GcSetModel gcSet
            string result = "";
            //HasPermissionEventServiceEdit = await _roleService.HasClaim(DocTypeID.ToString(), "Edit", ctc.Token);
            HasPermissionEventServiceEdit = _userService.GetClaimsAsync(model.UserInfo.UserID, DocTypeID.ToString(), "Edit", model.BranchID, ctc.Token);
            if (HasPermissionEventServiceEdit)
            {

                DateTime originalDateTime = DateTime.Now;
                DateTime dateTimeWithoutSeconds = new DateTime(
                    originalDateTime.Year,
                    originalDateTime.Month,
                    originalDateTime.Day,
                    originalDateTime.Hour,
                    originalDateTime.Minute,
                    0
                );

                if (model.EventTime <= dateTimeWithoutSeconds)
                {
                    List<GcSetModel> gcSetList = _Igc.SelectToUnload(model.TripID);
                    if (gcSetList != null && gcSetList.Count() != 0)
                    {
                        if (gcSetList.Any() && model.EventTypeID == 6)
                        {
                            result = "The trip is not unloaded";
                            return result;
                        }
                    }
                    try
                    {
                        SelectedEvent = Ievent.GetEventType(model.EventTypeID);
                        AssignedDriver = AssignedDriver ?? Idriver.SelectByID(model.DriverID);
                        if (model.TruckEventID == null && SelectedEvent.IsDriverRequired == true && AssignedDriver == null)
                        {
                            return "Cannot start this event without Driver";
                        }
                        model.EventTypeID = SelectedEvent.EventTypeID;
                        if (model.EventTypeID == 5 && model.TruckEventID == null)
                        {
                            List<GcSetModel> GcsToUnload = new();
                            GcsToUnload = _Igc.SelectedUnloadEvent(model.TripID);
                            if (GcsToUnload.Count == 0)
                            {
                                return "No Consignment being Unloaded";
                            }

                            foreach (var item in GcsToUnload)
                            {
                                if (item.SetUnloadQuantity == null)
                                {
                                    item.SetUnloadQuantity = item.TotalBillQuantity;
                                }
                            }

                            if (GcsToUnload.Any(x => x.TotalUnloadingQuantity == null) && model.EventTypeID == 5)
                            {
                                return result = "Unload Quantity not entered";
                            }
                            else
                            {
                                List<GcSetModel> gcs = (List<GcSetModel>)GcsToUnload;
                                foreach (var item in gcs)
                                {
                                    _Igc.UpdateUnloadingQuantity(item);
                                }
                            }

                            //DialogParameters parms = new DialogParameters();
                            //parms.Add("GcsToUnload", GcsToUnload);
                            //var dialog = DialogService.Show<UnloadQuantityDialog>("Unloading Consignment", parms);
                            //var result = await dialog.Result;
                            //if (!result.Canceled)
                            //{
                            //    List<GcSetModel> gcs = (List<GcSetModel>)result.Data;
                            //    foreach (var item in gcs)
                            //    {
                            //        Igc.UpdateUnloadingQuantity(item);
                            //    }
                            //}
                            //else
                            //{
                            //    return result;
                            //}
                        }
                        if (model.EventTypeID == 6)
                        {
                            bool IsbranchSetting = branchSettingsService.IsEnabled(model.BranchID, 109);
                            //if (IsbranchSetting)
                            //{
                            //    DialogOptions DialogWidth = new DialogOptions() { MaxWidth = MaxWidth.Small, FullWidth = true };
                            //    var dialog = DialogService.Show<AcceptedKMDialog>("Update Accepted KM", DialogWidth);
                            //    var result = await dialog.Result;
                            //    if (!result.Canceled)
                            //    {
                            //        int AcceptedKM = (int)result.Data;
                            //        model.AcceptedKM = AcceptedKM;
                            //    }
                            //    else
                            //    {
                            //        return result;
                            //    }
                            //}
                        }
                        //if (!ValidateEvent(model))
                        //{
                        //    return result;
                        //}
                        PreEvent = model.TruckEventID == null ? Ievent.GetCurrentEvent(model.TruckID) : Ievent.GetPreviousEvent(model.TruckEventID);
                        PreEventType = Ievent.GetEventType(PreEvent.EventTypeID);
                        EventModel NextEvent = new();
                        NextEvent = Ievent.GetNextEvent(model.TruckEventID);

                        string res = "";
                        if (PreEvent != null && model.EventTime <= PreEvent.EventTime)
                        {
                            return result = "Event Time cannot be before previous event";
                        }
                        else if (NextEvent != null && model.EventTime >= NextEvent.EventTime)
                        {
                            return result = "Event Time cannot be after next event";
                        }
                        else if ((PreEvent != null && PreEvent.EventTypeID == SelectedEvent.EventTypeID) || (NextEvent != null && NextEvent.EventTypeID == SelectedEvent.EventTypeID))
                        {
                            return result = "Cannot enter an event twice in a row";
                        }
                        else if (model.EventTypeID == 3 && model.TruckEventID == null)
                        {
                            var gcs = _Igc.SelectToDispatch(CurrentTrip.TripID);
                            if (!gcs.Any())
                            {
                                return result = "No Gcs to Dispatch";
                            }
                        }
                        else if (PreEventType?.LimitPostEvent is not null & PreEventType.LimitPostEvent != model.EventTypeID /*& CurrentTrip.UserInfo.RecordStatus == 3*/)
                        {
                            return result = "Due to previous event this Event is restricted!";
                        }
                        else
                        {
                            if (SelectedEvent.EventTypeID == 4 && model.TruckEventID == null)
                            {
                                if (!SelectedGCs.Any())
                                {
                                    return result = "No GCs selected to Unload!";
                                }
                            }

                            model = Ievent.Update(model);

                            if (SelectedEvent.EventTypeID == 4)
                            {
                                foreach (GcSetModel item in SelectedGCs)
                                {
                                    _Igc.BeginUnload(CurrentTrip.TripID, item.GcSetID);
                                }
                            }
                        }
                        //MudDialog.Close(DialogResult.Ok(model));
                    }
                    catch (Exception ex)
                    {
                        result = ex.Message;
                    }
                    finally
                    {
                        //isSubmitting = false;
                        await Task.Delay(200);
                        //_busy = false;
                    }
                }
                else
                {
                    result = "EventDate cannot be a greater than today's date.";
                }
            }
            else
            {
                result = "Permission denied! You dont have any permission to Add or Edit Event.";
            }
            return result;
        }


        [HttpPost("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]

        public async Task<EventModel> OnTruckSelected(TruckModel model)
        {
            SelectedTruck = model;
            if (model != null)
            {
                CurrentTrip = Itrip.Select(Itruck.GetCurrentTrip(model.TruckID));
                if (CurrentTrip != null)
                {
                    CurrentTrip.Closed = Itrip.IsClosed(CurrentTrip.TripID);
                }
                _truckStatusUpdateService.truckStatusUpdated(Ievent.GetCurrentEvent(model.TruckID), SelectedTruck);
                //ShowReportBreakdownButton = BranchID == (model.CurrentBranchID ?? BranchID);
            }
            else
            {
                CurrentTrip = null;
                _truckStatusUpdateService.truckStatusUpdated(null, null);
            }
            return Ievent.GetCurrentEvent(model.TruckID);
        }

        
    }
}
