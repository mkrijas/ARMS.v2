using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class ROIFixedExpenseService : IROIFixedExpenseService
    {
        IDbService Iservice;

        public ROIFixedExpenseService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public ROIFixedExpenseModel Update(ROIFixedExpenseModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@ExpenseName", model.ExpenseName),
               new SqlParameter("@Amount", model.Amount),
               new SqlParameter("@FromDate", model.FromDate),
               new SqlParameter("@ToDate", model.ToDate),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.FixedExpense.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<ROIFixedExpenseModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", 0),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.FixedExpense.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public ROIFixedExpenseModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
            };
            ROIFixedExpenseModel model = new ROIFixedExpenseModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.FixedExpense.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private ROIFixedExpenseModel GetModel(IDataRecord dr)
        {
            return new ROIFixedExpenseModel
            {
                ID = dr.GetInt32("ID"),
                ExpenseName = dr.GetString("ExpenseName"),
                Amount = dr.GetDecimal("Amount"),
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
