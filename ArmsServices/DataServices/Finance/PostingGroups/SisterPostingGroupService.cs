using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class SisterPostingGroupService : ISisterPostingGroupService
    {
        IDbService Iservice;

        public SisterPostingGroupService(IDbService iservice)
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
            return Iservice.ExecuteNonQuery("[usp.Finance.PostingGroup.SisterConcern.Delete]", parameters);
        }

        public SisterPostingGroupModel GetPostingGroup(int? SisterID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", SisterID),
               new SqlParameter("@Operation", "ByParty"),
            };            
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.SisterConcern.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        public IEnumerable<SisterPostingGroupModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.SisterConcern.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public SisterPostingGroupModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@Operation", "ByID"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.SisterConcern.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        public SisterPostingGroupModel Update(SisterPostingGroupModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.SisterPostingGroupID),
               new SqlParameter("@Trade", model.Trade.CoaID),
               new SqlParameter("@PrePayment", model.PrePayment.CoaID),
               new SqlParameter("@Deposit", model.Deposit.CoaID),
               new SqlParameter("@Title", model.Title),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.SisterConcern.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private SisterPostingGroupModel GetModel(IDataRecord dr)
        {
            return new SisterPostingGroupModel
            {
                SisterPostingGroupID = dr.GetInt32("ID"),
                Title = dr.GetString("Title"),
                Trade = new ChartOfAccountModel()
                {
                    CoaID = dr.GetInt32("Trade"),
                    ParentID = dr.GetInt32("TradeParentID"),
                    AccountCode = dr.GetString("TradeCoaCode"),
                    AccountName = dr.GetString("TradeCoa"),
                },
                PrePayment = new ChartOfAccountModel()
                {
                    CoaID = dr.GetInt32("PrePayment"),
                    ParentID = dr.GetInt32("PrePaymentParentID"),
                    AccountCode = dr.GetString("PrePaymentCoaCode"),
                    AccountName = dr.GetString("PrePaymentCoa"),
                },
                Deposit = new ChartOfAccountModel()
                {
                    CoaID = dr.GetInt32("Deposit"),
                    ParentID = dr.GetInt32("DepositParentID"),
                    AccountCode = dr.GetString("DepositCoaCode"),
                    AccountName = dr.GetString("DepositCoa"),
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