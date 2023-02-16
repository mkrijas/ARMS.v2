using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface ITruckService
    {       
        TruckModel Update(TruckModel model);
        int Delete(int? TruckID, string UserID);
        IEnumerable<TruckModel> Select(int? TruckID);
        IEnumerable<TruckModel> SelectByBranch(int? BranchID);
        TruckModel SelectByID(int? ID);
        TruckRegistrationModel GetRegistration(int? TruckID);
        TruckRegistrationModel GetRegistration(string RegNo);
        int Sold(int? TruckID, DateTime? SoldDate);
        int ChangeRegistration(TruckRegistrationModel model);        
        int UpdateDriver(int? TruckID, int? DriverID,bool AssignedStatus,string UserID);
        int? GetAssignedDriver(int? TruckID);
        long? GetCurrentTrip(int? TruckID);
    }

    public class TruckService : ITruckService
    {
        IDbService Iservice;
        public TruckService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public TruckModel Update(TruckModel model)
        {
            bool create = model.TruckID == null;
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", model.TruckID),
               new SqlParameter("@HomeBranchID", model.HomeBranchID),
               new SqlParameter("@BodyType", model.BodyType),
               new SqlParameter("@ChassisNumber", model.ChassisNumber),
               new SqlParameter("@EngineNumber", model.EngineNumber),
               new SqlParameter("@FuelTankCapacity", model.FuelTankCapacity),
               new SqlParameter("@FuelType", model.FuelType),
               new SqlParameter("@GpsDeviceID", model.GpsDeviceID),
               new SqlParameter("@ManufacturedYear", model.ManufacturedYear),
               new SqlParameter("@PurchaseDate", model.PurchaseDate),
               new SqlParameter("@TruckTypeID", model.TruckTypeID),
               new SqlParameter("@TransmissionType", model.TransmissionType),              
               new SqlParameter("@AssetID", model.AssetID),              
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            model.CurrentRegistration.UserInfo = model.UserInfo;

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Truck.Update]", parameters))
            {
                model = GetModel(dr);               
            }
            model.CurrentRegistration.TruckID = model.TruckID;
            if (create)
            {
                int reg = ChangeRegistration(model.CurrentRegistration);
                if (reg > 0)
                {
                    model.RegNo = model.CurrentRegistration.RegNo;
                }
            }
            return model;
        }
        public int Delete(int? TruckID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", TruckID),               
               new SqlParameter("@UserID", UserID),
            };            
            return Iservice.ExecuteNonQuery("[usp.Truck.Truck.Delete]", parameters);
        }
        public IEnumerable<TruckModel> Select(int? TruckID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", TruckID)               
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Truck.Select]", parameters))
            {
                yield return GetModel(dr);              
            }
        }

        public int Sold(int? TruckID, DateTime? SoldDate)
        {
            throw new NotImplementedException();
        }

        public int ChangeRegistration(TruckRegistrationModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {               
               new SqlParameter("@TruckID", model.TruckID),
               new SqlParameter("@RegNo", model.RegNo),
               new SqlParameter("@EffectFrom", model.EffectFrom),
               new SqlParameter("@EffectTo", model.EffectTo),              
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Truck.Registration.Update]", parameters);
        }

        private TruckModel GetModel(IDataRecord reader)
        {
            return new TruckModel
            {
                RegNo = reader.GetString("RegNo"),
                HomeBranchID = reader.GetInt32("HomeBranchID"),
                AssetID = reader.GetInt32("AssetID"),
                BodyType = reader.GetString("BodyType"),
                ChassisNumber = reader.GetString("ChassisNumber"),
                EngineNumber = reader.GetString("EngineNumber"),
                FuelTankCapacity = reader.GetDecimal("FuelTankCapacity"),
                FuelType = reader.GetString("FuelType"),
                TruckID = reader.GetInt32("TruckID"),
                GpsDeviceID = reader.GetInt64("GpsDeviceID"),
                ManufacturedYear = reader.GetInt16("ManufacturedYear"),
                PurchaseDate = reader.GetDateTime("PurchaseDate"),               
                SoldDate = reader.GetDateTime("SoldDate"),
                TransmissionType = reader.GetString("TransmissionType"),
                TruckTypeID = reader.GetInt16("TruckTypeID"),
                TruckType = reader.GetString("TruckType"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = reader.GetByte("RecordStatus"),
                    TimeStampField = reader.GetDateTime("TimeStamp"),
                    UserID = reader.GetString("UserID"),
                },
            };
        }

        private TruckRegistrationModel GetRegModel(IDataRecord reader)
        {
            return new TruckRegistrationModel
            {
                RegID = reader.GetInt32("RegID"),
                RegNo = reader.GetString("RegNo"),
                EffectFrom = reader.GetDateTime("EffectFrom"),
                EffectTo = reader.GetDateTime("EffectTo"),
                TruckID = reader.GetInt32("TruckID"),
                RC = reader.GetString("RC"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = reader.GetByte("RecordStatus"),
                    TimeStampField = reader.GetDateTime("TimeStamp"),
                    UserID = reader.GetString("UserID"),
                },
            };
        }

        public TruckModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", ID)
            };
            TruckModel model = new TruckModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Truck.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public TruckRegistrationModel GetRegistration(int? RegID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@String", RegID.ToString()),
               new SqlParameter("@Operation", "SelectByTruckID"),
            };
            TruckRegistrationModel model = new TruckRegistrationModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Registration.Select]", parameters))
            {
                model = GetRegModel(dr);
            }
            return model;
        }

        public TruckRegistrationModel GetRegistration(string RegNo)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@String", RegNo),
               new SqlParameter("@Operation", "SelectByRegNo"),
            };
            TruckRegistrationModel model = new TruckRegistrationModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Registration.Select]", parameters))
            {
                model = GetRegModel(dr);
            }
            return model; 
        }
        public int UpdateDriver(int? TruckID, int? DriverID, bool AssignedStatus,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", TruckID),
               new SqlParameter("@DriverID", DriverID),
               new SqlParameter("@AssignedStatus", AssignedStatus),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Truck.Driver.Assignment.Update]", parameters);
        }
        int? ITruckService.GetAssignedDriver(int? TruckID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", TruckID),               
            };
            int? DriverID = null;
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Driver.Assignment.Select]", parameters))
            {
                 if(dr.GetBoolean("AssignedStatus"))
                {
                    DriverID = dr.GetInt32("DriverID");
                }
            }
            return DriverID;
        }

        public long? GetCurrentTrip(int? TruckID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", TruckID),
            };
            long? TripID = null;
            foreach (var dr in Iservice.GetDataReader("[usp.Operation.Truck.CurrentTrip.Select]", parameters))
            {
                TripID = dr.GetInt64("TripID");
            }
            return TripID;
        }

        public IEnumerable<TruckModel> SelectByBranch(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Truck.Select]", parameters))
            {
                yield return new TruckModel()
                {
                    RegNo = dr.GetString("RegNo"),                   
                    TruckID = dr.GetInt32("TruckID"),                   
                    TruckTypeID = dr.GetInt16("TruckTypeID"),
                    TruckType = dr.GetString("TruckType"),
                    CurrentEvent = new EventModel()
                    {
                        BranchID = dr.GetInt32("BranchID"),
                        BranchName = dr.GetString("BranchName"),
                        DestinationID = dr.GetInt32("DestinationID"),
                        DriverID = dr.GetInt32("DriverID"),
                        EventReading = dr.GetInt64("EventReading"),
                        EventTime = dr.GetDateTime("EventTime"),
                        EventTypeID = dr.GetByte("EventTypeID"),
                        GcSetID = dr.GetInt64("GcSetID"),
                        OriginID = dr.GetInt32("OriginID"),
                        TripID = dr.GetInt64("TripID"),
                        TruckEventID = dr.GetInt64("EventID"),
                        TruckID = dr.GetInt32("TruckID"),
                        OriginName = dr.GetString("OriginName"),
                        DestinationName = dr.GetString("DestinationName")
                    },                    
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecordStatus"),
                        TimeStampField = dr.GetDateTime("TimeStamp"),
                        UserID = dr.GetString("UserID"),
                    },
                };
            }
        }
    }
}
