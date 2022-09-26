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
        int Delete(long? TripID, string UserID);
        TripModel Select(long? TripID);
        TripModel SelectByTripNumber(string TripNumber);
        int Cancel(long? TripID, string UserID);
        int CloseTrip(long? TripID, int? BranchID,string UserID);
        bool IsClosed(long? TripID);
        bool IsSettled(long? TripID);
        TripInfoModel GetTripInfo(long? TripID);
        IEnumerable<object> GetOutstandingBills(long? TripID);
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
               new SqlParameter("@DriverID", model.DriverID),
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
        public int Delete(long? TripID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripID),               
               new SqlParameter("@UserID", UserID),
            };            
            return Iservice.ExecuteNonQuery("[usp.Operation.Trip.Delete]", parameters);
        }
        public TripModel Select(long? TripID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripID),
                new SqlParameter("@Operation", "SelectByTripID"),
            };

            TripModel model = null;
            foreach (var reader in Iservice.GetDataReader("[usp.Operation.Trip.Select]", parameters))
            {
                model = GetModel(reader);
            }
            return model;
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

        int ITripService.Cancel(long? TripID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripID),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Operation.Trip.Cancel]", parameters);
        }

        int ITripService.CloseTrip(long? TripID,int? BranchID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripID),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Operation.Trip.Close]", parameters);
        }

        public bool IsClosed(long? TripID)
        {
            
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripID),
               new SqlParameter("@Query", "IsClosed"),
            };

            SqlParameter result = new SqlParameter("@result", SqlDbType.Bit);
            result.Direction = ParameterDirection.Output;
            parameters.Add(result);
             Iservice.ExecuteNonQuery("[usp.Operation.Trip.Query]", parameters);
            return (bool)result.Value;
        }

        public bool IsSettled(long? TripID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripID),
               new SqlParameter("@Query", "IsSettled"),
            };

            SqlParameter result = new SqlParameter("@result", SqlDbType.Bit);
            result.Direction = ParameterDirection.Output;
            Iservice.ExecuteNonQuery("[usp.Operation.Trip.Query]", parameters);
            return (bool)result.Value;
        }

        public IEnumerable<object> GetOutstandingBills(long? TripID)
        {
            throw new NotImplementedException();
        }

        public TripModel SelectByTripNumber(string TripNumber)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripNumber", TripNumber),
               new SqlParameter("@Operation", "SelectByTripNumber"),
            };

            TripModel model = null;
            foreach (var reader in Iservice.GetDataReader("[usp.Operation.Trip.Select]", parameters))
            {
                model = GetModel(reader);
            }
            return model;
        }

        public TripInfoModel GetTripInfo(long? TripID)
        {
            throw new NotImplementedException();
        }
    }
}
