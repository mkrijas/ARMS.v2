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
        TyreModel Update(TyreModel model);  //Edit
        TyreTypeAndPositionMappingModel UpdateMappingTyrePositionAndType(TyreTypeAndPositionMappingModel model);  //Edit
        public IEnumerable<TyreTypeAndPositionMappingModel> SelectMappingTyrePositionAndType(int? TrucktypeID);
        TyreModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);  //Delete
        IEnumerable<TyreModel> SelectByBranch(int? BranchID);
        IEnumerable<TyreModel> SelectByTruck(int? TruckID);
        IEnumerable<TyreModel> SelectUnmontedTyresByID(int? ID);
        IEnumerable<TyreModel> SelectUnmontedTyresByBranch(int? ID);
        IEnumerable<TyreMountedModel> SelectMountedTyresByID(int? TruckID);
        IEnumerable<TyreMountedModel> SelectMountedTyresByTyreID(int? TyreID);
        IEnumerable<TyrePositionModel> GetTyrePositionList(int? ID = null);
        IEnumerable<TyrePositionModel> GetTyrePositionListUsingTruckTypeId(int? TruckTypeId = null);
        int Mount(TyreMountedModel model);  //Edit
        int Unmount(int? TyreID, DateTime? UnmountedOn, int? UnmountedKm, string UserID);  //Delete
        int Unmount(string UserID, int? MountedID, DateTime? UnmountedOn, int? UnmountedKm);  //Delete
        int ResoleBegin(TyreResoleModel model);  //Edit
        int ResoleCancel(int? ID, string UserID);  //Cancel
        IEnumerable<int?> ResoleTyresByResoleId(int? ResoleId);
        IEnumerable<TyreResoleModel> SelectTyreResoleList(int? ID);
        IEnumerable<TyreResoleModel> SelectTyreResoleListByBranchId(int? ID);
        IEnumerable<ResoleDeliveryModel> SelectResoleDeliveryViewList(int? ID);
        IEnumerable<ResoleDeliveryModel> SelectResoleDeliveryListByBranch(int? ID);
        IEnumerable<ResoleDeliveryTyreModel> SelectResoleDeliveryTyresList(int? ResoleID, int? DeliveryID);
        int ResoleDeliveryUpdate(ResoleDeliveryModel model);  //Edit
        int UndoResoleDelivery(int? DeliveryId, string UserID);  //Cancel
        IEnumerable<LinkableBatchModel> GetNonLinkedTyreBatches(int BranchID, int ItemID);
    }
}