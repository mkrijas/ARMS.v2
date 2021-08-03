using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface ITripService
    {       
        TripModel Update(TripModel model);
        int Delete(int TripID, string UserID);
        IEnumerable<TripModel> Select(int? TripID);
        int Cancel(int TripID, string userID, string Reason);
        int StartNew(TripModel model);
        int CloseTrip(int TripID, string Remarks);
    }

    public class TripService : ITripService
    {
        IDbService Iservice;

        public TripService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public TripModel Update(TripModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", model.TripID),               
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@TripDate", model.TripDate),
               new SqlParameter("@TruckID", model.TruckID),                       
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (var reader in Iservice.GetDataReader("[usp.Operation.Trip.Update]", parameters))
            {
                model = GetModel(reader);
            }
            return model;
        }
        public int Delete(int TripID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripID),               
               new SqlParameter("@UserID", UserID),
            };            
            return Iservice.ExecuteNonQuery("[usp.Operation.Trip.Delete]", parameters);
        }
        public IEnumerable<TripModel> Select(int? TripID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripID)               
            };

            foreach (var reader in Iservice.GetDataReader("[usp.Operation.Trip.Select]", parameters))
            {
                yield return GetModel(reader);
            }
        }

        private TripModel GetModel(IDataRecord reader)
        {
           return new TripModel
            {
                BranchID = reader.GetInt32("BranchID"),
                DriverID = reader.GetInt32("DriverID"),
                Fuel = reader.GetInt32("Fuel"),
                Mileage = reader.GetInt32("Mileage"),
                RunKM = reader.GetInt32("RunKM"),
                TripDate = reader.GetDateTime("TripDate"),
                TripID = reader.GetInt64("TripID"),
                TripPrefix = reader.GetString("TripPrefix"),
                TripNumber = reader.GetInt64("TripNumber"),
                TruckID = reader.GetInt32("TruckID"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = reader.GetByte("RecordStatus"),
                    TimeStampField = reader.GetDateTime("TimeStamp"),
                    UserID = reader.GetString("UserID"),
                },
            };
        }

        int ITripService.Cancel(int TripID, string userID, string Reason)
        {
            throw new NotImplementedException();
        }

        int ITripService.StartNew(TripModel model)
        {
            throw new NotImplementedException();
        }

        int ITripService.CloseTrip(int TripID, string Remarks)
        {
            throw new NotImplementedException();
        }
    }
}
