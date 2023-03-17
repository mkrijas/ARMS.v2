using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ArmsServices.DataServices
{
    public interface ITruckTransferService
    {

        TruckTransferInitiationModel Update(TruckTransferInitiationModel model);
        IEnumerable<TruckTransferInitiationModel> SelectPendingByBranch(int? Branch);
        TruckTransferInitiationModel SelectDocumentRequest(int? ID);
        int Delete(int? ID, string UserID);
        IEnumerable<int?> getUnAssignrdTruckIds(int? BranchID);
    }

    public class TruckTransferService : ITruckTransferService
    {
        IDbService Iservice;

        public TruckTransferService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.FMS.Truck.Transfer.Initiation.Delete]", parameters);
        }


        public IEnumerable<TruckTransferInitiationModel> SelectPendingByBranch(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByBranch"),
               new SqlParameter("@ID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Truck.Transfer.Initiation.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public TruckTransferInitiationModel SelectDocumentRequest(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ID", ID),
               new SqlParameter("@Operation", "ByID")
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Truck.Transfer.Initiation.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }



        public TruckTransferInitiationModel Update(TruckTransferInitiationModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {

               new SqlParameter("@TruckTransferInitiationID", model.TruckTransferInitiationID),
               new SqlParameter("@TruckID", model.Truck.TruckID),
               new SqlParameter("@InitiationBranchID", model.InitiationBranchID),
               new SqlParameter("@DestinationBranchID", model.DestinationBranch.BranchID),
               new SqlParameter("@StartKM", model.StartKM),
               new SqlParameter("@Remarks", model.Remarks),
               new SqlParameter("@UserID", model.UserInfo.UserID),
               new SqlParameter("@RecordStatus", 3),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Truck.Transfer.Initiation.Update]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }



        public IEnumerable<int?> getUnAssignrdTruckIds(int? BranchID)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@BranchID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.Truck.UnAssigned.TruckIds]", parameters))
            {
                yield return dr.GetInt32("TruckID");
            }
            yield return null;
        }
        private TruckTransferInitiationModel GetModel(IDataRecord dr)
        {
            return new TruckTransferInitiationModel
            {
                TruckTransferInitiationID = dr.GetInt32("TruckTransferInitiationID"),
                InitiationBranchID = dr.GetInt32("InitiationBranchID"),
                DestinationBranchID = dr.GetInt32("DestinationBranchID"),
                DestinationBranch = new BranchModel()
                {
                    BranchID = dr.GetInt32("DestinationBranchID"),
                    BranchName = dr.GetString("DestinationBranchName"),
                },
                StartKM = dr.GetDecimal("StartKM"),
                Remarks = dr.GetString("Remarks"),
                Truck = new TruckModel()
                {
                    TruckID = dr.GetInt32("TruckID"),
                    RegNo = dr.GetString("RegNo"),
                },
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserId"),
                },
            };
        }

    }
}
