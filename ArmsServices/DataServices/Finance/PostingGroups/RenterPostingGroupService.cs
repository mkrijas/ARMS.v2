using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class RenterPostingGroupService : IRenterPostingGroupService
    {
        IDbService Iservice;

        public RenterPostingGroupService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public RenterPostingGroupModel GetPostingGroup(int? RenterID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", RenterID),
               new SqlParameter("@Operation", "ByParty"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.Rent.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.PostingGroup.Rent.Delete]", parameters);
        }


        public IEnumerable<RenterPostingGroupModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.Rent.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public RenterPostingGroupModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@Operation", "ByID"),
            };
            RenterPostingGroupModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.Rent.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public RenterPostingGroupModel Update(RenterPostingGroupModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.RenterPostingGroupID),
               new SqlParameter("@Rent", model.Rent.CoaID),
               new SqlParameter("@Other", model.Other.CoaID),
               new SqlParameter("@Deposit", model.Deposit.CoaID),
               new SqlParameter("@Title", model.Title),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.Rent.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private RenterPostingGroupModel GetModel(IDataRecord dr)
        {
            return new RenterPostingGroupModel
            {
                RenterPostingGroupID = dr.GetInt32("ID"),
                Title = dr.GetString("Title"),
                Rent = new ChartOfAccountModel()
                {
                    CoaID = dr.GetInt32("Rent"),
                    AccountCode = dr.GetString("RentCoaCode"),
                    AccountName = dr.GetString("RentCoa"),
                },
                Other = new ChartOfAccountModel()
                {
                    CoaID = dr.GetInt32("Other"),
                    AccountCode = dr.GetString("OtherCoaCode"),
                    AccountName = dr.GetString("OtherCoa"),
                },
                Deposit = new ChartOfAccountModel()
                {
                    CoaID = dr.GetInt32("Deposit"),
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