using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public class AssetService : IAssetService
    {
        IDbService Iservice;
        IAssetPostingGroupService _asset;

        public AssetService(IDbService iservice, IAssetPostingGroupService asset)
        {
            Iservice = iservice;
            _asset = asset;            
        }

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
                    AssetSubClassID = dr.GetInt32("AssetSubClassID"),
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
                DepreciationStartingDate =  dr.GetDateTime("DepreciationStartingDate"),
                DepreciationMethod = dr.GetString("DepreciationMethod"),
                Description = dr.GetString("Description"),
                CurrentValue = dr.GetDecimal("CurrentValue"),
                GstRateID = dr.GetInt32("GstRateID"),
                HsnCode = dr.GetString("HsnCode"),
                NatureOfAsset = dr.GetString("NatureOfAsset"),
                ProjectedDisposalDate = dr.GetDateTime("ProjectedDisposalDate"),
                RateOfDepreciation = dr.GetDecimal("RateOfDepreciation"),                
                SalvageValue = dr.GetDecimal("SalvageValue"),
                SerialNumber = dr.GetString("SerialNumber"),
                SpanOfYear = dr.GetDecimal("SpanOfYear"),
                Status = dr.GetString("Status"),               
                VendorInfo = new()
                {
                    PartyID = dr.GetInt32("PartyID"),
                    TradeName = dr.GetString("TradeName"),
                },
                WarrentyDate = dr.GetDateTime("WarrentyDate"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }




        public AssetModel UpdateAsset(AssetModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetID", model.AssetID),
               //new SqlParameter("@Description", model.Description),
               new SqlParameter("@IsComplex", model.IsComplex),
               new SqlParameter("@ParentAssetID", model.ParentAssetID),
               new SqlParameter("@BranchID", model.BranchID),
               //new SqlParameter("@AccountTransactionID", model.CurrentValue),
               new SqlParameter("@AssetClassID", model.AssetClass.AssetClassID),
               new SqlParameter("@SubAssetClassID", model.SubClass.AssetClassID),
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
               new SqlParameter("@HsnCode", model.HsnCode),
               new SqlParameter("@NatureOfAsset", model.NatureOfAsset),
               new SqlParameter("@ProjectedDisposalDate", model.ProjectedDisposalDate),
               new SqlParameter("@RateOfDepreciation", model.RateOfDepreciation),
               new SqlParameter("@SalvageValue", model.SalvageValue),
               new SqlParameter("@SerialNumber", model.SerialNumber),
               new SqlParameter("@SpanOfYear", model.SpanOfYear),
               new SqlParameter("@Status", model.Status),
               new SqlParameter("@PartyID", model.VendorInfo.PartyID),
               new SqlParameter("@WarrentyDate", model.WarrentyDate),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Update]", parameters))
            {
               return GetModel(dr);
            }
            return null;
        }       

        public int MoveAsset(int? AssetID, int? ParentAssetID,string Mode,string UserID)
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


        AssetModel IAssetService.SelectByID(int? ID)
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

        public IEnumerable<AssetModel> SelectByBranch(int BranchID,bool Scrap)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByBranch"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@Scrap", Scrap)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

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
        public IEnumerable<AssetModel> GetAttachedAssets(int? ParentAssetID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "BYParent"),
               new SqlParameter("@AssetID", ParentAssetID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

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


        public IEnumerable<AssetStatusUpdateModel> GetStatusHistory(int? AssetID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetHistory"),
               new SqlParameter("@AssetID", AssetID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.AssetStatus.Select]", parameters))
            {
                yield return new AssetStatusUpdateModel()
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
        }

        public int? Scrap(int? AssetID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetID", AssetID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Asset.Scrap]", parameters);
            //throw new NotImplementedException();
        }


        private List<AssetModel> assets = new();
        public List<AssetViewModel> GetAssetView(int BranchID,int? ParentID = null)
        {            
            if(assets.Count == 0)
            {
               assets = SelectByBranch(BranchID, false).ToList();
            }  
            List<AssetViewModel> view = new List<AssetViewModel>();

            foreach(var item in assets.Where(x => x.ParentAssetID == ParentID))
            {                
                    view.Add(new AssetViewModel() { 
                        Parent = item ,
                        Children = GetAssetView(BranchID,item.AssetID)
                    });               
            }
            return view;
        }

        public int? GetCapitalizationCoaID(int? AssetID)
        {
            return _asset.GetPostingGroup(AssetID).Capitalization.CoaID;
        }

        public int? GetCWIPCoaID(int? AssetID)
        {
            return _asset.GetPostingGroup(AssetID).CWIP.CoaID;
        }

        public int? GetDepreciationCoaID(int? AssetID)
        {
            return _asset.GetPostingGroup(AssetID).Depreciation.CoaID;
        }

        public int? GetAccumulatedDepreciationCoaID(int? AssetID)
        {
            return _asset.GetPostingGroup(AssetID).AccummulatedDepreciation.CoaID;
        }

        public int? GetRevaluationCoaID(int? AssetID)
        {
            return _asset.GetPostingGroup(AssetID).Revaluation.CoaID;
        }

        public int? GetRevaluationReserveCoaID(int? AssetID)
        {
            return _asset.GetPostingGroup(AssetID).RevaluationReserve.CoaID;
        }

      
    }
}
   

     