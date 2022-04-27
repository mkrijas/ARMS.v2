using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{

    public interface IConsigneeService
    {
        Task<ConsigneeModel> Update(ConsigneeModel model);
        Task<ConsigneeModel> SelectByID(int? ID);
        Task<int> Delete(int? ConsigneeID, string UserID);
        IAsyncEnumerable<ConsigneeModel> Select(int? ConsigneeID);
        IAsyncEnumerable<ConsigneeModel> SelectByOrder(int? ID);
    }

    public class ConsigneeService : IConsigneeService
    {
        IDbService Iservice;
        IAddressService Iaddress;

        public ConsigneeService(IDbService iservice,IAddressService addressService)
        {
            Iservice = iservice;
            Iaddress = addressService;
        }
        public async Task<int> Delete(int? ConsigneeID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ConsigneeID", ConsigneeID),
               new SqlParameter("@UserID", UserID),
            };
            return await Iservice.ExecuteNonQueryAsync("[usp.Gc.Consignee.Delete]", parameters);

        }
        public async IAsyncEnumerable<ConsigneeModel> Select(int? ConsigneeID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ConsigneeID)
            };

            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Gc.Consignee.Select]", parameters))
            {
                yield return await GetModel(dr);
            }

        }
        public async Task<ConsigneeModel> Update(ConsigneeModel model)
        {
            model.Address.UserInfo = model.UserInfo;
            AddressModel address = await Iaddress.Update(model.Address);
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ConsigneeID", model.ConsigneeID),
               new SqlParameter("@ConsigneeName", model.ConsigneeName),
               new SqlParameter("@Consignor", model.Consignor),
               new SqlParameter("@Mobile", model.Mobile),
               new SqlParameter("@OrderID", model.OrderID),
               new SqlParameter("@PlaceID", model.PlaceID),
               new SqlParameter("@AddressID", address.AddressID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Gc.Consignee.Update]", parameters))
            {
                model = await GetModel(dr);
            }
            return model;
        }

        public async Task<ConsigneeModel> SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),              
            };
            ConsigneeModel model = new ConsigneeModel();
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Gc.Consignee.Select]", parameters))
            {
                model = await GetModel(dr);
            }
            return model;
        }

        public async IAsyncEnumerable<ConsigneeModel> SelectByOrder(int? ID)
        {                         
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@Operation", "ByOrder"),
            };

            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Gc.Consignee.Select]", parameters))
            {
                yield return await GetModel(dr);
            }

        }

        private async Task<ConsigneeModel> GetModel(IDataRecord dr)
        {
            return new ConsigneeModel
            {
                ConsigneeID = dr.GetInt32("ConsigneeID"),
                ConsigneeName = dr.GetString("ConsigneeName"),
                Consignor = dr.GetBoolean("Consignor"),
                Mobile = dr.GetString("Mobile"),
                Address = await Iaddress.SelectByID(dr.GetInt32("AddressID").GetValueOrDefault()),
                OrderID = dr.GetInt32("OrderID"),
                PlaceID = dr.GetInt32("PlaceID"),
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
