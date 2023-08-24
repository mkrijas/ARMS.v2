using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Core.BaseModels.Inventory;
using ArmsModels.BaseModels;
using ArmsModels.SharedModels;

namespace ArmsServices.DataServices
{
    public class StockTransferService : IStockTransferService
    {
        IDbService Iservice;
        public StockTransferService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public StockTransferInitiationModel Update(StockTransferInitiationModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@InitiationStoreID",model.InitiatedStore.StoreID),
               new SqlParameter("@DestinationBranchID",model.DestinationBranch.BranchID),
               new SqlParameter("@RecordStatus",model.Status),
               new SqlParameter("@Items",model.ItemsList.ToDataTable()),
               new SqlParameter("@UserID",model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[Inventory.Store.Transfer.Initiation]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private StockTransferInitiationModel GetModel(IDataRecord dr)
        {
            return new StockTransferInitiationModel
            {
                StockTransferID = dr.GetInt32("StockTransferID"),
                InvTranID = dr.GetInt32("InvTranID"),
                InitiatedStore = new StoreModel
                    {
                        StoreID = dr.GetInt32("InitiationStoreID")
                    },
                DestinationBranch = new BranchModel
                    {
                        BranchID = dr.GetInt32("DestinationBranchID")
                    },
                InitiatedDate = dr.GetDateTime("InitiatedDate"),
                UserInfo = new UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecordStatus"),
                        TimeStampField = dr.GetDateTime("TimeStamp"),
                        UserID = dr.GetString("UserID"),
                    },
            };
        }
    }
}