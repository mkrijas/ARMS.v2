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
        Task<AddressModel> Update(AddressModel model);
        Task<AddressModel> SelectByID(int AddressID);
        Task<int> Delete(int AddressID, string UserID);
        IAsyncEnumerable<AddressModel> Select(int? AddressID);

    }

    public class AddressService : IAddressService
    {
        IDbService Iservice;
        public AddressService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public async Task<AddressModel> Update(AddressModel model)
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
            await foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.AddressesUpdate]", parameters))
            {
                model = new AddressModel
                {
                    AddressID = dr.GetInt32("AddressID"),
                    AddresseeName = dr.GetString("AddresseeName"),
                    Building = dr.SafeGetString("Building"),
                    City = dr.SafeGetString("City"),
                    PinCode = dr.SafeGetString("PinCode"),
                    Place = dr.SafeGetString("Place"),
                    Street = dr.SafeGetString("Street"),
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
        public async Task<AddressModel> SelectByID(int AddressID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@AddressID", AddressID),               
            };
            AddressModel model = new AddressModel();
            await foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.AddressesSelect]", parameters))
            {
                model = new AddressModel
                {
                    AddressID = dr.GetInt32("AddressID"),
                    AddresseeName = dr.GetString("AddresseeName"),
                    Building = dr.SafeGetString("Building"),
                    City = dr.SafeGetString("City"),
                    PinCode = dr.SafeGetString("PinCode"),
                    Place = dr.SafeGetString("Place"),
                    Street = dr.SafeGetString("Street"),
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
        public async Task<int> Delete(int AddressID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AddressID", AddressID),               
               new SqlParameter("@UserID", UserID),
            };            
            return await Iservice.ExecuteNonQuery("[usp.Entity.AddressesDelete]", parameters);
        }
        public async IAsyncEnumerable<AddressModel> Select(int? AddressID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AddressID", AddressID)               
            };

            await foreach(IDataRecord dr in Iservice.GetDataReader("[usp.Entity.AddressesSelect]", parameters))
            {               
                    yield return new AddressModel
                    {
                        AddressID = dr.GetInt32("AddressID"),
                        AddresseeName = dr.GetString("AddresseeName"),
                        Building = dr.SafeGetString("Building"),
                        City = dr.SafeGetString("City"),
                        PinCode = dr.SafeGetString("PinCode"),
                        Place = dr.SafeGetString("Place"),
                        Street = dr.SafeGetString("Street"),
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
