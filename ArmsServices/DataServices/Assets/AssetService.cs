using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public interface IAssetService
    {
        AssetHolderModel UpdateSimpleAsset(AssetModel model,AssetHolderModel Holder);
        AssetHolderModel UpdateComplexAsset(AssetHolderModel model);
        int MoveAsset(int? AssetID,int? AssetHolderID,string Mode, string UserID);
        AssetHolderModel SelectByID(int? ID);
        IEnumerable<AssetHolderModel> SelectByBranch(int BranchID);
        IEnumerable<AssetModel> GetAssets(int? AssetHolderID);
        int? UpdateStatus(AssetStatusUpdateModel model); 
        AssetStatusUpdateModel GetCurrentStatus(int? AssetID);
        IEnumerable<AssetStatusUpdateModel> GetStatusHistory(int? AssetID);
    }

    public class AssetService : IAssetService
    {
        IDbService Iservice;

        public AssetService(IDbService iservice)
        {
            Iservice = iservice;
        }       

        public IEnumerable<AssetHolderModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Holder.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }      
       

        private AssetHolderModel GetModel(IDataRecord dr)
        {
            return new AssetHolderModel
            {
                AssetHolderID = dr.GetInt32("AssetHolderID"),
                Description = dr.GetString("Description"),
                BranchID = dr.GetInt32("BranchID"),
                //UserInfo = new ArmsModels.SharedModels.UserInfoModel
                //{
                //    RecordStatus = dr.GetByte("RecordStatus"),
                //    TimeStampField = dr.GetDateTime("TimeStamp"),
                //    UserID = dr.GetString("UserID"),
                //},
            };
        }
        private AssetModel GetAsset(IDataRecord dr)
        {
            return new AssetModel
            {
                AssetID = dr.GetInt32("AssetID"),
                BranchID = dr.GetInt32("BranchID"),
               // AccountTransactionID =dr.GetInt32("AccountTransactionID"),
                AssetClass = new()
                {
                    AssetClassID = dr.GetInt32("AssetClassID"),
                    AssetClassName = dr.GetString("AssetClassName"),                    
                },
                SubClass = new()
                {
                    AssetClassID = dr.GetInt32("AssetSubClassID"),
                    AssetClassName = dr.GetString("AssetSubClassName"),
                },
                AssetCode = dr.GetString("AssetCode"),
                BookValue = dr.GetDecimal("BookValue"),
                DepreciationBookCode = dr.GetString("DepreciationBookCode"),
                DepreciationEndingDate = dr.GetDateTime("DepreciationEndingDate"),
                DepreciationStartingDate =  dr.GetDateTime("DepreciationStartingDate"),
                DepreciationMethod = dr.GetString("DepreciationMethod"),
                Description = dr.GetString("Description"),
                GstRateID = dr.GetInt32("GstRateID"),
                HsnCode = dr.GetString("HsnCode"),
                NatureOfAsset = dr.GetString("NatureOfAsset"),
                ProjectedDisposalDate = dr.GetDateTime("ProjectedDisposalDate"),
                RateOfDepreciation = dr.GetByte("RateOfDepreciation"),
                SalvageValue = dr.GetByte("SalvageValue"),
                SerialNumber = dr.GetString("SerialNumber"),
                SpanOfYear = dr.GetInt32("SpanOfYear"),
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




        public AssetHolderModel UpdateSimpleAsset(AssetModel model, AssetHolderModel Holder)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@AssetHolderID", Holder.AssetHolderID),
                new SqlParameter("@Description", Holder.Description),
                new SqlParameter("@BranchID", Holder.BranchID),
               new SqlParameter("@AssetID", model.AssetID),
               new SqlParameter("@BranchID", model.BranchID),
             //  new SqlParameter("@AccountTransactionID", model.AccountTransactionID),
               new SqlParameter("@AssetClassID", model.AssetClass.AssetClassID),
               new SqlParameter("@SubClassID", model.SubClass.AssetClassID),
               new SqlParameter("@AssetCode", model.AssetCode),
               new SqlParameter("@BookValue", model.BookValue),
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

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Simple.Update]", parameters))
            {
                Holder = GetModel(dr);
            }
            return Holder;
        }

        public AssetHolderModel UpdateComplexAsset(AssetHolderModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetHolderID", model.AssetHolderID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@AssetTime", model.Description),               
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Complex.Update]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        public int MoveAsset(int? AssetID, int? AssetHolderID,string Mode,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetID", AssetID),
               new SqlParameter("@AssetHolderID", AssetHolderID),
               new SqlParameter("@Mode", Mode),// Attach,Detach
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Asset.Attach]", parameters);           
        }


        AssetHolderModel IAssetService.SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetHolderID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Holder.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        public IEnumerable<AssetHolderModel> SelectByBranch(int BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByBranch"),
               new SqlParameter("@BranchID", BranchID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Holder.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<AssetModel> GetAssets(int? AssetHolderID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByHolder"),
               new SqlParameter("@AssetHolderID", AssetHolderID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Tag.Select]", parameters))
            {
                yield return GetAsset(dr);
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
               new SqlParameter("@AssetHolderID", model.UserInfo.UserID),
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
    }
}
   

     