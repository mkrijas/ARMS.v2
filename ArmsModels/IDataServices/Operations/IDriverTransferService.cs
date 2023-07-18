using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ArmsServices.DataServices
{
    public interface IDriverTransferService
    {
        DriverTransferInitiationModel UpdateOutgoing(DriverTransferInitiationModel model);  //Edit
        IEnumerable<DriverTransferInitiationModel> SelectOutgoingDrivers(int? Branch);
        int DeleteInitiation(int? ID, int? BranchID, int? DriverID);  //Delete
        DriverTransferInitiationModel UpdateStatus(DriverTransferInitiationModel model);  //Edit
        IEnumerable<DriverTransferInitiationModel> SelectIncomingDrivers(int? Branch);
    }
}