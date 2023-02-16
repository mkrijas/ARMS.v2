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
        TyreModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);        
        IEnumerable<TyreModel> SelectByBranch(int? BranchID);
        IEnumerable<TyreModel> SelectByTruck(int? TruckID);
        int Mount(TyreMountedModel model);
        int Unmount(int? TyreID, DateTime? UnmountedOn, int? UnmountedKm, string UserID);
        int Unmount(string UserID, int? MountedID, DateTime? UnmountedOn, int? UnmountedKm);
        int ResoleBegin(TyreResoleModel model);
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

        public int Unmount(int? TyreID,DateTime? UnmountedOn,int? UnmountedKm, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TyreID", TyreID),
               new SqlParameter("@UnmountedKM", UnmountedKm),
               new SqlParameter("@UnmountedOn", UnmountedOn),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.Tyre.Unmount]", parameters);
        }

        public int Unmount(string UserID, int? MountedID, DateTime? UnmountedOn, int? UnmountedKm )
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@MountedID", MountedID),
               new SqlParameter("@UnmountedKM", UnmountedKm),
               new SqlParameter("@UnmountedOn", UnmountedOn),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.Tyre.Unmount]", parameters);
        }

        public int ResoleBegin(TyreResoleModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@PartyID", model.Party.PartyID),
               new SqlParameter("@RequestedDate", model.RequestedDate),
               new SqlParameter("@Tyres", model.Tyres.ToDataTable()),               
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.Tyre.Resole.Begin]", parameters);
        }

        public IEnumerable<TyreModel> SelectByBranch(int BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@Operation", "ByBranch"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Tyre.Select]", parameters))
            {
                yield return GetModel(dr);
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

      

        public TyreModel Update(TyreModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TyreID", model.TyreID),
               new SqlParameter("@BranchID",model.BranchID),
               new SqlParameter("@TyreSize",model.TyreSize),
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


