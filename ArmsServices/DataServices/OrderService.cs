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
        OrderModel Update(OrderModel model);
        int Delete(int OrderID, string UserID);
        IEnumerable<OrderModel> Select(int? OrderID);
    }

    public class OrderService : IOrderService
    {
        IDbService Iservice;

        public OrderService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public OrderModel Update(OrderModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@OrderID", model.OrderID),
               new SqlParameter("@OrderName", model.OrderName),
               new SqlParameter("@ClientID", model.ClientID),
               new SqlParameter("@ConsigneeID", model.ConsigneeID),
               new SqlParameter("@ConsignorID", model.ConsignorID),
               new SqlParameter("@ContentID", model.ContentID),
               new SqlParameter("@RouteID", model.RouteID),
               new SqlParameter("@OrderQuantity", model.OrderQuantity),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            OrderModel rmodel = new OrderModel();
            using (var reader = Iservice.GetDataReader("[usp.Gc.OrdersUpdate]", parameters))
            {
                while (reader.Read())
                {
                    rmodel = new OrderModel
                    {
                        ClientID = reader.GetInt32("ClientID"),
                        ConsigneeID = reader.GetInt32("ConsigneeID"),
                        ConsignorID = reader.GetInt32("ConsignorID"),
                        ContentID = reader.GetInt32("ContentID"),
                        RouteID = reader.GetInt32("RouteID"),
                        OrderID = reader.GetInt32("OrderID"),
                        OrderName = reader.GetString("OrderName"),  
                        BranchID = reader.GetInt32("BranchID"),
                        OrderQuantity = reader.GetDecimal("OrderQuantity"),
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
        public int Delete(int OrderID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@OrderID", OrderID),               
               new SqlParameter("@UserID", UserID),
            };            
            return Iservice.ExecuteNonQuery("[usp.Gc.OrdersDelete]", parameters);
        }
        public IEnumerable<OrderModel> Select(int? OrderID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@OrderID", OrderID)               
            };

            using (var reader = Iservice.GetDataReader("[usp.Gc.OrdersSelect]", parameters))
            {
                while (reader.Read())
                {
                    yield return new OrderModel
                    {
                        ClientID = reader.GetInt32("ClientID"),
                        ConsigneeID = reader.GetInt32("ConsigneeID"),
                        ConsignorID = reader.GetInt32("ConsignorID"),
                        ContentID = reader.GetInt32("ContentID"),
                        RouteID = reader.GetInt32("RouteID"),
                        OrderID = reader.GetInt32("OrderID"),
                        OrderName = reader.GetString("OrderName"),
                        BranchID = reader.GetInt32("BranchID"),
                        OrderQuantity = reader.GetDecimal("OrderQuantity"),
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
