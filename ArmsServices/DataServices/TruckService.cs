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
        int Delete(int TruckID, string UserID);
        IEnumerable<TruckModel> Select(int? TruckID);
        int Sold(int TruckID, DateTime SoldDate);
        int ChangeRegistration(TruckRegistrationModel model);
        int AssignDriver(int TruckID, int DriverID);
        int RemoveDriver(int TruckID, int DriverID);
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
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", model.TruckID),
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
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            TruckModel rmodel = new TruckModel();
            using (var reader = Iservice.GetDataReader("[usp.Truck.TrucksUpdate]", parameters))
            {
                while (reader.Read())
                {
                    rmodel = new TruckModel
                    {
                        BodyType = reader.GetString("BodyType"),
                        ChassisNumber = reader.GetString("ChassisNumber"),
                        EngineNumber = reader.GetString("EngineNumber"),
                        FuelTankCapacity = reader.GetDecimal("FuelTankCapacity"),
                        FuelType = reader.GetString("FuelType"),
                        TruckID = reader.GetInt32("TruckID"),
                        GpsDeviceID = reader.IsDBNull("GpsDeviceID") ?null: reader.GetInt64("GpsDeviceID"),
                        ManufacturedYear = reader.GetInt16("ManufacturedYear"),
                        PurchaseDate = reader.GetDateTime("PurchaseDate"),
                        RegNumber = reader.GetString("RegNumber"),
                        SoldDate = reader.GetDateTime("SoldDate"),
                        TransmissionType = reader.GetString("TransmissionType"),
                        TruckTypeID = reader.GetInt16("TruckTypeID"),
                        UserInfo = new ArmsModels.SharedModels.UserInfoModel
                        {
                            RecordStatus = reader.GetByte("RecordStatus"),
                            TimeStampField = reader.GetDateTime("TimeStamp"),
                            UserID = reader.GetString("UserID"),
                        },
                    };
                }
            }
            return rmodel;
        }
        public int Delete(int TruckID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", TruckID),               
               new SqlParameter("@UserID", UserID),
            };            
            return Iservice.ExecuteNonQuery("[usp.Truck.TrucksDelete]", parameters);
        }
        public IEnumerable<TruckModel> Select(int? TruckID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", TruckID)               
            };

            using (var reader = Iservice.GetDataReader("[usp.Truck.TrucksSelect]", parameters))
            {
                while (reader.Read())
                {
                    yield return new TruckModel
                    {
                        BodyType = reader.GetString("BodyType"),
                        ChassisNumber = reader.GetString("ChassisNumber"),
                        EngineNumber = reader.GetString("EngineNumber"),
                        FuelTankCapacity = reader.GetDecimal("FuelTankCapacity"),
                        FuelType = reader.GetString("FuelType"),
                        TruckID = reader.GetInt32("TruckID"),
                        GpsDeviceID = reader.IsDBNull("GpsDeviceID") ? null : reader.GetInt64("GpsDeviceID"),
                        ManufacturedYear = reader.GetInt16("ManufacturedYear"),
                        PurchaseDate = reader.GetDateTime("PurchaseDate"),
                        RegNumber = reader.GetString("RegNumber"),
                        SoldDate = reader.GetDateTime("SoldDate"),
                        TransmissionType = reader.GetString("TransmissionType"),
                        TruckTypeID = reader.GetInt16("TruckTypeID"),
                        UserInfo = new ArmsModels.SharedModels.UserInfoModel
                        {
                            RecordStatus = reader.GetByte("RecordStatus"),
                            TimeStampField = reader.GetDateTime("TimeStamp"),
                            UserID = reader.GetString("UserID"),
                        },
                    };
                }
            }
        }

        public int Sold(int TruckID, DateTime SoldDate)
        {
            throw new NotImplementedException();
        }

        public int ChangeRegistration(TruckRegistrationModel model)
        {
            throw new NotImplementedException();
        }

        public int AssignDriver(int TruckID, int DriverID)
        {
            throw new NotImplementedException();
        }

        public int RemoveDriver(int TruckID, int DriverID)
        {
            throw new NotImplementedException();
        }
    }
}
