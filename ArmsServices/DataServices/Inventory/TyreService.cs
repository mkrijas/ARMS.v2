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
    public class TyreService : ITyreService
    {
        IDbService Iservice;
        public TyreService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public int Delete(int? ID, string UserID)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.Tyre.Delete]", parameters);
        }

        public IEnumerable<LinkableBatchModel> GetNonLinkedTyreBatches(int BranchID, int ItemID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetNonlinkedBatches"),
               new SqlParameter("@itemID",ItemID),
               new SqlParameter("@BranchID",BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Tyre.Select]", parameters))
            {
                yield return new LinkableBatchModel()
                {
                    BatchID = dr.GetInt64("BatchID"),
                    LinkableQty = dr.GetDecimal("LinkableQty"),
                    GrnNo = dr.GetString("GrnNo"),
                    PartyID = dr.GetInt32("PartyID")
                };
            }
        }

        public int Mount(TyreMountedModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@MountedKM", model.MountedKM),
               new SqlParameter("@MountedOn", model.MountedOn),
               new SqlParameter("@PositionID", model.PositionID),
               new SqlParameter("@RequestID", model.RequestID),
               new SqlParameter("@TruckID", model.TruckID),
               new SqlParameter("@TyreID", model.TyreID),
               new SqlParameter("@CostCenter", model.CostCenter),
               new SqlParameter("@Dimension", model.Dimension),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.Tyre.Mount]", parameters);
        }

        public int Unmount(int? TyreID, DateTime? UnmountedOn, int? UnmountedKm,StoreModel Store, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TyreID", TyreID),
               new SqlParameter("@UnmountedKM", UnmountedKm),
               new SqlParameter("@UnmountedOn", UnmountedOn),
               new SqlParameter("StoreID", Store.StoreID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.Tyre.UnMount]", parameters);
        }

        public int Unmount(string UserID, int? MountedID, DateTime? UnmountedOn, int? UnmountedKm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@MountedID", MountedID),
               new SqlParameter("@UnmountedKM", UnmountedKm),
               new SqlParameter("@UnmountedOn", UnmountedOn),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.Tyre.UnMount]", parameters);
        }

        public int ResoleBegin(TyreResoleModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@Party", model.Party?.PartyID??null),
               new SqlParameter("@RequestedDate", model.RequestedDate),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@Tyres", model.Tyres.Select(s=>s.Value)?.ToList()?.ToDataTable()??null),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.Tyre.Resole.Update]", parameters);
        }

        public IEnumerable<TyreResoleModel> SelectTyreResoleList(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Tyre.Resole.Select]", parameters))
            {
                yield return GetTyreResoleModel(dr);
            }
        }
        public IEnumerable<TyreResoleModel> SelectTyreResoleListByBranchId(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByBranchID"),
               new SqlParameter("@ID", ID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Tyre.Resole.Select]", parameters))
            {
                yield return GetTyreResoleModel(dr);
            }
        }

        public IEnumerable<ResoleDeliveryModel> SelectResoleDeliveryViewList(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.Tyre.Resole.NotDeliveredAndDelivered.Select]", parameters))
            {
                yield return GetResoleDeliveryModel(dr);
            }
        }
        public IEnumerable<ResoleDeliveryModel> SelectResoleDeliveryListByBranch(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByBranchID"),
               new SqlParameter("@ID", ID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.Tyre.Resole.NotDeliveredAndDelivered.Select]", parameters))
            {
                yield return GetResoleDeliveryModel(dr);
            }
        }

        public IEnumerable<ResoleDeliveryTyreModel> SelectResoleDeliveryTyresList(int? ResoleID, int? DeliveryID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ResoleID", ResoleID),
               new SqlParameter("@DeliveryID", DeliveryID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.Tyre.Resole.DeliveredAndNonDeliveredTyres.Select]", parameters))
            {
                yield return GetResoleDeliveryTyreModel(dr);
            }
        }

        public int ResoleDeliveryUpdate(ResoleDeliveryModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@ResoleID", model.ResoleID),
               new SqlParameter("@PartyID", model.Party.PartyID),
               new SqlParameter("@DeliveryDate", model.DeliveryDate),
               new SqlParameter("@UsageCode", model.UsageCode),
               new SqlParameter("@TaxIncluded", model.TaxIncluded),
               new SqlParameter("@PID", model.PID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@ResoleDeliveryTyres", model.ResoleDeliveryTyreList.Select(s=>new {
                   ID =s.ID,
                   DeliveryID = s.DeliveryID ,
                   TyreID = s.TyreID ,
                   Status = s.Status ,
                   Amount = s.Amount ,
                   Tax = s.Tax
               }).ToList().ToDataTable()),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.Tyre.Resole.Delivery.Update]", parameters);
        }

        public int UndoResoleDelivery(int? DeliveryId, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DeliveryID", DeliveryId),
               new SqlParameter("@UserID", UserID)
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.Tyre.Resole.Delivery.Delete]", parameters);
        }

        public int ResoleCancel(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID)
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.Tyre.Resole.Delete]", parameters);

        }

        public IEnumerable<int?> ResoleTyresByResoleId(int? ResoleId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ResoleId)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.Tyre.Resole.SelectTyreIds]", parameters))
            {
                yield return dr.GetInt32("TyreID");
            }

        }

        public IEnumerable<TyreModel> SelectByBranch(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@Operation", "ByBranch")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Tyre.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public TyreModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TyreID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Tyre.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        public IEnumerable<TyreModel> SelectByTruck(int? TruckID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", TruckID),
               new SqlParameter("@Operation", "ByTruck")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Tyre.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<TyreModel> SelectUnmontedTyresByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.Tyre.UnMount.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<TyreModel> SelectUnmontedTyresByBranch(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@Operation", "ByBranch")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.Tyre.UnMount.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<TyreMountedModel> SelectMountedTyresByID(int? TruckID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", TruckID),
               new SqlParameter("@Operation", "ByTruckID"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.Tyre.Mount.Select]", parameters))
            {
                yield return GetTyreMountModel(dr);
            }
        }

        public IEnumerable<TyreMountedModel> SelectMountedTyresByTyreID(int? TyreID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", TyreID),
               new SqlParameter("@Operation", "ByTyreID"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.Tyre.Mount.Select]", parameters))
            {
                yield return GetTyreMountModel(dr);
            }
        }

        public IEnumerable<TyrePositionModel> GetTyrePositionList(int? ID = null)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Tyre.Postion.Select]", parameters))
            {
                yield return GetPositionModel(dr);
            }
        }

        public IEnumerable<TyrePositionModel> GetTyrePositionListUsingTruckTypeId(int? TruckTypeId = null)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckTypeId", TruckTypeId)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Tyre.Postion.SelectUsingTruckTypeId]", parameters))
            {
                yield return GetPositionModel(dr);
            }
        }



        public TyreModel Update(TyreModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TyreID", model.TyreID),
               new SqlParameter("@BranchID",model.BranchID),
               new SqlParameter("@TyreSize",model.TyreSize),
               new SqlParameter("@TyreType",model.TyreType),
               new SqlParameter("@TyreStatus",model.TyreStatus),
               new SqlParameter("@TotalExpectedLife",model.TotalExpectedLife),
               new SqlParameter("@Tubeless",model.Tubeless),
               new SqlParameter("@InventoryItemID",model.InventoryItemID),
               new SqlParameter("@Make",model.Make),
               new SqlParameter("@InventoryBatchID",model.InventoryBatchID),
               new SqlParameter("@TyreSerialNumber",model.TyreSerialNumber),
               new SqlParameter("@UserID",model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Tyre.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }


        public IEnumerable<TyreTypeAndPositionMappingModel> SelectMappingTyrePositionAndType(int? TrucktypeID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TrucktypeID", TrucktypeID)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Tyre.Postion.Trucktype.Mapping.Select]", parameters))
            {
                yield return GetPositionAndTypeMappingModel(dr);
            }
        }

        public TyreTypeAndPositionMappingModel UpdateMappingTyrePositionAndType(TyreTypeAndPositionMappingModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@TruckTypeID", model.TruckTypeID),
               new SqlParameter("@PositionIDs",model.PositionIDs),
               new SqlParameter("@UserID",model.UserInfo.UserID),
               new SqlParameter("@RecordStatus",3),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Tyre.Postion.Trucktype.Mapping.Update]", parameters))
            {
                model = GetPositionAndTypeMappingModel(dr);
            }
            return model;
        }

        private TyreModel GetModel(IDataRecord dr)
        {
            return new TyreModel()
            {
                TyreSerialNumber = dr.GetString("TyreSerialNumber"),
                TyreID = dr.GetInt32("TyreID"),
                BranchID = dr.GetInt32("BranchID"),
                InventoryBatchID = dr.GetInt64("InventoryBatchID"),
                InventoryItemID = dr.GetInt32("InventoryItemID"),
                Make = dr.GetString("make"),
                Tubeless = dr.GetBoolean("Tubeless"),
                TyreSize = dr.GetString("TyreSize"),
                TyreType = dr.GetString("TyreType"),
                TyreStatus = dr.GetByte("TyreStatus"),
                TyrePosition = dr.GetString("TyrePosition"),
                IsMounted = dr.GetBoolean("IsMounted"),
                TotalExpectedLife = dr.GetInt32("TotalExpectedLife"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        private TyreResoleModel GetTyreResoleModel(IDataRecord dr)
        {
            return new TyreResoleModel()
            {
                ID = dr.GetInt32("ID"),
                RequestedDate = dr.GetDateTime("RequestedDate"),
                Party = new PartyModel() { PartyID = dr.GetInt32("Party") },
                DeliveryID = dr.GetInt32("DeliveryID"),
                BranchID = dr.GetInt32("BranchID"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("MountedOn"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        private ResoleDeliveryModel GetResoleDeliveryModel(IDataRecord dr)
        {
            return new ResoleDeliveryModel()
            {
                ID = dr.GetInt32("ID"),
                ResoleID = dr.GetInt32("ResoleID"),
                Party = new PartyModel() { PartyID = dr.GetInt32("Party") },
                RequestedDate = dr.GetDateTime("RequestedDate"),
                DeliveryDate = dr.GetDateTime("DeliveryDate"),
                TaxIncluded = dr.GetBoolean("TaxIncluded"),
                UsageCode = dr.GetString("UsageCode"),
                PID = dr.GetInt32("PID"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("DeliveryDate") == null ? dr.GetDateTime("RequestedDate") : dr.GetDateTime("DeliveryDate"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        private ResoleDeliveryTyreModel GetResoleDeliveryTyreModel(IDataRecord dr)
        {
            return new ResoleDeliveryTyreModel()
            {
                ID = dr.GetInt32("ID"),
                TyreID = dr.GetInt32("TyreID"),
                DeliveryID = dr.GetInt32("DeliveryID"),
                Status = dr.GetBoolean("Status"),
                Amount = dr.GetDecimal("Amount"),
                Tax = dr.GetDecimal("Tax"),

            };
        }

        private TyreMountedModel GetTyreMountModel(IDataRecord dr)
        {
            return new TyreMountedModel()
            {
                ID = dr.GetInt32("ID"),
                TyreID = dr.GetInt32("TyreID"),
                TyreNo = dr.GetString("TyreSerialNumber"),
                TruckID = dr.GetInt32("TruckID"),
                PositionID = dr.GetInt32("PositionID"),
                PositionName = dr.GetString("Description"),
                MountedOn = dr.GetDateTime("MountedOn"),
                MountedKM = dr.GetInt32("MountedKM"),
                UnmountedKM = dr.GetInt32("MountedKM") + dr.GetInt32("RunKM"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("MountedOn"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        private TyrePositionModel GetPositionModel(IDataRecord dr)
        {
            return new TyrePositionModel()
            {
                PositionID = dr.GetInt32("ID"),
                Side = dr.GetString("Side"),
                Description = dr.GetString("Description"),
                SRow = dr.GetInt32("SRow"),
                SColumn = dr.GetInt32("SColumn"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        private TyreTypeAndPositionMappingModel GetPositionAndTypeMappingModel(IDataRecord dr)
        {
            return new TyreTypeAndPositionMappingModel()
            {
                ID = dr.GetInt32("ID"),
                TruckTypeID = dr.GetInt16("TruckTypeID"),
                PositionIDs = dr.GetString("PositionIDs"),
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


