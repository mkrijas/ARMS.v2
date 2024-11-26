using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class ROIAdminExpenseService : IROIAdminExpenseService
    {
        IDbService Iservice;

        public ROIAdminExpenseService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public ROIAdminExpenseModel Update(ROIAdminExpenseModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@BranchAdmin", model.BranchAdmin),
               new SqlParameter("@HOAdmin", model.HOAdmin),
               new SqlParameter("@FromDate", model.FromDate),
               new SqlParameter("@ToDate", model.ToDate),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.AdminExpense.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<ROIAdminExpenseModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", 0),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.AdminExpense.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public ROIAdminExpenseModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
            };
            ROIAdminExpenseModel model = new ROIAdminExpenseModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.AdminExpense.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private ROIAdminExpenseModel GetModel(IDataRecord dr)
        {
            return new ROIAdminExpenseModel
            {
                ID = dr.GetInt32("ID"),
                BranchAdmin = dr.GetDecimal("BranchAdmin"),
                HOAdmin = dr.GetDecimal("HOAdmin"),
                FromDate = dr.GetDateTime("FromDate"),
                ToDate = dr.GetDateTime("ToDate"),
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
