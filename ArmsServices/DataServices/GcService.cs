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
        GcSetModel Update(GcSetModel model);
        int Delete(long GcID, string UserID);
        List<GcSetModel> Select(int BranchID);
        List<GcSetModel> SelectByTrip(long TripID);
        List<GcSetModel> SelectUnAssigned(int BranchID);
        GcSetModel SelectByID(long GcSetID);
        IEnumerable<GcTypeModel> SelectGcTypes();
        int AppendToTrip(long TripID, long GcSetID, string UserID);
        int RemoveFromTrip(long GcSetID, int TripID, string UserID);
        int UpdateEwayBill(EwayBillModel model);
    }

    public class GcService : IGcService
    {
        IDbService Iservice;

        public GcService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public GcSetModel Update(GcSetModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GcSetID", model.GcSetID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@ConsigneeID", model.ConsigneeID),
               new SqlParameter("@ConsignorID", model.ConsignorID),
               new SqlParameter("@GcDate", model.GcDate??DateTime.Today),
               new SqlParameter("@OrderID", model.OrderID),
               new SqlParameter("@RouteID", model.RouteID),
            };
            GcSetModel InsertedModel = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.GcSet.Update]", parameters))
            {
                InsertedModel = GetModel(dr);
            }
            foreach (GcModel bill in model.Gcs)
            {
                parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@GcID", bill.GcID),
                new SqlParameter("@BranchID", InsertedModel.BranchID),
                new SqlParameter("@GcDate", InsertedModel.GcDate),
                new SqlParameter("@GcSetID", InsertedModel.GcSetID),
                new SqlParameter("@GcType", bill.GcType),
                new SqlParameter("@BillDate", bill.BillDate),
                new SqlParameter("@BillNumber", bill.BillNumber),
                new SqlParameter("@BillQuantity", bill.BillQuantity),
                new SqlParameter("@PassNumber", bill.PassNumber),
                new SqlParameter("@UnloadedQuantity", bill.UnloadedQuantity),               
                new SqlParameter("@UserID", bill.UserInfo.UserID),
                };
                foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Gc.Gcs.Update]", parameters))
                {
                    InsertedModel.Gcs.Add(GetGcModel(dr));
                }
            }
            return InsertedModel;
        }

        public int Delete(long GcID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GcID", GcID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Gc.Gcs.Delete]", parameters);
        }

        public List<GcSetModel> Select(int BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "All"),
               new SqlParameter("@BranchID", BranchID)
            };
            return GetList(parameters);
        }
        public List<GcSetModel> SelectByTrip(long TripID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByTrip"),
               new SqlParameter("@TripID", TripID)
            };
            return GetList(parameters);
        }

        public List<GcSetModel> SelectUnAssigned(int BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "UnAssigned"),
               new SqlParameter("@BranchID", BranchID)
            };
            return GetList(parameters);
        }

        public IEnumerable<GcTypeModel> SelectGcTypes()
        {
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Gc.GcType.Select]", null))
            {
                yield return new GcTypeModel
                {
                    GcTypeID = dr.GetInt16("GcTypeID"),
                    GcTypeName = dr.GetString("GcTypeName")
                };
            }
        }
        public GcSetModel SelectByID(long GcSetID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GcSetID", GcSetID),
               new SqlParameter("@Operation", "ByID"),
            };
            var list = GetList(parameters);
            return list.FirstOrDefault();
        }

        private List<GcSetModel> GetList(List<SqlParameter> parameters)
        {
            List<GcSetModel> list = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.GcSet.Select]", parameters))
            {
                GcModel gc = GetGcModel(dr);
                GcSetModel set = new();

                if (list.Exists(x => x.GcSetID == gc.GcSetID))
                {
                    set = list.Find(x => x.GcSetID == gc.GcSetID);
                }
                else
                {
                    set = GetModel(dr);
                    list.Add(set);
                }
                bool IsFirst = set.Gcs.Count == 0;

                set.SetBillNumber = set.SetBillNumber + (IsFirst ? null : ", ") + gc.BillNumber;
                set.SetGcNumber = set.SetGcNumber + (IsFirst ? gc.GcPrefix : ", ") + gc.GcNumber;
                set.SetBillQuantity = set.SetBillQuantity + gc.BillQuantity;
                set.SetUnloadQuantity = set.SetUnloadQuantity + gc.UnloadedQuantity;
                set.Gcs.Add(gc);
            }

            return list;
        }

        private GcSetModel GetModel(IDataRecord dr)
        {
            return new GcSetModel
            {
                GcSetID = dr.GetInt64("GcSetID"),
                BranchID = dr.GetInt32("BranchID"),
                GcDate = dr.GetDateTime("GcDate"),
                OrderID = dr.GetInt32("OrderID"),
                OrderName = dr.HasColumn("OrderName") ? dr.GetString("OrderName") : null,
                RouteID = dr.GetInt32("RouteID"),
                RouteName = dr.HasColumn("RouteName") ? dr.GetString("RouteName") : null,
                TripID = dr.GetInt64("TripID"),
                ConsigneeID = dr.GetInt32("ConsigneeID"),
                ConsigneeName = dr.HasColumn("ConsigneeName") ? dr.GetString("ConsigneeName") : null,
                ConsignorID = dr.GetInt32("ConsignorID"),
                ConsignorName = dr.HasColumn("ConsignorName") ? dr.GetString("ConsignorName") : null,
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        private GcModel GetGcModel(IDataRecord dr)
        {
            return new GcModel
            {
                GcID = dr.GetInt64("GcID"),
                GcSetID = dr.GetInt64("GcSetID"),
                BillDate = dr.GetDateTime("BillDate"),
                BillNumber = dr.GetString("BillNumber"),
                BillQuantity = dr.GetDecimal("BillQuantity"),
                GcNumber = dr.GetInt32("GcNo"),
                GcPrefix = dr.GetString("GcPrefix"),
                GcType = dr.GetInt16("GcType"),
                GcTypeName = dr.HasColumn("GcTypeName") ? dr.GetString("GcTypeName") : null,
                PassNumber = dr.GetString("PassNumber"),
                UnloadedQuantity = dr.GetDecimal("UnloadedQuantity"),
                EwayBill = new EwayBillModel
                {
                    EwayBillDate = dr.GetDateTime("EwayBillDate"),
                    EwayBillRef = dr.GetString("EwayBillRef"),
                    ExpireOn = dr.GetDateTime("ExpireOn"),
                    GcID = dr.GetInt64("GcID"),
                },
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        public int AppendToTrip(long TripID, long GcSetID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GcSetID", GcSetID),
               new SqlParameter("@TripID", TripID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Operation", "Append"),
            };
            return Iservice.ExecuteNonQuery("[usp.GcSet.AppendTrip]", parameters);
        }

        public int RemoveFromTrip(long GcSetID, int TripID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GcSetID", GcSetID),
               new SqlParameter("@TripID", TripID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Operation", "Remove"),
            };
            return Iservice.ExecuteNonQuery("[usp.GcSet.AppendTrip]", parameters);
        }

        public int UpdateEwayBill(EwayBillModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@GcID",model.GcID),
                new SqlParameter("@EwayBillRef", model.EwayBillRef),
                new SqlParameter("@EwayBillDate", model.EwayBillDate),
                new SqlParameter("@ExpireOn",model.ExpireOn),
                new SqlParameter("@UserID",model.UserInfo.UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Gc.EwayBill.Update]", parameters);
        }
    }
}
