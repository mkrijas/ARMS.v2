using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class VendorPostingGroupService : IVendorPostingGroupService
    {
        IDbService Iservice;

        public VendorPostingGroupService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to delete a posting group by ID
        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.PostingGroup.Vendor.Delete]", parameters);
        }

        // Method to get a posting group by VendorID
        public VendorPostingGroupModel GetPostingGroup(int? VendorID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", VendorID),
               new SqlParameter("@Operation", "ByParty"),
            };            
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.Vendor.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Method to select and retrieve all VendorPostingGroupModel records
        public IEnumerable<VendorPostingGroupModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.Vendor.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select a posting group by its ID
        public VendorPostingGroupModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@Operation", "ByID"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.Vendor.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }
        
        // Method to update an existing VendorPostingGroupModel record
        public VendorPostingGroupModel Update(VendorPostingGroupModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.VendorPostingGroupID),
               new SqlParameter("@Payable", model.Payable.CoaID),
               new SqlParameter("@PrePayment", model.PrePayment.CoaID),
               new SqlParameter("@Deposit", model.Deposit.CoaID),
               new SqlParameter("@Title", model.Title),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.Vendor.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Private method to convert an IDataRecord to a VendorPostingGroupModel
        private VendorPostingGroupModel GetModel(IDataRecord dr)
        {
            return new VendorPostingGroupModel
            {
                VendorPostingGroupID = dr.GetInt32("ID"),
                Title = dr.GetString("Title"),
                Payable = new ChartOfAccountModel()
                {
                    CoaID = dr.GetInt32("Payable"),
                    ParentID = dr.GetInt32("PayableParentID"),
                    AccountCode = dr.GetString("PayableCoaCode"),
                    AccountName = dr.GetString("PayableCoa"),
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