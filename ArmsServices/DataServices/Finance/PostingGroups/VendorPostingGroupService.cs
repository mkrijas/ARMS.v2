using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IVendorPostingGroupService
    {
        VendorPostingGroupModel Update(VendorPostingGroupModel model);
        VendorPostingGroupModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);
        IEnumerable<VendorPostingGroupModel> Select();
    }

    public class VendorPostingGroupService : IVendorPostingGroupService
    {
        IDbService Iservice;

        public VendorPostingGroupService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@VendorPostingGroupID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.PostingGroup.Vendor.Delete]", parameters);
        }


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

        public VendorPostingGroupModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@VendorPostingGroupID", ID),
               new SqlParameter("@Operation", "ByID"),
            };
            VendorPostingGroupModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.Vendor.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public VendorPostingGroupModel Update(VendorPostingGroupModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@VendorPostingGroupID", model.VendorPostingGroupID),
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

        private VendorPostingGroupModel GetModel(IDataRecord dr)
        {
            return new VendorPostingGroupModel
            {
                VendorPostingGroupID = dr.GetInt32("VendorPostingGroupID"),
                Title = dr.GetString("Title"),
                Payable = new ChartOfAccountModel()
                {
                    CoaID = dr.GetInt32("Payable"),
                    AccountName = dr.GetString("PayableCoa"),
                },
                PrePayment = new ChartOfAccountModel()
                {
                    CoaID = dr.GetInt32("PrePayment"),
                    AccountName = dr.GetString("PrePaymentCoa"),
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