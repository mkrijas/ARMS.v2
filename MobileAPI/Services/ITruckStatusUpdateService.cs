using ArmsModels.BaseModels;

namespace MobileAPI.Services
{
    public interface ITruckStatusUpdateService
    {
        void truckStatusUpdated(EventModel ev, TruckModel SelectedTruck);
    }
}
