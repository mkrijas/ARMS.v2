using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ArmsServices.DataServices
{
    public interface IDriverTransferService
    {
        DriverTransferInitiationModel UpdateOutgoing(DriverTransferInitiationModel model);
        IEnumerable<DriverTransferInitiationModel> SelectOutgoingDrivers(int? Branch);
        int DeleteInitiation(int? ID, int? BranchID, int? DriverID);
        DriverTransferInitiationModel UpdateStatus(DriverTransferInitiationModel model);
        IEnumerable<DriverTransferInitiationModel> SelectIncomingDrivers(int? Branch);
    }
}