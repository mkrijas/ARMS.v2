using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IAddressService
    {       
        AddressModel Update(AddressModel model);
        int Delete(int AddressID, string UserID);
        IEnumerable<AddressModel> Select(int? AddressID);
    }

    public class AddressService : IAddressService
    {
        IDbService Iservice;

        public AddressService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public AddressModel Update(AddressModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@AddressID", model.AddressID),
               new SqlParameter("@AddresseeName", model.AddresseeName),
               new SqlParameter("@Building", model.Building),
               new SqlParameter("@City", model.City),
               new SqlParameter("@PinCode", model.PinCode),
               new SqlParameter("@Place", model.Place),
               new SqlParameter("@Street", model.Street),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            AddressModel rmodel = new AddressModel();
            using (var reader = Iservice.GetDataReader("[usp.Entity.AddresssUpdate]", parameters))
            {
                while (reader.Read())
                {
                    rmodel = new AddressModel
                    {
                        AddressID = reader.GetInt32("AddressID"),
                        AddresseeName = reader.GetString("AddresseeName"),
                        Building = reader.SafeGetString("Building"),
                        City = reader.SafeGetString("City"),
                        PinCode = reader.SafeGetString("PinCode"),
                        Place = reader.SafeGetString("Place"),
                        Street = reader.SafeGetString("Street"),                        
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

        public int Delete(int AddressID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AddressID", AddressID),               
               new SqlParameter("@UserID", UserID),
            };            
            return Iservice.ExecuteNonQuery("[usp.Entity.AddresssDelete]", parameters);
        }
        public IEnumerable<AddressModel> Select(int? AddressID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AddressID", AddressID)               
            };

            using (var reader = Iservice.GetDataReader("[usp.Entity.AddresssSelect]", parameters))
            {
                while (reader.Read())
                {
                    yield return new AddressModel
                    {
                        AddressID = reader.GetInt32("AddressID"),
                        AddresseeName = reader.GetString("AddresseeName"),
                        Building = reader.SafeGetString("Building"),
                        City = reader.SafeGetString("City"),
                        PinCode = reader.SafeGetString("PinCode"),
                        Place = reader.SafeGetString("Place"),
                        Street = reader.SafeGetString("Street"),
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
