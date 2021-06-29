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
        Task<int> Delete(int GcID, string UserID);
        IAsyncEnumerable<GcModel> Select(long? GcID);
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
               new SqlParameter("@PassNumber", model.PassNumber),
               new SqlParameter("@TripID", model.TripID),
               new SqlParameter("@UnloadedQuantity", model.UnloadedQuantity),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            
                await foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Gc.GcsUpdate]", parameters))
                {
                    model = new GcModel
                    {
                        GcID = dr.GetInt32("GcID"),
                        BillDate = dr.GetDateTime("BillDate"),
                        BillNumber = dr.SafeGetString("BillNumber"),
                        BillQuantity = dr.GetInt32("BillQuantity"),
                        BranchID = dr.GetInt32("BranchID"),
                        GcNo = dr.GetInt32("GcNo"),
                        GcDate = dr.GetDateTime("GcDate"),
                        GcPrefix = dr.SafeGetString("GcPrefix"),
                        GcType = dr.GetInt16("GcType"),
                        OrderID = dr.GetInt32("OrderID"),
                        PassNumber = dr.SafeGetString("PassNumber"),
                        TripID = dr.GetInt32("TripID"),
                        UnloadedQuantity = dr.GetInt32("UnloadedQuantity"),
                        ConsigneeID = dr.GetInt32("ConsigneeID"),
                        ConsignorID = dr.GetInt32("ConsignorID"),
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
        public async Task<int> Delete(int GcID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GcID", GcID),               
               new SqlParameter("@UserID", UserID),
            };            
            return await Iservice.ExecuteNonQuery("[usp.Gc.GcsDelete]", parameters);
        }
        public async IAsyncEnumerable<GcModel> Select(long? GcID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GcID", GcID)               
            };

            await foreach(IDataRecord dr in Iservice.GetDataReader("[usp.Gc.GcsSelect]", parameters))
            {               
                    yield return new GcModel
                    {
                        GcID = dr.GetInt32("GcID"),
                        BillDate = dr.GetDateTime("BillDate"),
                        BillNumber = dr.SafeGetString("BillNumber"),
                        BillQuantity = dr.GetInt32("BillQuantity"),
                        BranchID = dr.GetInt32("BranchID"),
                        GcNo = dr.GetInt32("GcNo"),
                        GcDate = dr.GetDateTime("GcDate"),
                        GcPrefix = dr.SafeGetString("GcPrefix"),
                        GcType = dr.GetInt16("GcType"),
                        OrderID = dr.GetInt32("OrderID"),
                        PassNumber = dr.SafeGetString("PassNumber"),
                        TripID = dr.GetInt32("TripID"),
                        UnloadedQuantity = dr.GetInt32("UnloadedQuantity"),
                        ConsigneeID = dr.GetInt32("ConsigneeID"),
                        ConsignorID = dr.GetInt32("ConsignorID"),
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
