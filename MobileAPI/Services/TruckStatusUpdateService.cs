using ArmsModels.BaseModels;
using ArmsServices.DataServices;

namespace MobileAPI.Services
{
    public class TruckStatusUpdateService: ITruckStatusUpdateService
    {

        private readonly IEventService Ievent;
        private readonly IDriverService Idriver;
        private readonly ITruckService Itruck;
        private readonly IBranchService Ibranch;

        private EventTypeModel EventType { get; set; }
        private EventModel CurrentEvent { get; set; }
        string AssignDriverLabel;
        string TruckStatus;
        public DriverModel AssignedDriver { get; set; }

        public TruckStatusUpdateService(IEventService eventService, IDriverService driverService, ITruckService truckService, IBranchService branchService)
        {
            Ievent = eventService;
            Idriver = driverService;
            Itruck = truckService;
            Ibranch = branchService;
        }

        public void truckStatusUpdated(EventModel ev, TruckModel SelectedTruck)
        {
            // HideControls = true; HideStatus = true; 
            EventType = null;

            CurrentEvent = ev;

            if (SelectedTruck != null)
            {
                int? DriverID = Itruck.GetAssignedDriver(SelectedTruck.TruckID);
                AssignedDriver = DriverID == null ? null : Idriver.SelectByID(DriverID.Value);
                AssignDriverLabel = AssignedDriver == null ? "Assign Driver" : "Replace Driver";

                // Set text to show
                if (ev != null)
                {
                    //HideStatus = false;
                    EventType = Ievent.GetEventType(ev.EventTypeID);
                    //CurrentTrip = Itrip.Select(LastEvent.TripID);
                    string TruckStatusAppend;
                    if (!EventType.IsStationary)
                    {
                        TruckStatusAppend = " From " + ev.OriginName + " To " + ev.DestinationName;
                    }
                    else
                    {
                        TruckStatusAppend = " At " + ev.OriginName;
                    }
                    TruckStatus = string.Format("{0} On {1}{2} (Reported by {3}", EventType?.EventStatusText, ev?.EventTime, TruckStatusAppend, Ibranch.GetBranchName(ev.BranchID));
                }

                //Hide-Show Controls
                //if (EventType != null && EventType.IsBlocking)
                //{
                //    HideControls = true;
                //}
                //else if (EventType == null || EventType.EventStatusText == "UnAssigned")
                //{
                //    HideControls = false; HideStatus = true;
                //    EventButtonLabel = "Start New Trip";
                //}
                //else
                //{
                //    HideControls = false; HideStatus = false;
                //    EventButtonLabel = "Update Status";
                //}
            }
        }
    }
}
