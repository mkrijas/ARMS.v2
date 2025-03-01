using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class OrderService : IOrderService
    {
        IDbService Iservice;

        public OrderService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to update an order
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
                new SqlParameter("@IsLimitedQuantity", model.IsLimitedQuantity),
                new SqlParameter("@UserID", model.UserInfo.UserID),
                new SqlParameter("@Declaration", model.Declaration)
            };
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.gc.Order.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to select an order by its IDc
        public async Task<OrderModel> SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@Operation", "Order"),
            };
            OrderModel model = new OrderModel();
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.gc.Order.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to delete an order by its ID
        public async Task<int> Delete(int? OrderID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@OrderID", OrderID),
               new SqlParameter("@UserID", UserID),
            };
            return await Iservice.ExecuteNonQueryAsync("[usp.Gc.Order.Delete]", parameters);
        }

        // Method to select orders based on a specific ID
        public async IAsyncEnumerable<OrderModel> Select(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@Operation", "Order"),
            };
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Gc.Order.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select orders by branch ID
        public async IAsyncEnumerable<OrderModel> SelectByBranch(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", BranchID),
               new SqlParameter("@Operation", "Branch"),
            };
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Gc.Order.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to update the order for a specific branch
        public async Task<int> BranchOrderUpdate(int? BranchID, int? OrderID, string UserID, string operation)
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

        // Helper method to map data record to OrderModel
        private OrderModel GetModel(IDataRecord dr)
        {
            return new OrderModel
            {
                ClientID = dr.GetInt32("ClientID"),
                ConsignorID = dr.GetInt32("ConsignorID"),
                ContentID = dr.GetInt16("ContentID"),
                OrderID = dr.GetInt32("OrderID"),
                GstNo = dr.GetString("GstNo"),
                OrderName = dr.GetString("OrderName"),
                IsLimitedQuantity = dr.GetBoolean("IsLimitedQuantity"),
                OrderQuantity = dr.GetDecimal("OrderQuantity"),
                Declaration = dr.GetString("Declaration"),
                Party = new PartyModel
                {
                    PartyID = dr.GetInt32("ClientID"),
                    TradeName = dr.GetString("TradeName"),
                    PartyCode = dr.GetString("PartyCode"),
                },
                Content = new ContentModel
                {
                    ContentID = dr.GetInt16("contentID"),
                    ContentName = dr.GetString("ContentName"),
                    PrimaryUnit = dr.GetString("PrimaryUnit")
                },
                Consignor = new()
                {
                    ConsigneeID = dr.GetInt32("consignorID"),
                    ConsigneeName = dr.GetString("consigneeName"),
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