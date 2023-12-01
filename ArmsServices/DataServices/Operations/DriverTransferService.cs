using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ArmsServices.DataServices
{
    public class DriverTransferService : IDriverTransferService
    {
        IDbService Iservice;

        public DriverTransferService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int DeleteInitiation(int? ID, int? BranchID, int? DriverID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@DriverID", DriverID),

            };
            return Iservice.ExecuteNonQuery("[usp.Driver.Transfer.Delete]", parameters);
        }


        public IEnumerable<DriverTransferInitiationModel> SelectOutgoingDrivers(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "Outgoing"),
               new SqlParameter("@ID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.Transfer.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }




        public DriverTransferInitiationModel UpdateOutgoing(DriverTransferInitiationModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {

               new SqlParameter("@DriverTransferID", model.DriverTransferID),
               new SqlParameter("@DriverID", model.Driver.DriverID),
               new SqlParameter("@InitiatedBranchID", model.InitiatedBranch?.BranchID??null),
               new SqlParameter("@DestinationBranchID", model.DestinationBranch.BranchID),
               new SqlParameter("@TransferInitiatedDate", model.TransferInitiatedDate),
               new SqlParameter("@Remarks", model.Remarks),
               new SqlParameter("@RecordStatus", 3),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.Transfer.Update]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }



        public IEnumerable<DriverTransferInitiationModel> SelectIncomingDrivers(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "Incoming"),
               new SqlParameter("@ID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.Transfer.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }




        public DriverTransferInitiationModel UpdateStatus(DriverTransferInitiationModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {

               new SqlParameter("@DriverTransferEndID", model.DriverTransferEndModel.DriverTransferEndID),
               new SqlParameter("@DriverTransferID", model.DriverTransferID),
               new SqlParameter("@DriverID", model.Driver.DriverID),
               new SqlParameter("@InitiatedBranchID", model.InitiatedBranch.BranchID),
               new SqlParameter("@BranchID", model.DestinationBranch.BranchID),
               new SqlParameter("@TransferEndDate", model.DriverTransferEndModel.TransferEndDate),
               new SqlParameter("@Remarks", model.DriverTransferEndModel.Remarks),
               new SqlParameter("@RecordStatus", 3),
               new SqlParameter("@Status", model.DriverTransferEndModel.TransferStatus),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.Transfer.Status.Update]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }


        private DriverTransferInitiationModel GetModel(IDataRecord dr)
        {
            return new DriverTransferInitiationModel()
            {
                DriverTransferID = dr.GetInt32("DriverTransferID"),
                InitiatedBranch = new BranchModel()
                {
                    BranchID = dr.GetInt32("InitiatedBranchID"),
                    BranchName = dr.GetString("InitiatedBranchName"),

                },
                DestinationBranch = new BranchModel()
                {
                    BranchID = dr.GetInt32("DestinationBranchID"),
                    BranchName = dr.GetString("DestinationBranchName"),
                },
                TransferInitiatedDate = dr.GetDateTime("TransferInitiatedDate"),
                DriverTransferEndModel = new DriverTransferEndModel()
                {

                    DriverTransferEndID = dr.GetInt32("DriverTransferEndID"),
                    TransferStatus = dr.GetBooleanNullable("TransferStatus"),

                    TransferEndDate = dr.GetDateTime("TransferEndDate"),

                },
                Remarks = dr.GetString("Remarks"),
                Driver = new DriverModel()
                {
                    DriverID = dr.GetInt32("DriverID"),
                    DriverName = dr.GetString("DriverName"),
                },
            };
        }


    }
}
