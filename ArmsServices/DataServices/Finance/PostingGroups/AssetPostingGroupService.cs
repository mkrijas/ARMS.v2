using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class AssetPostingGroupService : IAssetPostingGroupService
    {
        IDbService Iservice;

        public AssetPostingGroupService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.PostingGroup.Asset.Delete]", parameters);
        }

        public AssetPostingGroupModel GetPostingGroup(int? AssetID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", AssetID),
               new SqlParameter("@Operation", "ByAsset"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.Asset.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        public IEnumerable<AssetPostingGroupModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.Asset.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public AssetPostingGroupModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@Operation", "ByID"),
            };            
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.Asset.Select]", parameters))
            {
               return  GetModel(dr);
            }
            return null;
        }

        public AssetPostingGroupModel Update(AssetPostingGroupModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@CapitalizationID", model.Capitalization.CoaID),
               new SqlParameter("@CWIPID", model.CWIP.CoaID),
               new SqlParameter("@DepreciationID", model.AccummulatedDepreciation.CoaID),
               new SqlParameter("@AccummulatedDepreciationID", model.Depreciation.CoaID),
               new SqlParameter("@RevaluationID", model.Revaluation.CoaID),
               new SqlParameter("@RevaluationReserveID", model.RevaluationReserve.CoaID),
               new SqlParameter("@Title", model.Title),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.Asset.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private AssetPostingGroupModel GetModel(IDataRecord dr)
        {
            return new AssetPostingGroupModel
            {
                ID = dr.GetInt32("ID"),
                Title = dr.GetString("Title"),
                Capitalization = new ChartOfAccountModel()
                {
                    CoaID = dr.GetInt32("CapitalizationID"),
                    AccountName = dr.GetString("CapitalizationCoa"),
                },
                CWIP = new ChartOfAccountModel()
                {
                    CoaID = dr.GetInt32("CWIPID"),
                    AccountName = dr.GetString("CWIPCoa"),
                },
                Depreciation = new ChartOfAccountModel()
                {
                    CoaID = dr.GetInt32("DepreciationID"),
                    AccountName = dr.GetString("DepreciationCoa"),
                },
                AccummulatedDepreciation = new ChartOfAccountModel()
                {
                    CoaID = dr.GetInt32("AccummulatedDepreciationID"),
                    AccountName = dr.GetString("AccummulatedDepreciationCoa"),
                },
                Revaluation = new ChartOfAccountModel()
                {
                    CoaID = dr.GetInt32("RevaluationID"),
                    AccountName = dr.GetString("RevaluationCoa"),
                },
                RevaluationReserve = new ChartOfAccountModel()
                {
                    CoaID = dr.GetInt32("RevaluationReserveID"),
                    AccountName = dr.GetString("RevaluationReserveCoa"),
                },
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