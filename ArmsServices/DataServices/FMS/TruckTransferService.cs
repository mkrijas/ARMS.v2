using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ArmsServices.DataServices
{
    public interface ITruckTransferService
    {

        TruckTransferInitiationModel UpdateOutgoing(TruckTransferInitiationModel model);
        IEnumerable<TruckTransferInitiationModel> SelectOutgoingTrucks(int? Branch);
        int DeleteInitiation(int? ID, long? EventID, string UserID);

        TruckTransferInitiationModel UpdateStatus(TruckTransferInitiationModel model);
        IEnumerable<TruckTransferInitiationModel> SelectIncomingTrucks(int? Branch);
    }

    public class TruckTransferService : ITruckTransferService
    {
        IDbService Iservice;

        public TruckTransferService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int DeleteInitiation(int? ID, long? EventID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@EventID", EventID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.FMS.Truck.Transfer.Delete]", parameters);
        }


        public IEnumerable<TruckTransferInitiationModel> SelectOutgoingTrucks(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "Outgoing"),
               new SqlParameter("@ID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Truck.Transfer.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }




        public TruckTransferInitiationModel UpdateOutgoing(TruckTransferInitiationModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {

               new SqlParameter("@TruckTransferInitiationID", model.TruckTransferInitiationID),
               new SqlParameter("@TruckID", model.Truck.TruckID),
               new SqlParameter("@InitiationBranchID", model.InitiationBranch.BranchID),
               new SqlParameter("@DestinationBranchID", model.DestinationBranch.BranchID),
               new SqlParameter("@EventReading", model.TruckEvent.EventReading),
               new SqlParameter("@Remarks", model.Remarks),
               new SqlParameter("@UserID", model.TruckEvent.UserInfo.UserID),
               new SqlParameter("@EventID", model.TruckEvent.TruckEventID),
               new SqlParameter("@RecordStatus", 3),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Truck.Transfer.Update]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }



        public IEnumerable<TruckTransferInitiationModel> SelectIncomingTrucks(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByIncoming"),
               new SqlParameter("@ID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Truck.Transfer.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }




        public TruckTransferInitiationModel UpdateStatus(TruckTransferInitiationModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {

               new SqlParameter("@TruckTransferEndID", model.TruckTransferEndModel.TruckTransferEndID),
               new SqlParameter("@TruckTransferInitiationID", model.TruckTransferInitiationID),
               new SqlParameter("@TruckID", model.Truck.TruckID),
               new SqlParameter("@InitiationBranchID", model.InitiationBranch.BranchID),
               new SqlParameter("@BranchID", model.DestinationBranch.BranchID),
               new SqlParameter("@EventReading", model.TruckTransferEndModel.TruckEvent.EventReading),
               new SqlParameter("@Remarks", model.TruckTransferEndModel.Remarks),
               new SqlParameter("@UserID", model.TruckTransferEndModel.TruckEvent.UserInfo.UserID),
               new SqlParameter("@RecordStatus", 3),
               new SqlParameter("@Status", model.TruckTransferEndModel.TransferStatus),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Truck.Transfer.Status.Update]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }


        private TruckTransferInitiationModel GetModel(IDataRecord dr)
        {
            return new TruckTransferInitiationModel
            {
                TruckTransferInitiationID = dr.GetInt32("TruckTransferInitiationID"),
                InitiationBranch = new BranchModel()
                {
                    BranchID = dr.GetInt32("InitiationBranchID"),
                    BranchName = dr.GetString("InitiationBranchName"),

                },
                DestinationBranch = new BranchModel()
                {
                    BranchID = dr.GetInt32("DestinationBranchID"),
                    BranchName = dr.GetString("DestinationBranchName"),
                },
                TruckEvent = new EventModel()
                {
                    TruckEventID = dr.GetInt64("EventID"),
                     
                    EventReading = dr.GetInt64("EventReading"),
                    EventTime = dr.GetDateTime("EventTime"),
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecordStatus"),
                        TimeStampField = dr.GetDateTime("TimeStamp"),
                        UserID = dr.GetString("UserId"),
                    },
                },
                TruckTransferEndModel = new TruckTransferEndModel()
                {

                    TruckTransferEndID = dr.GetInt32("TruckTransferEndID"),
                    TransferStatus = dr.GetBooleanNullable("TransferStatus"),
                   

                    TruckEvent = new EventModel()
                    {
                        TruckEventID = dr.GetInt64("EventEndID"),
                        EventReading = dr.GetInt64("EndKM"),
                        EventTime = dr.GetDateTime("EventTime"),
                        BranchName = dr.GetString("OrginBranchName"),
                        BranchID = dr.GetInt32("OrginBranchID"),
                        UserInfo = new ArmsModels.SharedModels.UserInfoModel
                        {
                            RecordStatus = dr.GetByte("RecordStatus"),
                            TimeStampField = dr.GetDateTime("TimeStamp"),
                            UserID = dr.GetString("UserId"),
                        },
                    },
                },
                Remarks = dr.GetString("Remarks"),
                Truck = new TruckModel()
                {
                    TruckID = dr.GetInt32("TruckID"),
                    RegNo = dr.GetString("RegNo"),
                },
            };
        }


    }
}
