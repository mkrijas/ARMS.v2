using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Reflection;


namespace ArmsServices.DataServices
{
    public class GcService : IGcService
    {
        IDbService Iservice;
        ITariffService Itariff;

        public GcService(IDbService iservice, ITariffService tariff)
        {
            Iservice = iservice;
            Itariff = tariff;
        }

        // Method to update a GC set
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
               new SqlParameter("@OrderTime", model.OrderTime),
               new SqlParameter("@PaidBy", model.PaidBy),
               new SqlParameter("@LRDate", model.LRDate),
               new SqlParameter("@Gcs", model.Gcs.ToDataTable()),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.GcSet.Update]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Method to delete a GC set by its ID
        public int DeleteSet(long? id, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GcSetID", id),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.GcSet.Delete]", parameters);
        }

        // Method to delete a GC by its ID
        public int Delete(long? GcID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GcID", GcID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Gc.Gcs.Delete]", parameters);
        }

        // Method to update the unloading quantity of a GC set
        public int UpdateUnloadingQuantity(GcSetModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GcSetID", model.GcSetID),
               new SqlParameter("@UnloadedQuantity", model.TotalUnloadingQuantity),
               new SqlParameter("@PONo", model.PONo),
               new SqlParameter("@Acknowledgement", model.Acknowledgement)
            };
            return Iservice.ExecuteNonQuery("[usp.GcSet.UpdateUnloadingQuantity]", parameters);
        }

        // Method to select all GC sets for a specific branch
        public List<GcSetModel> Select(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "All"),
               new SqlParameter("@BranchID", BranchID)
            };
            return GetList(parameters);
        }

        // Method to select GC sets by trip ID
        public List<GcSetModel> SelectByTrip(long? TripID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByTrip"),
               new SqlParameter("@TripID", TripID)
            };
            return GetList(parameters);
        }

        // Method to select GC sets that are ready to unload
        public List<GcSetModel> SelectToUnload(long? TripID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "Unloadable"),
               new SqlParameter("@TripID", TripID)
            };
            return GetList(parameters);
        }

        // Method to select unloaded GC sets for mobile 
        public List<GcSetModel> SelectUnloadedMobile(long? TripID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "Unload"),
               new SqlParameter("@TripID", TripID)
            };
            return GetList(parameters);
        }

        // Method to select pending GC sets for a trip
        public List<GcSetModel> SelectPending(long? TripID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "Pending"),
               new SqlParameter("@TripID", TripID)
            };
            return GetList(parameters);
        }

        // Method to select GC sets that are ready to dispatch
        public List<GcSetModel> SelectToDispatch(long? TripID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "Dispatchable"),
               new SqlParameter("@TripID", TripID)
            };
            return GetList(parameters);
        }

        // Method to select unassigned GC sets for a branch
        public List<GcSetModel> SelectUnAssigned(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "UnAssigned"),
               new SqlParameter("@BranchID", BranchID)
            };
            return GetList(parameters);
        }

        // Method to select GC sets that have started unloading
        public List<GcSetModel> SelectedUnloadEvent(long? TripID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "UnloadStarted"),
               new SqlParameter("@TripID", TripID)
            };
            return GetList(parameters);
        }

        // Method to select all GC types
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

        // Method to select a GC set by its ID
        public GcSetModel SelectByID(long? GcSetID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GcSetID", GcSetID),
               new SqlParameter("@Operation", "ByID"),
            };
            var list = GetList(parameters);
            return list.FirstOrDefault();
        }

        // Helper method to get a list of GC sets based on parameters
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

                set.SetBillNumber = set.SetBillNumber + (IsFirst ? string.Empty : ", ") + "(" + gc.BillNumber + " - " + gc.BillDate?.ToString("dd/MM/yy") + ")";
                set.SetGcNumber = set.SetGcNumber + (IsFirst ? gc.GcPrefix : ", ") + gc.GcNumber;
                set.TotalBillQuantity = (set.TotalBillQuantity ?? 0) + gc.BillQuantity;
                set.TotalUnloadingQuantity = (set.TotalUnloadingQuantity ?? 0) + gc.UnloadedQuantity;
                set.OrderTime = gc.OrderTime;
                set.Gcs.Add(gc);
            }
            return list;
        }

        // Helper method to map data record to GcSetModel
        private GcSetModel GetModel(IDataRecord dr)
        {
            return new GcSetModel
            {
                GcSetID = dr.GetInt64("GcSetID"),
                BranchID = dr.GetInt32("BranchID"),
                GcDate = dr.GetDateTime("GcDate"),
                OrderID = dr.GetInt32("OrderID"),                
                OrderName = dr.GetString("OrderName"),
                RouteID = dr.GetInt32("RouteID"),
                RouteName = dr.GetString("RouteName"),
                TripID = dr.GetInt64("TripID"),
                Content = dr.GetString("ContentName"),
                UoM = dr.GetString("UoM"),
                ConsigneeID = dr.GetInt32("ConsigneeID"),
                ConsigneeName = dr.GetString("ConsigneeName"),
                ConsignorID = dr.GetInt32("ConsignorID"),
                ConsignorName = dr.GetString("ConsignorName"),
                PaidBy = dr.GetByte("PaidBy"),
                LRDate = dr.GetDateTime("LRDate"),
                LoadEndEventID = dr.GetInt64("LoadEndEventID"),
                LoadStartEventID = dr.GetInt64("LoadStartEventID"),
                UnloadEndEventID = dr.GetInt64("UnloadEndEventID"),
                UnloadStartEventID = dr.GetInt64("UnloadStartEventID"),
                PONo =  dr.GetString("PONo"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        // Helper method to map data record to GcModel
        private GcModel GetGcModel(IDataRecord dr)
        {
            return new GcModel
            {
                GcID = dr.GetInt64("GcID"),
                GcSetID = dr.GetInt64("GcSetID"),
                BillDate = dr?.GetDateTime("BillDate"),
                BillNumber = dr.GetString("BillNumber"),
                BillQuantity = dr.GetDecimal("BillQuantity"),
                //TotalBillQuantity = dr.GetDecimal("TotalBillQuantity"),
                GcNumber = dr.GetInt32("GcNo"),
                GcPrefix = dr.GetString("GcPrefix"),
                GcType = dr.GetInt16("GcType"),
                OrderTime = dr.GetDateTime("OrderTime"),
                GcTypeName = dr.HasColumn("GcTypeName") ? dr.GetString("GcTypeName") : null,
                PassNumber = dr.GetString("PassNumber"),
                LRNo = dr.GetString("LRNo"),
                Freight = dr.GetDecimal("Freight"),
                EFreight = dr.GetDecimal("Freight"),
                UnloadedQuantity = dr.GetDecimal("UnloadedQuantity"),
                IsAcknowledged = dr.GetBoolean("IsAcknowledged"),
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

        // Method to append a GC set to a trip
        public int AppendToTrip(long? TripID, long? GcSetID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GcSetID", GcSetID),
               new SqlParameter("@TripID", TripID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Operation", "Append"),
            };
            return Iservice.ExecuteNonQuery("[usp.GcSet.EventUpdate]", parameters);
        }

        // Method to begin unloading a GC set
        public int BeginUnload(long? TripID, long? GcSetID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GcSetID", GcSetID),
               new SqlParameter("@TripID", TripID),
               new SqlParameter("@Operation", "BeginUnload"),
            };
            return Iservice.ExecuteNonQuery("[usp.GcSet.EventUpdate]", parameters);
        }

        // Method to remove a GC set from a trip
        public int RemoveFromTrip(long? GcSetID, long? TripID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GcSetID", GcSetID),
               new SqlParameter("@TripID", TripID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Operation", "Remove"),
            };
            return Iservice.ExecuteNonQuery("[usp.GcSet.EventUpdate]", parameters);
        }

        // Method to update the E-way bill details
        public EwayBillModel UpdateEwayBill(EwayBillModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@GcID",model.GcID),
                new SqlParameter("@EwayBillRef", model.EwayBillRef),
                new SqlParameter("@EwayBillDate", model.EwayBillDate),
                new SqlParameter("@ExpireOn",model.ExpireOn),
                new SqlParameter("@UserID",model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Gc.EwayBill.Update]", parameters))
            {
                return new EwayBillModel()
                {
                    GcID = dr.GetInt64("GcID"),
                    EwayBillRef = dr.GetString("EwayBillRef"),
                    EwayBillDate = dr.GetDateTime("EwayBillDate"),
                    ExpireOn = dr.GetDateTime("ExpireOn"),
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecordStatus"),
                        TimeStampField = dr.GetDateTime("TimeStamp"),
                        UserID = dr.GetString("UserID"),
                    },
                };
            }
            return null;
        }

        // Method to confirm the acknowledgement of a GC
        public int ConfirmAcknowledgement(long? GcID, bool IsAcknowledged)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GcID", GcID),
               new SqlParameter("@IsAcknowledged", IsAcknowledged),
            };
            return Iservice.ExecuteNonQuery("[usp.Gc.Gcs.Acknowledged]", parameters);
        }

        // Method to get GCs associated with a specific GC set
        public IEnumerable<GcModel> GetGcs(long? GcSetID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GcSetID", GcSetID),
               new SqlParameter("@Operation", "ByID"),
            };
            foreach (var reader in Iservice.GetDataReader("[usp.GcSet.Select]", parameters))
            {
                yield return GetGcModel(reader);
            }
        }
    }
}
