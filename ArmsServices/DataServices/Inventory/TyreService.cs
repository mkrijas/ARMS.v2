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
    public interface ITyreService
    {
        TyreModel Update(TyreModel model);
        TyreTypeAndPositionMappingModel UpdateMappingTyrePositionAndType(TyreTypeAndPositionMappingModel model);
        public IEnumerable<TyreTypeAndPositionMappingModel> SelectMappingTyrePositionAndType(int? TrucktypeID);
        TyreModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);
        IEnumerable<TyreModel> SelectByBranch(int? BranchID);
        IEnumerable<TyreModel> SelectByTruck(int? TruckID);
        IEnumerable<TyreModel> SelectUnmontedTyresByID(int? ID);
        IEnumerable<TyreModel> SelectUnmontedTyresByBranch(int? ID);
        IEnumerable<TyreMountedModel> SelectMountedTyresByID(int? TruckID);
        IEnumerable<TyreMountedModel> SelectMountedTyresByTyreID(int? TyreID);
        IEnumerable<TyrePositionModel> GetTyrePositionList(int? ID = null);
        IEnumerable<TyrePositionModel> GetTyrePositionListUsingTruckTypeId(int? TruckTypeId = null);
        int Mount(TyreMountedModel model);
        int Unmount(int? TyreID, DateTime? UnmountedOn, int? UnmountedKm, string UserID);
        int Unmount(string UserID, int? MountedID, DateTime? UnmountedOn, int? UnmountedKm);
        int ResoleBegin(TyreResoleModel model);
        int ResoleCancel(int? ID, string UserID);
        IEnumerable<int?> ResoleTyresByResoleId(int? ResoleId);
        IEnumerable<TyreResoleModel> SelectTyreResoleList(int? ID);
        IEnumerable<LinkableBatchModel> GetNonLinkedTyreBatches(int BranchID, int ItemID);
    }
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
               new SqlParameter("@TyreID", ID),
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
               new SqlParameter("@TruckID", model.TruckID),
               new SqlParameter("@TyreID", model.TyreID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.Tyre.Mount]", parameters);
        }

        public int Unmount(int? TyreID, DateTime? UnmountedOn, int? UnmountedKm, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TyreID", TyreID),
               new SqlParameter("@UnmountedKM", UnmountedKm),
               new SqlParameter("@UnmountedOn", UnmountedOn),
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
               new SqlParameter("@Party", model.Party.PartyID),
               new SqlParameter("@RequestedDate", model.RequestedDate),
               new SqlParameter("@Tyres", model.Tyres.Select(s=>s.Value).ToList().ToDataTable()),
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

        public int ResoleCancel(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID)
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.Tyre.Resole.Update]", parameters);

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
                InventoryBatchID = dr.GetInt32("InventoryBatchID"),
                InventoryItemID = dr.GetInt32("InventoryItemID"),
                Make = dr.GetString("make"),
                Tubeless = dr.GetBoolean("Tubeless"),
                TyreSize = dr.GetString("TyreSize"),
                TyreType = dr.GetString("TyreType"),
                TyrePosition = dr.GetString("TyrePosition"),
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
                Party = new PartyModel() {PartyID = dr.GetInt32("Party") },
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("MountedOn"),
                    UserID = dr.GetString("UserID"),
                },
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
                TruckTypeID = dr.GetInt32("TruckTypeID"),
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


