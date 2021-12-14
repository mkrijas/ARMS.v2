using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IGstService
    {       
        Task<GstModel> Update(GstModel model);
        Task<int> Delete(int? GstID, string UserID);
        IAsyncEnumerable<GstModel> Select(int? GstID);
        IAsyncEnumerable<GstModel> SelectByParty(int? PartyID = 0);
    }

    public class GstService : IGstService
    {
        IDbService Iservice;
        IAddressService _addressService;

        public GstService(IDbService iservice,IAddressService addressService)
        {
            Iservice = iservice;
            _addressService = addressService;
        }
        public async Task<GstModel> Update(GstModel model)
        {
            AddressModel addressModel = await _addressService.Update(model.Address);
            model.AddressID = addressModel.AddressID;
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GstID", model.GstID),
               new SqlParameter("@AddressID", model.AddressID),
               new SqlParameter("@Email", model.Email),
               new SqlParameter("@GstNo", model.GstNo),
               new SqlParameter("@PartyID", model.PartyID),
               new SqlParameter("@Phone", model.Phone),
               new SqlParameter("@RegName", model.RegName),
               new SqlParameter("@TanNo", model.TanNo),
               new SqlParameter("@TradeName", model.TradeName),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            
            await foreach (IDataRecord reader in Iservice.GetDataReaderAsync("[usp.Entity.GstUpdate]", parameters))
            {               
                    model = new GstModel
                    {
                        GstID = reader.GetInt32("GstID"),
                        AddressID = reader.GetInt32("AddressID"),
                        Email = reader.GetString("Email"),
                        GstNo = reader.GetString("GstNo"),
                        PartyID = reader.GetInt32("PartyID"),
                        Phone = reader.GetString("Phone"),
                        RegName = reader.GetString("RegName"),
                        TanNo = reader.GetString("TanNo"),
                        TradeName = reader.GetString("TradeName"),
                        Address = await _addressService.SelectByID(reader.GetInt32("AddressID").GetValueOrDefault()),
                        UserInfo = new ArmsModels.SharedModels.UserInfoModel
                        {
                            RecordStatus = reader.GetByte("RecordStatus"),
                            TimeStampField = reader.GetDateTime("TimeStamp"),
                            UserID = reader.GetString("UserID"),
                        },
                    };                
            }
            return model;
        }
        public async Task<int> Delete(int? GstID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GstID", GstID),               
               new SqlParameter("@UserID", UserID),
            };            
            return await Iservice.ExecuteNonQueryAsync("[usp.Entity.GstDelete]", parameters);
        }
        public async IAsyncEnumerable<GstModel> Select(int? GstID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", GstID),
               new SqlParameter("@Operation", "SelectByGst")
            };

            await foreach (IDataRecord reader in Iservice.GetDataReaderAsync("[usp.Entity.GstSelect]", parameters))
            {
                    yield return new GstModel
                    {
                        GstID = reader.GetInt32("GstID"),
                        AddressID = reader.GetInt32("AddressID"),
                        Email = reader.GetString("Email"),
                        GstNo = reader.GetString("GstNo"),
                        PartyID = reader.GetInt32("PartyID"),
                        Phone = reader.GetString("Phone"),
                        RegName = reader.GetString("RegName"),
                        TanNo = reader.GetString("TanNo"),
                        TradeName = reader.GetString("TradeName"),
                        Address = await _addressService.SelectByID(reader.GetInt32("AddressID").GetValueOrDefault()),
                        UserInfo = new ArmsModels.SharedModels.UserInfoModel
                        {
                            RecordStatus = reader.GetByte("RecordStatus"),
                            TimeStampField = reader.GetDateTime("TimeStamp"),
                            UserID = reader.GetString("UserID"),
                        },
                    };               
            }
        }

        public async IAsyncEnumerable<GstModel> SelectByParty(int? PartyID = 0)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", PartyID),
               new SqlParameter("@Operation", "SelectByParty"),
            };

            await foreach (IDataRecord reader in Iservice.GetDataReaderAsync("[usp.Entity.GstSelect]", parameters))
            {
                yield return new GstModel
                {
                    GstID = reader.GetInt32("GstID"),
                    AddressID = reader.GetInt32("AddressID"),
                    Email = reader.GetString("Email"),
                    GstNo = reader.GetString("GstNo"),
                    PartyID = reader.GetInt32("PartyID"),
                    Phone = reader.GetString("Phone"),
                    RegName = reader.GetString("RegName"),
                    TanNo = reader.GetString("TanNo"),
                    TradeName = reader.GetString("TradeName"),
                    Address = await _addressService.SelectByID(reader.GetInt32("AddressID").GetValueOrDefault()),
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
