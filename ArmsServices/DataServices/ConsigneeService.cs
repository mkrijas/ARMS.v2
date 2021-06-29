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
        Task<ConsigneeModel> SelectByID(int ID);
        Task<int> Delete(int ConsigneeID, string UserID);
        IAsyncEnumerable<ConsigneeModel> Select(int? ConsigneeID);
    }
    public class ConsigneeService : IConsigneeService
    {
        IDbService Iservice;

        public ConsigneeService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public async Task<int> Delete(int ConsigneeID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ConsigneeID", ConsigneeID),
               new SqlParameter("@UserID", UserID),
            };
            return await Iservice.ExecuteNonQuery("[usp.Gc.Consignee.Delete]", parameters);

        }
        public async IAsyncEnumerable<ConsigneeModel> Select(int? ConsigneeID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ConsigneeID", ConsigneeID)
            };

            await foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Gc.Consignee.Select]", parameters))
            {
                yield return await GetModel(dr);
            }

        }
        public async Task<ConsigneeModel> Update(ConsigneeModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ConsigneeID", model.ConsigneeID),
               new SqlParameter("@ConsigneeNo", model.ConsigneeName),
               new SqlParameter("@BillDate", model.Consignor),
               new SqlParameter("@BillNumber", model.Mobile),
               new SqlParameter("@BillQuantity", model.OrderID),
               new SqlParameter("@BranchID", model.PlaceID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            await foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Gc.Consignee.Update]", parameters))
            {
                model = await GetModel(dr);
            }
            return model;
        }

        public async Task<ConsigneeModel> SelectByID(int ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ConsigneeID", ID),              
            };
            ConsigneeModel model = new ConsigneeModel();
            await foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Gc.Consignee.Select]", parameters))
            {
                model = await GetModel(dr);
            }
            return model;
        }

        private async Task<ConsigneeModel> GetModel(IDataRecord dr)
        {
            return new ConsigneeModel
            {
                ConsigneeID = dr.GetInt32("ConsigneeID"),
                ConsigneeName = dr.SafeGetString("ConsigneeName"),
                Consignor = dr.GetBoolean("Consignor"),
                Mobile = dr.SafeGetString("Mobile"),
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
