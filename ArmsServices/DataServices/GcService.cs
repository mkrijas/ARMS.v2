using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IGcService
    {       
        Task<GcModel> Update(GcModel model);
        Task<int> Delete(long GcID, string UserID);
        IAsyncEnumerable<GcModel> Select(long? GcID);
        Task<GcModel> SelectByID(long GcID);
        IAsyncEnumerable<GcTypeModel> SelectGcTypes();
    }

    public class GcService : IGcService
    {
        IDbService Iservice;

        public GcService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public async Task<GcModel> Update(GcModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@GcID", model.GcID),
               new SqlParameter("@GcNo", model.GcNo),
               new SqlParameter("@BillDate", model.BillDate),
               new SqlParameter("@BillNumber", model.BillNumber),
               new SqlParameter("@BillQuantity", model.BillQuantity),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@ConsigneeID", model.ConsigneeID),
               new SqlParameter("@ConsignorID", model.ConsignorID),
               new SqlParameter("@GcDate", model.GcDate),
               new SqlParameter("@GcPrefix", model.GcPrefix),
               new SqlParameter("@GcType", model.GcType),
               new SqlParameter("@OrderID", model.OrderID),
               new SqlParameter("@RouteID", model.RouteID),
               new SqlParameter("@PassNumber", model.PassNumber),               
               new SqlParameter("@UnloadedQuantity", model.UnloadedQuantity),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Gc.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public async Task<int> Delete(long GcID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GcID", GcID),               
               new SqlParameter("@UserID", UserID),
            };            
            return await Iservice.ExecuteNonQueryAsync("[usp.Gc.Delete]", parameters);
        }
        public async IAsyncEnumerable<GcModel> Select(long? GcID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GcID", GcID)               
            };

            await foreach(IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Gc.Select]", parameters))
            {
                yield return GetModel(dr);      
            }
        }

        public async IAsyncEnumerable<GcTypeModel> SelectGcTypes()
        {
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Gc.GcType.Select]", null))
            {
                yield return new GcTypeModel
                {
                    GcTypeID = dr.GetInt16("GcTypeID"),
                    GcTypeName = dr.GetString("GcTypeName")
                };
            }
        }
        public async Task<GcModel> SelectByID(long GcID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GcID", GcID)
            };
            GcModel model = new GcModel();
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Gc.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
        private  GcModel GetModel(IDataRecord dr)
        {
            return new GcModel
            {
                GcID = dr.GetInt64("GcID"),
                GcSetID = dr.GetInt64("GcSetID"),
                BillDate = dr.GetDateTime("BillDate"),
                BillNumber = dr.GetString("BillNumber"),
                BillQuantity = dr.GetDecimal("BillQuantity"),
                BranchID = dr.GetInt32("BranchID"),
                GcNo = dr.GetInt32("GcNo"),
                GcDate = dr.GetDateTime("GcDate"),
                GcPrefix = dr.GetString("GcPrefix"),
                GcType = dr.GetInt16("GcType"),
                GcTypeName = dr.GetString("GcTypeName"),
                OrderID = dr.GetInt32("OrderID"),
                OrderName = dr.GetString("OrderName"),
                RouteID = dr.GetInt32("RouteID"),
                RouteName = dr.GetString("RouteName"),
                PassNumber = dr.GetString("PassNumber"),
                TripID = dr.GetInt64("TripID"),
                UnloadedQuantity = dr.GetDecimal("UnloadedQuantity"),
                ConsigneeID = dr.GetInt32("ConsigneeID"),
                ConsigneeName = dr.GetString("ConsigneeName"),
                ConsignorID = dr.GetInt32("ConsignorID"),
                ConsignorName = dr.GetString("ConsignorName"),
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
