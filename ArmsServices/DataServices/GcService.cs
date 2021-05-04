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
        GcModel Update(GcModel model);
        int Delete(int GcID, string UserID);
        IEnumerable<GcModel> Select(int? GcID);
    }

    public class GcService : IGcService
    {
        IDbService Iservice;

        public GcService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public GcModel Update(GcModel model)
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

            GcModel rmodel = new GcModel();
            using (var reader = Iservice.GetDataReader("[usp.Gc.GcsUpdate]", parameters))
            {
                while (reader.Read())
                {
                    rmodel = new GcModel
                    {
                        GcID = reader.GetInt32("GcID"),
                        BillDate = reader.GetDateTime("BillDate"),
                        BillNumber = reader.SafeGetString("BillNumber"),
                        BillQuantity = reader.GetInt32("BillQuantity"),
                        BranchID = reader.GetInt32("BranchID"),
                        GcNo = reader.GetInt32("GcNo"),
                        GcDate = reader.GetDateTime("GcDate"),
                        GcPrefix = reader.SafeGetString("GcPrefix"),
                        GcType = reader.GetInt16("GcType"),
                        OrderID = reader.GetInt32("OrderID"),
                        PassNumber = reader.SafeGetString("PassNumber"),
                        TripID = reader.GetInt32("TripID"),
                        UnloadedQuantity = reader.GetInt32("UnloadedQuantity"),
                        ConsigneeID = reader.GetInt32("ConsigneeID"),
                        ConsignorID = reader.GetInt32("ConsignorID"),
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
        public int Delete(int GcID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GcID", GcID),               
               new SqlParameter("@UserID", UserID),
            };            
            return Iservice.ExecuteNonQuery("[usp.Gc.GcsDelete]", parameters);
        }
        public IEnumerable<GcModel> Select(int? GcID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GcID", GcID)               
            };

            using (var reader = Iservice.GetDataReader("[usp.Gc.GcsSelect]", parameters))
            {
                while (reader.Read())
                {
                    yield return new GcModel
                    {
                        GcID = reader.GetInt32("GcID"),
                        BillDate = reader.GetDateTime("BillDate"),
                        BillNumber = reader.SafeGetString("BillNumber"),
                        BillQuantity = reader.GetInt32("BillQuantity"),
                        BranchID = reader.GetInt32("BranchID"),
                        GcNo = reader.GetInt32("GcNo"),
                        GcDate = reader.GetDateTime("GcDate"),
                        GcPrefix = reader.SafeGetString("GcPrefix"),
                        GcType = reader.GetInt16("GcType"),
                        OrderID = reader.GetInt32("OrderID"),
                        PassNumber = reader.SafeGetString("PassNumber"),
                        TripID = reader.GetInt32("TripID"),
                        UnloadedQuantity = reader.GetInt32("UnloadedQuantity"),
                        ConsigneeID = reader.GetInt32("ConsigneeID"),
                        ConsignorID = reader.GetInt32("ConsignorID"),
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
