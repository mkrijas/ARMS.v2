using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IOrderService
    {
        Task<OrderModel> Update(OrderModel model);
        Task<OrderModel> SelectByID(int ID);
        IAsyncEnumerable<OrderModel> SelectByBranch(int BranchID);
        Task<int> Delete(int OrderID, string UserID);
        IAsyncEnumerable<OrderModel> Select(int? OrderID);
        Task<int> BranchOrderUpdate(int BranchID, int OrderID, string UserID, string operation);

    }

    public class OrderService : IOrderService
    {
        IDbService Iservice;
        IPartyService partyService;
        IConsigneeService consigneeService;
        IContentService contentService;
        public OrderService(IDbService iservice, IPartyService partyService,
            IConsigneeService consigneeService, IContentService contentService)
        {
            Iservice = iservice;
            this.partyService = partyService;
            this.consigneeService = consigneeService;
            this.contentService = contentService;
        }
        public async Task<OrderModel> Update(OrderModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@OrderID", model.OrderID),
               new SqlParameter("@OrderName", model.OrderName),
               new SqlParameter("@ClientID", model.ClientID),
               new SqlParameter("@ConsignorID", model.ConsignorID),
               new SqlParameter("@ContentID", model.ContentID),
               new SqlParameter("@OrderQuantity", model.OrderQuantity),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.gc.Order.Update]", parameters))
            {
                model = await GetModel(dr);
            }
            return model;
        }
        public async Task<OrderModel> SelectByID(int ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@Operation", "Order"),
            };
            OrderModel model = new OrderModel();
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.gc.Order.Update]", parameters))
            {
                model = await GetModel(dr);
            }
            return model;
        }
        public async Task<int> Delete(int OrderID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@OrderID", OrderID),
               new SqlParameter("@UserID", UserID),
            };
            return await Iservice.ExecuteNonQueryAsync("[usp.Gc.Order.Delete]", parameters);
        }
        public async IAsyncEnumerable<OrderModel> Select(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@Operation", "Order"),
            };
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Gc.Order.Select]", parameters))
            {
                yield return await GetModel(dr);
            }
        }
        public async IAsyncEnumerable<OrderModel> SelectByBranch(int BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", BranchID),
               new SqlParameter("@Operation", "Branch"),
            };
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Gc.Order.Select]", parameters))
            {
                yield return await GetModel(dr);
            }
        }

        public async Task<int> BranchOrderUpdate(int BranchID,int OrderID,string UserID,string operation)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@OrderID", OrderID),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Operation", operation),
            };
            return await Iservice.ExecuteNonQueryAsync("[usp.Gc.Order.Branch.Update]", parameters);
        }
        private async Task<OrderModel> GetModel(IDataRecord dr)
        {
            return new OrderModel
            {
                ClientID = dr.GetInt32("ClientID"),
                ConsignorID = dr.GetInt32("ConsignorID"),
                ContentID = dr.GetInt32("ContentID"),
                OrderID = dr.GetInt32("OrderID"),
                OrderName = dr.GetString("OrderName"),
                OrderQuantity = dr.GetDecimal("OrderQuantity"),
                Party = await partyService.SelectByID(dr.GetInt32("ClientID")),
                Content = await contentService.SelectByID(dr.GetInt32("contentID")),
                Consignor = await consigneeService.SelectByID(dr.GetInt32("consignorID")),

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
