using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
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
        public int Delete(long? TripID, string UserID)
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
        public IEnumerable<TripModel> SelectAll(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@searchTerm", searchTerm),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
                new SqlParameter("@Operation", "SelectAll"),
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
                Fuel = reader.GetDecimal("Fuel"),
                Mileage = reader.GetDecimal("Mileage"),
                RunKM = reader.GetInt32("RunKM"),
                TripDate = reader.GetDateTime("TripDate"),
                TripID = reader.GetInt64("TripID"),
                TripPrefix = reader.GetString("TripPrefix"),
                TripNumber = reader.GetInt64("TripNumber"),
                TruckID = reader.GetInt32("TruckID"),
                IsLocked = reader.GetBoolean("LockedStatus"),
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

        int ITripService.CloseTrip(long? TripID, int? BranchID, string UserID)
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

        public int LockedTrip(long? TripID, bool LockedStatus, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripID),
               new SqlParameter("@LockedStatus", LockedStatus),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Operation.Trips.Locked.Update]", parameters);
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
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripID),
               new SqlParameter("@Operation", "GetTripInfo"),
            };
            foreach (var reader in Iservice.GetDataReader("[usp.Operation.Trip.Select]", parameters))
            {
                return new TripInfoModel()
                {
                    Driver = reader.GetString("Driver"),
                    Fuel = reader.GetDecimal("Fuel"),
                    RunKM = reader.GetInt32("RunKm"),
                    TripID = reader.GetInt64("TripID"),
                    //TripID = TripID,
                    TripNumber = (reader.GetInt64("TripNumber")).ToString(),
                    Truck = reader.GetString("Truck"),
                    Gcs = reader.GetString("Gcs"),
                    Mileage = reader.GetDecimal("Mileage"),   //.HasValue ? Math.Round(reader.GetDecimal("Mileage") ?? 0) : null,
                    Expenses = reader.GetDecimal("Expenses"),
                    Freight = reader.GetDecimal("Freight"),
                };
            }
            return null;
        }

        public async IAsyncEnumerable<TripModel> SearchTrips(int? TruckID, int? BranchID, string TripNumberSearchString)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripNumber", TripNumberSearchString),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@TruckID", TruckID),
               new SqlParameter("@Operation", "SearchTrip"),
            };
            await foreach (var reader in Iservice.GetDataReaderAsync("[usp.Operation.Trip.Select]", parameters))
            {
                yield return GetModel(reader);
            }
        }


        public IEnumerable<GcTariffModel> GetTariffs(long? TripID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripID),
               new SqlParameter("@Operation", "ByTrip"),
            };
            foreach (var dr in Iservice.GetDataReader("[usp.Gc.TariffEntry.Select]", parameters))
            {
                yield return new GcTariffModel()
                {
                    Amount = dr.GetDecimal("Amount"),
                    GcNumber = dr.GetString("GcNumber"),
                    TripNumber = dr.GetInt64("TripNumber"),
                    TariffTypeName = dr.GetString("TariffTypeName"),
                };
            }
        }
    }
}
