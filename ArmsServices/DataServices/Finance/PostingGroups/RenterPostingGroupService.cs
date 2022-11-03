using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IRenterPostingGroupService
    {
        RenterPostingGroupModel Update(RenterPostingGroupModel model);
        RenterPostingGroupModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);
        IEnumerable<RenterPostingGroupModel> Select();
    }

    public class RenterPostingGroupService : IRenterPostingGroupService
    {
        IDbService Iservice;

        public RenterPostingGroupService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RenterPostingGroupID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.PostingGroup.Renter.Delete]", parameters);
        }


        public IEnumerable<RenterPostingGroupModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.Renter.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public RenterPostingGroupModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RenterPostingGroupID", ID),
               new SqlParameter("@Operation", "ByID"),
            };
            RenterPostingGroupModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.Renter.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public RenterPostingGroupModel Update(RenterPostingGroupModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RenterPostingGroupID", model.RenterPostingGroupID),
               new SqlParameter("@Rent", model.Rent.CoaID),
               new SqlParameter("@Other", model.Other.CoaID),
               new SqlParameter("@Deposit", model.Deposit.CoaID),
               new SqlParameter("@Title", model.Title),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.Renter.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private RenterPostingGroupModel GetModel(IDataRecord dr)
        {
            return new RenterPostingGroupModel
            {
                RenterPostingGroupID = dr.GetInt32("RenterPostingGroupID"),
                Title = dr.GetString("Title"),
                Rent = new ChartOfAccountModel()
                {
                    CoaID = dr.GetInt32("Rent"),
                    AccountName = dr.GetString("RentCoa"),
                },
                Other = new ChartOfAccountModel()
                {
                    CoaID = dr.GetInt32("Other"),
                    AccountName = dr.GetString("OtherCoa"),
                },
                Deposit = new ChartOfAccountModel()
                {
                    CoaID = dr.GetInt32("Deposit"),
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