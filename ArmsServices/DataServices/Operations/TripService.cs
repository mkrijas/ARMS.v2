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

        // Method to update a trip
        public TripModel Update(TripModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", model.TripID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DriverID", model.DriverID),
               new SqlParameter("@TripDate", model.TripDate),
               new SqlParameter("@TruckID", model.TruckID),
               new SqlParameter("@StartWithLoading", model.StartWithLoading),
               new SqlParameter("@EventTime", model.EventTime),
               new SqlParameter("@EventReading", model.EventReading),
               new SqlParameter("@OriginID", model.OriginID),
               new SqlParameter("@DestinationID", model.DestinationID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (var reader in Iservice.GetDataReader("[usp.Operation.Trip.Update]", parameters))
            {
                model = GetModel(reader);
            }
            return model;
        }

        // Method to delete a trip by its ID
        public int Delete(long? TripID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Operation.Trip.Delete]", parameters);
        }

        // Method to select a trip by its ID
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

        // Method to select all trips with optional filters
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

        // Helper method to map data record to TripModel
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
                IsMileageOverride = reader.GetBoolean("OverrideMileageShortage"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = reader.GetByte("RecordStatus"),
                    TimeStampField = reader.GetDateTime("TimeStamp"),
                    UserID = reader.GetString("UserID"),
                },
            };
        }

        // Method to cancel a trip  
        int ITripService.Cancel(long? TripID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripID),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Operation.Trip.Cancel]", parameters);
        }

        // Method to close a trip
        int ITripService.CloseTrip(long? TripID, int? BranchID, DateTime? EventTime, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripID),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@EventTime", EventTime),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Operation.Trip.Close]", parameters);
        }

        // Method to check if a trip is closed
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

        // Method to lock a trip
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

        // Method to override mileage shortage for a trip
        public int OverrideMileageShortage(long? TripID, bool Override, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Override", Override),
            };
            return Iservice.ExecuteNonQuery("[usp.Operation.Trip.OverrideMileageShortage]", parameters);
        }

        // Method to check if a trip is settled
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

        // Method to get outstanding bills for a trip (not implemented)c
        public IEnumerable<object> GetOutstandingBills(long? TripID)
        {
            throw new NotImplementedException();
        }

        // Method to select a trip by its trip number
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

        // Method to get the event list for a trip
        public IEnumerable<EventCardModel> GetEventList(long? TripID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripID),
               new SqlParameter("@Operation", "EventDetailsList"),
            };

            foreach (var reader in Iservice.GetDataReader("[usp.Operation.Trip.Select]", parameters))
            {
                yield return GetEventDetailsModel(reader);
            }
        }

        // Helper method to map data record to EventCardModel
        private EventCardModel GetEventDetailsModel(IDataRecord reader)
        {
            return new EventCardModel
            {
                EventID = reader?.GetInt64("EventID"),
                EventName = reader.GetString("EventTypeName"),
                NextEventName = reader?.GetString("NextEventTypeName"),
                PlaceName = reader.GetString("PlaceName"),
                EventDateTime = reader?.GetDateTime("EventDateTime"),
                EventDateTimeDiff = reader.GetString("EventDateTimeDiff"),
                KMReading = reader?.GetInt64("KMReading"),
                KMReadingDiff = reader?.GetInt64("KMReadingDiff"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = reader.GetByte("RecordStatus"),
                    TimeStampField = reader.GetDateTime("TimeStamp"),
                    UserID = reader.GetString("UserID"),
                },
            };
        }

        // Method to get trip information
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
                    RunDuration = reader.GetString("TimeDifference"),
                    TripID = reader.GetInt64("TripID"),
                    TripNumber = reader.GetString("TripNumber"),
                    Truck = reader.GetString("Truck"),
                    Gcs = reader.GetString("Gcs"),
                    Mileage = reader.GetDecimal("Mileage"),   //.HasValue ? Math.Round(reader.GetDecimal("Mileage") ?? 0) : null,
                    Expenses = reader.GetDecimal("Expenses"),
                    Freight = reader.GetDecimal("Freight"),
                    IsMileageOverride = reader.GetBoolean("OverrideMileageShortage"),
                    SettledKm = reader.GetDecimal("SettledKm"),
                };
            }
            return null;
        }

        // Method to search trips based on various filters
        public async IAsyncEnumerable<TripModel> SearchTrips(int? TruckID, int? BranchID, string TripNumberSearchString, DateTime? FromDate, DateTime? ToDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripNumber", TripNumberSearchString),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@TruckID", TruckID),
               new SqlParameter("@FromDate", FromDate),
               new SqlParameter("@ToDate", ToDate),
               new SqlParameter("@Operation", "SearchTrip"),
            };
            await foreach (var reader in Iservice.GetDataReaderAsync("[usp.Operation.Trip.Select]", parameters))
            {
                yield return GetModel(reader);
            }
        }

        // Method to get tariffs associated with a trip
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
