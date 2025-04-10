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
        public int DeleteInitiation(int? ID, int? BranchID, int? AssetID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@AssetID", AssetID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Asset.Transfer.Delete]", parameters);
        }

        // Retrieves a list of outgoing asset transfers for a specific branch
        public IEnumerable<AssetTransferInitiationModel> SelectOutgoingAssets(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetFlow", "Outgoing"),
               new SqlParameter("@ID", BranchID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Transfer.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<AssetTransferInitiationModel> SelectOutgoingApproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetFlow", "Outgoing"),
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@ID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Transfer.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<AssetTransferInitiationModel> SelectOutgoingUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetFlow", "Outgoing"),
               new SqlParameter("@Operation", "ByUnapproved"),
               new SqlParameter("@ID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
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
                    Value = dr.GetString("Value"),
                    Condition = dr.GetString("Condition")
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
        public AssetTransferInitiationModel UpdateOutgoing(AssetTransferInitiationModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetTransferID", model.AssetTransferID),
               new SqlParameter("@AssetID", model.Asset.AssetID),
               new SqlParameter("@InitiatedBranchID", model.InitiatedBranch?.BranchID??null),
               new SqlParameter("@DestinationBranchID", model.DestinationBranch.BranchID),
               new SqlParameter("@TransferInitiatedDate", model.TransferInitiatedDate),
               new SqlParameter("@ImagePath", string.Join(";",model.ImagePath)),
               new SqlParameter("@Remarks", model.Remarks),
               //new SqlParameter("@RecordStatus", 3),
               new SqlParameter("@UserID", model.UserInfo.UserID),
               new SqlParameter("@GstMechanism", model.GstMechanism),
               new SqlParameter("@AssetList", model.Items.ToDataTable()),
               new SqlParameter("@CheckList", model.CheckList.ToDataTable()),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Transfer.Initiate]", parameters))
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
               new SqlParameter("@AssetFlow", "Incoming"),
               new SqlParameter("@ID", BranchID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Transfer.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<AssetTransferInitiationModel> SelectIncomingApproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetFlow", "Incoming"),
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@ID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Transfer.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<AssetTransferInitiationModel> SelectIncomingUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetFlow", "Incoming"),
               new SqlParameter("@Operation", "ByUnapproved"),
               new SqlParameter("@ID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Transfer.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Updates the status of an asset transfer and returns the updated model
        public AssetTransferInitiationModel UpdateStatus(AssetTransferInitiationModel model, List<int?> RecievedList)
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
               //new SqlParameter("@InitiatedBranchID", model.InitiatedBranch.BranchID),
               new SqlParameter("@BranchID", model.DestinationBranch.BranchID),
               new SqlParameter("@TransferEndDate", model.AssetTransferEndModel.TransferEndDate),
               new SqlParameter("@Remarks", model.AssetTransferEndModel.Remarks),
               new SqlParameter("@RecordStatus", 3),
               new SqlParameter("@UserID", model.UserInfo.UserID),
               new SqlParameter("@Status", model.AssetTransferEndModel.TransferStatus),
               new SqlParameter("@CheckList", dataTable),
               new SqlParameter("@GstMechanism", model.AssetTransferEndModel.GstMechanism),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Transfer.Complete]", parameters))
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
                    DocumentNumber = dr.GetString("TargetDocumentNumber"),
                    TransferEndDate = dr.GetDateTime("TransferEndDate"),
                },
                Remarks = dr.GetString("Remarks"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
                GstMechanism = dr.GetString("GstMechanism"),
                Asset = new AssetModel()
                {
                    AssetID = dr.GetInt32("AssetID"),
                    Description = dr.GetString("Description"),
                    AssetCode = dr.GetString("AssetCode"),
                },
                MID = dr.GetInt32("MID"),
                DocumentNumber = dr.GetString("OriginDocumentNumber"),
                AuthLevelId = dr.GetInt32("AuthlevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
            };
        }

        IEnumerable<AssetTransferItemModel> IAssetTransferService.GetAssets(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetAssets"),
               new SqlParameter("@ID", ID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Transfer.Select]", parameters))
            {
                yield return new AssetTransferItemModel()
                {
                    ID = dr.GetInt32("ID"),
                    AssetID = dr.GetInt32("AssetID"),
                    BookValue = dr.GetDecimal("BookValue"),
                    TaxableValue = dr.GetDecimal("TaxableValue"),
                    TaxRate = dr.GetDecimal("TaxRate"),
                    TaxValue = dr.GetDecimal("TaxValue"),
                    Description = dr.GetString("Description"),
                    AssetStatus = dr.GetString("AssetStatus")
                };
            }
        }

        public int ApproveOutgoing(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetTransferID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Asset.Transfer.Initiate.Approve]", parameters);
        }

        public int ApproveIncoming(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetTransferEndID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Asset.Transfer.Complete.Approve]", parameters);
        }
    }
}