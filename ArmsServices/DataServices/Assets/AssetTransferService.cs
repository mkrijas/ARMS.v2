using ArmsModels.BaseModels;
using Core.BaseModels.Finance.Transactions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ArmsServices.DataServices
{
    // Service class for managing asset transfers
    public class AssetTransferService : IAssetTransferService
    {
        IDbService Iservice;

        public AssetTransferService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Deletes an asset transfer initiation record and returns the number of affected rows
        public int DeleteInitiation(int? ID, int? BranchID, int? AssetID,int? TruckID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@AssetID", AssetID),
               new SqlParameter("TruckID", TruckID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Asset.Transfer.Delete]", parameters);
        }

        // Retrieves a list of outgoing asset transfers for a specific branch
        public IEnumerable<AssetTransferInitiationModel> SelectOutgoingAssets(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "Outgoing"),
               new SqlParameter("@ID", BranchID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Transfer.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Retrieves a checklist of asset settings based on subclass ID
        public IEnumerable<AssetSettingsModel> GetCheckList(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetSub"),
               new SqlParameter("@ID", ID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Transfer.Select]", parameters))
            {
                yield return new AssetSettingsModel()
                {
                    CheckListID = dr.GetInt32("CheckListID"),
                    SettingsID = dr.GetInt32("AssetSettingsID"),
                    SettingsName = dr.GetString("SettingsName"),
                    SettingsDescription = dr.GetString("Description"),
                };
            }
        }

        // Removes a photo associated with an asset transfer and returns the number of affected rows
        public int RemovePhoto(AssetTransferInitiationModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetTransferID", model.AssetTransferID),
               new SqlParameter("@Images", model.Images),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Asset.Transfer.RemovePhoto]", parameters);
        }

        // Updates an outgoing asset transfer and returns the updated model
        public AssetTransferInitiationModel UpdateOutgoing(AssetTransferInitiationModel model, int? TruckID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetTransferID", model.AssetTransferID),
               new SqlParameter("@AssetID", model.Asset.AssetID),
               new SqlParameter("@TruckID", TruckID),
               new SqlParameter("@InitiatedBranchID", model.InitiatedBranch?.BranchID??null),
               new SqlParameter("@DestinationBranchID", model.DestinationBranch.BranchID),
               new SqlParameter("@TransferInitiatedDate", model.TransferInitiatedDate),
               new SqlParameter("@ImagePath", string.Join(";",model.ImagePath)),
               new SqlParameter("@Remarks", model.Remarks),
               new SqlParameter("@RecordStatus", 3),
               new SqlParameter("@UserID", model.UserInfo.UserID),
               new SqlParameter("@CheckList", model.CheckList.ToDataTable()),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Transfer.Update]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Retrieves a list of incoming asset transfers for a specific branch
        public IEnumerable<AssetTransferInitiationModel> SelectIncomingAssets(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "Incoming"),
               new SqlParameter("@ID", BranchID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Transfer.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Updates the status of an asset transfer and returns the updated model
        public AssetTransferInitiationModel UpdateStatus(AssetTransferInitiationModel model, int? TruckID, List<int?> RecievedList)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("IntField", typeof(int));
            foreach (int? value in RecievedList)
            {
                if (value.HasValue)
                {
                    dataTable.Rows.Add(value.Value);
                }
                else
                {
                    dataTable.Rows.Add(DBNull.Value);
                }
            }
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetTransferEndID", model.AssetTransferEndModel.AssetTransferEndID),
               new SqlParameter("@AssetTransferID", model.AssetTransferID),
               new SqlParameter("@AssetID", model.Asset.AssetID),
               new SqlParameter("@TruckID", TruckID),
               new SqlParameter("@InitiatedBranchID", model.InitiatedBranch.BranchID),
               new SqlParameter("@BranchID", model.DestinationBranch.BranchID),
               new SqlParameter("@TransferEndDate", model.AssetTransferEndModel.TransferEndDate),
               new SqlParameter("@Remarks", model.AssetTransferEndModel.Remarks),
               new SqlParameter("@RecordStatus", 3),
               new SqlParameter("@UserID", model.UserInfo.UserID),
               new SqlParameter("@Status", model.AssetTransferEndModel.TransferStatus),
               new SqlParameter("@CheckList", dataTable),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Transfer.Status.Update]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Helper method to map data from the database to an AssetTransferInitiationModel
        private AssetTransferInitiationModel GetModel(IDataRecord dr)
        {
            return new AssetTransferInitiationModel()
            {
                AssetTransferID = dr.GetInt32("AssetTransferID"),
                ImagePath = dr.GetString("ImagePath").Split(";").ToList(),
                InitiatedBranch = new BranchModel()
                {
                    BranchID = dr.GetInt32("InitiatedBranchID"),
                    BranchName = dr.GetString("InitiatedBranchName"),

                },
                DestinationBranch = new BranchModel()
                {
                    BranchID = dr.GetInt32("DestinationBranchID"),
                    BranchName = dr.GetString("DestinationBranchName"),
                },
                TransferInitiatedDate = dr.GetDateTime("TransferInitiatedDate"),
                AssetTransferEndModel = new AssetTransferEndModel()
                {

                    AssetTransferEndID = dr.GetInt32("AssetTransferEndID"),
                    TransferStatus = dr.GetBooleanNullable("TransferStatus"),
                    //TransferEndDate = dr.GetDateTime("TransferEndDate"),
                },
                Remarks = dr.GetString("Remarks"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
                Asset = new AssetModel()
                {
                    AssetID = dr.GetInt32("AssetID"),
                    Description = dr.GetString("Description"),
                    AssetCode = dr.GetString("AssetCode"),
                },
            };
        }
    }
}