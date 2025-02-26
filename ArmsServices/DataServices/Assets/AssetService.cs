using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    // Service class for managing assets
    public class AssetService : IAssetService
    {
        IDbService Iservice;
        IAssetPostingGroupService _asset;

        public AssetService(IDbService iservice, IAssetPostingGroupService asset)
        {
            Iservice = iservice;
            _asset = asset;
        }

        // Retrieves a list of all assets
        public IEnumerable<AssetModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Removes a photo associated with an asset and returns the number of affected rows
        public int RemovePhoto(AssetModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetID", model.AssetID),
               new SqlParameter("@Images", model.Images),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Asset.RemovePhoto]", parameters);
        }

        // Helper method to map data from the database to an AssetModel
        private AssetModel GetModel(IDataRecord dr)
        {
            return new AssetModel
            {
                AssetID = dr.GetInt32("AssetID"),
                BranchID = dr.GetInt32("BranchID"),
                AssetClass = new()
                {
                    AssetClassID = dr.GetInt32("AssetClassID"),
                    AssetClassName = dr.GetString("AssetClassName"),
                },
                SubClass = new()
                {
                    ID = dr.GetInt32("ID"),
                    AssetSubClassID = dr.GetInt32("SubAssetClassID"),
                    AssetSubclass = dr.GetString("AssetSubClassName"),
                    AssetClassID = dr.GetInt32("AssetClassID"),
                },
                AssetCode = dr.GetString("AssetCode"),
                IsComplex = dr.GetBoolean("IsComplex"),
                ParentAssetID = dr.GetInt32("ParentAssetID"),
                TotalValue = dr.GetDecimal("TotalValue"),
                Scrap = dr.GetBoolean("Scrap"),
                BookValue = dr.GetDecimal("BookValue"),
                DepreciationBookCode = dr.GetString("DepreciationBookCode"),
                DepreciationEndingDate = dr.GetDateTime("DepreciationEndingDate"),
                DepreciationStartingDate = dr.GetDateTime("DepreciationStartingDate"),
                DepreciationMethod = dr.GetString("DepreciationMethod"),
                Description = dr.GetString("Description"),
                CurrentValue = dr.GetDecimal("CurrentValue"),
                GstRateID = dr.GetInt32("GstRateID"),
                GstMechanism = dr.GetString("GstMechanism"),
                HsnCode = dr.GetString("HsnCode"),
                NatureOfAsset = dr.GetString("NatureOfAsset"),
                ProjectedDisposalDate = dr.GetDateTime("ProjectedDisposalDate"),
                RateOfDepreciation = dr.GetDecimal("RateOfDepreciation"),
                SalvageValue = dr.GetDecimal("SalvageValue"),
                SerialNumber = dr.GetString("SerialNumber"),
                SpanOfYear = dr.GetDecimal("SpanOfYear"),
                Status = dr.GetString("Status"),
                Images = dr.GetString("ImagePath"),//.Split(";").ToList(),
                //ImagePath = dr.GetString("ImagePath").Split(";").ToList(),
                VendorInfo = new()
                {
                    PartyID = dr.GetInt32("PartyID"),
                    TradeName = dr.GetString("TradeName"),
                },
                WarrentyDate = dr.GetDateTime("WarrentyDate"),
                GSTValue = dr.GetDecimal("GSTValue"),
                GetAccountRuleDefinition = dr.GetInt32("AccountDef"),
                AccountName = dr.GetString("AccountName"),
                CoaID = dr.GetInt32("CoaID"),
                TaxRate = dr.GetDecimal("TaxRate"),
                AssetStatus = dr.GetString("AssetStatus"),
                IsSold = dr.GetBoolean("IsSold"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        // Deletes an asset by its ID and records the user who performed the deletion
        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Asset.Delete]", parameters);
        }

        // Updates the status of an asset and returns the updated model
        public AssetModel UpdateAsset(AssetModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetID", model.AssetID),
               new SqlParameter("@IsComplex", model.IsComplex),
               new SqlParameter("@ParentAssetID", model.ParentAssetID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@AssetClassID", model.AssetClass.AssetClassID),
               new SqlParameter("@SubAssetClassID", model.SubClass.ID),
               new SqlParameter("@AssetCode", model.AssetCode),
               new SqlParameter("@BookValue", model.BookValue),
               new SqlParameter("@CurrentValue", model.CurrentValue),
               new SqlParameter("@TotalValue", model.TotalValue),
               new SqlParameter("@Scrap", model.Scrap),
               new SqlParameter("@DepreciationBookCode", model.DepreciationBookCode),
               new SqlParameter("@DepreciableValue", model.DepreciableValue),
               new SqlParameter("@DepreciationEndingDate", model.DepreciationEndingDate),
               new SqlParameter("@DepreciationStartingDate", model.DepreciationStartingDate),
               new SqlParameter("@DepreciationMethod", model.DepreciationMethod),
               new SqlParameter("@Description", model.Description),
               new SqlParameter("@GstRateID", model.GstRateID),
               new SqlParameter("@GstMechanism", model.GstMechanism),
               new SqlParameter("@HsnCode", model.HsnCode),
               new SqlParameter("@NatureOfAsset", model.NatureOfAsset),
               new SqlParameter("@ProjectedDisposalDate", model.ProjectedDisposalDate),
               new SqlParameter("@RateOfDepreciation", model.RateOfDepreciation),
               new SqlParameter("@SalvageValue", model.SalvageValue),
               new SqlParameter("@SerialNumber", model.SerialNumber),
               new SqlParameter("@SpanOfYear", model.SpanOfYear),
               new SqlParameter("@Status", model.Status),
               new SqlParameter("@PartyID", model.VendorInfo?.PartyID),
               new SqlParameter("@WarrentyDate", model.WarrentyDate),
               new SqlParameter("@ImagePath", string.Join(";",model.ImagePath)),
               new SqlParameter("@UserID", model.UserInfo.UserID),
               new SqlParameter("@AccountDef", model.GetAccountRuleDefinition),
               new SqlParameter("AssetStatus", model.AssetStatus)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Update]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Moves an asset to a new parent asset and returns the number of affected rows
        public AssetModel UpdateAssetStatus(AssetModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetID", model.AssetID)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.AssetStatus.Update]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Moves an asset to a new parent asset and returns the number of affected rows
        public int MoveAsset(int? AssetID, int? ParentAssetID, string Mode, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetID", AssetID),
               new SqlParameter("@ParentAssetID", ParentAssetID),
               new SqlParameter("@Mode", Mode),// Attach,Detach
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Asset.Attach]", parameters);
        }

        // Retrieves an asset by its ID
        public AssetModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Retrieves a list of assets associated with a specific branch
        public IEnumerable<AssetModel> SelectByBranch(int BranchID, bool Scrap, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByBranch"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@Scrap", Scrap),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Retrieves a list of assets associated with a specific subclass
        public IEnumerable<AssetModel> SelectBySubClass(int BranchID, int? SubClassID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "BySubClass"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@SubClassID", SubClassID)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Retrieves a list of assets attached to a specific asset
        public IEnumerable<AssetModel> GetAttachedAssets(int? AssetID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetID", AssetID)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Child.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Retrieves a list of linked assets on a truck
        public IEnumerable<AssetModel> SelectLinkedAssetsOnTruck()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "LinkedAssets")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Updates the status of an asset and returns the status update ID
        public int? UpdateStatus(AssetStatusUpdateModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@StatusUpdateID", model.StatusUpdateID),
               new SqlParameter("@Amount", model.Amount),
               new SqlParameter("@AssetID", model.AssetID),
               new SqlParameter("@Status", model.Status),
               new SqlParameter("@StatusDate", model.StatusDate),
               new SqlParameter("@AssetID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.AssetStatus.Update]", parameters))
            {
                return dr.GetInt32("StatusUpdateID");
            }
            return null;
        }

        // Retrieves the current status of an asset
        public AssetStatusUpdateModel GetCurrentStatus(int? AssetID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetCurrent"),
               new SqlParameter("@AssetID", AssetID)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.AssetStatus.Select]", parameters))
            {
                return new AssetStatusUpdateModel()
                {
                    Amount = dr.GetDecimal("Amount"),
                    AssetID = dr.GetInt32("AssetID"),
                    Status = dr.GetString("Status"),
                    StatusDate = dr.GetDateTime("StatusDate"),
                    StatusUpdateID = dr.GetInt32("StatusUpdateID"),
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

        // Retrieves the status history of an asset
        public IEnumerable<AssetStatusUpdateModel> GetStatusHistory(int? AssetID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetID", AssetID)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[rptAssetStatusHistory]", parameters))
            {
                yield return new AssetStatusUpdateModel()
                {
                    Amount = dr.GetDecimal("BookValue"),
                    AssetID = dr.GetInt32("AssetID"),
                    Status = dr.GetString("AssetStatus"),
                    StatusDate = dr.GetDateTime("StatusDate"),
                    StatusUpdateID = dr.GetInt32("StatusUpdateID"),
                };
            }
        }

        // Marks an asset as scrapped and returns the number of affected rows
        public int? Scrap(int? AssetID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetID", AssetID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Asset.Scrap]", parameters);
        }

        // Clears the list of assets
        public void ClearAssets()
        {
            if (assets != null)
            {
                assets.Clear();
            }
        }

        private List<AssetModel> assets = new(); // List to hold assets

        // Retrieves a hierarchical view of assets
        public List<AssetViewModel> GetAssetView(int BranchID, int? ParentID = null, int? NumberOfRecords = 1000, string searchTerm = "")
        {
            if (assets.Count == 0)
            {
                assets = SelectByBranch(BranchID, false, NumberOfRecords, searchTerm).ToList();
            }
            List<AssetViewModel> view = new List<AssetViewModel>();
            foreach (var item in assets.Where(x => x.ParentAssetID == ParentID))
            {
                view.Add(new AssetViewModel()
                {
                    Parent = item,
                    Children = GetAssetView(BranchID, item.AssetID, null, "")
                });
            }
            return view;
        }

        // Retrieves the capitalization COA ID for a given asset
        public int? GetCapitalizationCoaID(int? AssetID)
        {
            return _asset.GetPostingGroup(AssetID).Capitalization.CoaID;
        }

        // Retrieves the CWIP COA ID for a given asset
        public int? GetCWIPCoaID(int? AssetID)
        {
            return _asset.GetPostingGroup(AssetID).CWIP.CoaID;
        }

        // Retrieves the depreciation COA ID for a given asset
        public int? GetDepreciationCoaID(int? AssetID)
        {
            return _asset.GetPostingGroup(AssetID).Depreciation.CoaID;
        }

        // Retrieves the accumulated depreciation COA ID for a given asset
        public int? GetAccumulatedDepreciationCoaID(int? AssetID)
        {
            return _asset.GetPostingGroup(AssetID).AccummulatedDepreciation.CoaID;
        }

        // Retrieves the revaluation COA ID for a given asset
        public int? GetRevaluationCoaID(int? AssetID)
        {
            return _asset.GetPostingGroup(AssetID).Revaluation.CoaID;
        }

        // Retrieves the revaluation reserve COA ID for a given asset
        public int? GetRevaluationReserveCoaID(int? AssetID)
        {
            return _asset.GetPostingGroup(AssetID).RevaluationReserve.CoaID;
        }

        // Retrieves an asset by its truck ID
        public AssetModel SelectByTruckID(int? TruckID)
        {
            ITruckService truckService = new TruckService(Iservice, null);
            var truck = truckService.SelectByID(TruckID);
            return SelectByID(truck.AssetID);
        }

        // Retrieves a list of assets based on various criteria
        public IEnumerable<AssetModel> GetAssetList(int BranchID, int? ParentID, int? NumberOfRecords, string searchTerm)
        {
            bool Scrap = false;
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByBranchPO"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@Scrap", Scrap),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Retrieves a list of non-invoiced assets based on various criteria
        public IEnumerable<AssetModel> GetAssetListNonInvoiced(int BranchID, int? ParentID, int? NumberOfRecords, string searchTerm)
        {
            bool Scrap = false;
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByBranchPONonInv"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@Scrap", Scrap),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Retrieves a purchase order by its ID
        public AssetPOModel SelectPOByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetID", ID),
               new SqlParameter("@Operation", "POByID")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Select]", parameters))
            {
                return GetModelPO(dr);
            }
            return null;
        }

        // Retrieves a list of account rule definitions
        public IEnumerable<AccountRuleDefModel> GetAccountRuleDefinition()
        {
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Assets.AccountDef.Select]", null))
            {
                yield return new AccountRuleDefModel()
                {
                    ID = dr.GetInt32("ID"),
                    Title = dr.GetString("Title"),
                    CapitalizationID = dr.GetInt32("CapitalizationID"),
                    CWIPID = dr.GetInt32("CWIPID")
                };
            }
        }

        // Helper method to map data from the database to an AssetPOModel
        private AssetPOModel GetModelPO(IDataRecord dr)
        {
            return new AssetPOModel
            {
                AssetID = dr.GetInt32("AssetID"),
                BranchID = dr.GetInt32("BranchID"),
                AssetCode = dr.GetString("AssetCode"),
                BookValue = dr.GetDecimal("BookValue"),
                Description = dr.GetString("Description"),
                GstMechanism = dr.GetString("GstMechanism"),
                GSTValue = dr.GetDecimal("GSTValue"),
                AccountName = dr.GetString("AccountName"),
                CoaID = dr.GetInt32("CoaID"),
                TaxRate = dr.GetDecimal("TaxRate"),
                AssetStatus = dr.GetString("AssetStatus"),
            };
        }
    }
}