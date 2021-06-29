using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Data;
using System.Data.SqlClient;

namespace ArmsServices.DataServices
{
    public interface IDriverService
    {
        DriverModel Update(DriverModel model);
        int Delete(int DriverID, string UserID);
        IEnumerable<DriverModel> Select(int? PlaceID);
    }
    public class DriverService : IDriverService
    {
        IDbService Iservice;
        public DriverService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public DriverModel Update(DriverModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
    
               new SqlParameter("@DriverID", model.DriverID),
               new SqlParameter("@DriverName", model.DriverName),
               new SqlParameter("@DriverImage", model.DriverImage),
               new SqlParameter("@DateOfBirth", model.DateOfBirth),
               new SqlParameter("@AdhaarNo", model.AdhaarNo),
               new SqlParameter("@AdhaarImage", model.AdhaarImage),
               new SqlParameter("@Mobile", model.Mobile),
               new SqlParameter("@AlternateContactPerson", model.AlternateContactPerson),
               new SqlParameter("@AlternateContactMobile", model.AlternateContactMobile),
               new SqlParameter("@AddressID", model.AddressID),
               new SqlParameter("@DriverAgentID", model.DriverAgentID),
               new SqlParameter("@FestivalBonus", model.FestivalBonus),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            DriverModel driver = new DriverModel();
            using (var reader = Iservice.GetDataReader("[usp.Driver.DriversUpdate]", parameters))
            {
                while (reader.Read())
                {
                    driver = new DriverModel
                    {
                        DriverName = reader.SafeGetString("DriverName"),
                        DriverImage = reader.SafeGetString("DriverImage"),
                        DateOfBirth = reader.GetDateTime("DateOfBirth"),
                        AdhaarNo = reader.SafeGetString("AdhaarNo"),
                        Mobile = reader.SafeGetString("Mobile"),
                        AddressID = reader.GetInt32("AddressID"),
                        UserInfo = new ArmsModels.SharedModels.UserInfoModel
                        {
                            RecordStatus = reader.GetByte("RecordStatus"),
                            TimeStampField = reader.GetDateTime("TimeStamp"),
                            UserID = reader.GetString("UserID"),
                        },
                    };
                }
            }
            return driver;
              
        }
        public int Delete(int DriverID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DriverID", DriverID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Driver.DriversDelete]", parameters);
        }
        public IEnumerable<DriverModel> Select(int? DriverID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DriverID", DriverID)
            };

            using (var reader = Iservice.GetDataReader("[usp.Driver.DriversSelect]", parameters))
            {
                while (reader.Read())
                {
                    yield return new DriverModel
                    {
                        DriverName = reader.SafeGetString("DriverName"),
                        DriverImage = reader.SafeGetString("DriverImage"),
                        DateOfBirth = reader.GetDateTime("DateOfBirth"),
                        AdhaarNo = reader.SafeGetString("AdhaarNo"),
                        Mobile = reader.SafeGetString("Mobile"),
                        AddressID = reader.GetInt32("AddressID"),
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
    }
}
