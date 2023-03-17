using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ArmsServices.DataServices
{
    public interface ITruckTransferService
    {

        TruckTransferInitiationModel UpdateInitiation(TruckTransferInitiationModel model);
        IEnumerable<TruckTransferInitiationModel> SelectPendingByBranchInitiation(int? Branch);
        int DeleteInitiation(int? ID, string UserID);
        IEnumerable<int?> getUnAssignrdTruckIds(int? BranchID);


        TruckTransferEndModel UpdateEnd(TruckTransferEndModel model);
        IEnumerable<TruckTransferEndModel> SelectPendingByBranchEnd(int? Branch);
        int DeleteEnd(int? ID, string UserID);
    }

    public class TruckTransferService : ITruckTransferService
    {
        IDbService Iservice;

        public TruckTransferService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int DeleteInitiation(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.FMS.Truck.Transfer.Initiation.Delete]", parameters);
        }


        public IEnumerable<TruckTransferInitiationModel> SelectPendingByBranchInitiation(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByBranch"),
               new SqlParameter("@ID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Truck.Transfer.Initiation.Select]", parameters))
            {
                yield return GetModelInitiation(dr);
            }
        }




        public TruckTransferInitiationModel UpdateInitiation(TruckTransferInitiationModel model)
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
                return GetModelInitiation(dr);
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



        public int DeleteEnd(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.FMS.Truck.Transfer.End.Delete]", parameters);
        }


        public IEnumerable<TruckTransferEndModel> SelectPendingByBranchEnd(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByBranch"),
               new SqlParameter("@ID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Truck.Transfer.End.Select]", parameters))
            {
                yield return GetModelEnd(dr);
            }
        }




        public TruckTransferEndModel UpdateEnd(TruckTransferEndModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {

               new SqlParameter("@TruckTransferEndID", model.TruckTransferEndID),
               new SqlParameter("@TruckTransferInitiationID", model.TruckTransferInitiationID),
               new SqlParameter("@TruckID", model.Truck.TruckID),
               new SqlParameter("@DestinationBranchID", model.DestinationBranchID),
               new SqlParameter("@EndKM", model.EndKM),
               new SqlParameter("@Remarks", model.Remarks),
               new SqlParameter("@UserID", model.UserInfo.UserID),
               new SqlParameter("@RecordStatus", 3),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Truck.Transfer.End.Update]", parameters))
            {
                return GetModelEnd(dr);
            }
            return null;
        }


        private TruckTransferInitiationModel GetModelInitiation(IDataRecord dr)
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


        private TruckTransferEndModel GetModelEnd(IDataRecord dr)
        {
            return new TruckTransferEndModel
            {
                TruckTransferEndID = dr.GetInt32("TruckTransferEndID"),
                TruckTransferInitiationID = dr.GetInt32("TruckTransferInitiationID"),
                InitiationBranchID = dr.GetInt32("InitiationBranchID"),
                DestinationBranchID = dr.GetInt32("DestinationBranchID"),
                TransferStatus = dr.GetString("TransferStatus"),
                InitiationBranch = new BranchModel()
                {
                    BranchID = dr.GetInt32("InitiationBranchID"),
                    BranchName = dr.GetString("InitiationBranchName"),
                },
                EndKM = dr.GetDecimal("EndKM"),
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
