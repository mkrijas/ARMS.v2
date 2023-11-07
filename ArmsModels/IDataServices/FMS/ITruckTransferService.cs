//using ArmsModels.BaseModels;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;

//namespace ArmsServices.DataServices
//{
//    public interface ITruckTransferService
//    {
//        TruckTransferInitiationModel UpdateOutgoing(TruckTransferInitiationModel model);
//        IEnumerable<TruckTransferInitiationModel> SelectOutgoingTrucks(int? Branch);
//        int DeleteInitiation(int? ID, long? EventID, string UserID);
//        TruckTransferInitiationModel UpdateStatus(TruckTransferInitiationModel model);
//        IEnumerable<TruckTransferInitiationModel> SelectIncomingTrucks(int? Branch);
//    }
//}