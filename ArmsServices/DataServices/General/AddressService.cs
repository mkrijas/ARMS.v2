using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class AddressService : IAddressService
    {
        IDbService Iservice;
        public AddressService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to update an address entry
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
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.AddressesUpdate]", parameters))
            {
                model = new AddressModel
                {
                    AddressID = dr.GetInt32("AddressID"),
                    AddresseeName = dr.GetString("AddresseeName"),
                    Building = dr.GetString("Building"),
                    City = dr.GetString("City"),
                    PinCode = dr.GetString("PinCode"),
                    Place = dr.GetString("Place"),
                    Street = dr.GetString("Street"),
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecordStatus"),
                        TimeStampField = dr.GetDateTime("TimeStamp"),
                        UserID = dr.GetString("UserID"),
                    },
                };
            }
            return model;
        }

        // Method to select an address by its ID
        public AddressModel SelectByID(int? AddressID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@AddressID", AddressID),
            };
            AddressModel model = new AddressModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.AddressesSelect]", parameters))
            {
                model = new AddressModel
                {
                    AddressID = dr.GetInt32("AddressID"),
                    AddresseeName = dr.GetString("AddresseeName"),
                    Building = dr.GetString("Building"),
                    City = dr.GetString("City"),
                    PinCode = dr.GetString("PinCode"),
                    Place = dr.GetString("Place"),
                    Street = dr.GetString("Street"),
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecordStatus"),
                        TimeStampField = dr.GetDateTime("TimeStamp"),
                        UserID = dr.GetString("UserID"),
                    },
                };
            }
            return model;
        }

        // Method to delete an address by its ID
        public int Delete(int? AddressID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AddressID", AddressID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Entity.AddressesDelete]", parameters);
        }

        // Method to select addresses based on their ID
        public IEnumerable<AddressModel> Select(int? AddressID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AddressID", AddressID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.AddressesSelect]", parameters))
            {
                yield return new AddressModel
                {
                    AddressID = dr.GetInt32("AddressID"),
                    AddresseeName = dr.GetString("AddresseeName"),
                    Building = dr.GetString("Building"),
                    City = dr.GetString("City"),
                    PinCode = dr.GetString("PinCode"),
                    Place = dr.GetString("Place"),
                    Street = dr.GetString("Street"),
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
