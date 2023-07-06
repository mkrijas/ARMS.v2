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
        IEnumerable<ResoleDeliveryModel> SelectResoleDeliveryViewList(int? ID);
        IEnumerable<ResoleDeliveryTyreModel> SelectResoleDeliveryTyresList(int? ResoleID, int? DeliveryID);
        int ResoleDeliveryUpdate(ResoleDeliveryModel model);
        int UndoResoleDelivery(int? DeliveryId, string UserID);
        IEnumerable<LinkableBatchModel> GetNonLinkedTyreBatches(int BranchID, int ItemID);
    }
}