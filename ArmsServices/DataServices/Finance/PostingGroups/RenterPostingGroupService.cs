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

        // Method to get a posting group by RenterID
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

        // Method to delete a posting group by ID
        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.PostingGroup.Rent.Delete]", parameters);
        }

        // Method to select and retrieve all RenterPostingGroupModel records
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

        // Method to select a posting group by its ID
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

        // Method to update an existing RenterPostingGroupModel record
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

        // Private method to convert an IDataRecord to a RenterPostingGroupModel
        private RenterPostingGroupModel GetModel(IDataRecord dr)
        {
            return new RenterPostingGroupModel
            {
                RenterPostingGroupID = dr.GetInt32("ID"),
                Title = dr.GetString("Title"),
                Rent = new ChartOfAccountModel()
                {
                    CoaID = dr.GetInt32("Rent"),
                    ParentID = dr.GetInt32("RentParentID"),
                    AccountCode = dr.GetString("RentCoaCode"),
                    AccountName = dr.GetString("RentCoa"),
                },
                Deposit = new ChartOfAccountModel()
                {
                    CoaID = dr.GetInt32("Deposit"),
                    ParentID = dr.GetInt32("DepositParentID"),
                    AccountCode = dr.GetString("DepositCoaCode"),
                    AccountName = dr.GetString("DepositCoa"),
                },
                Other = new ChartOfAccountModel()
                {
                    CoaID = dr.GetInt32("Other"),
                    ParentID = dr.GetInt32("OtherParentID"),
                    AccountCode = dr.GetString("OtherCoaCode"),
                    AccountName = dr.GetString("OtherCoa"),
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