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
               new SqlParameter("@Wheels", model.Wheels),
               new SqlParameter("@BSType", model.BSType.BSType),
               new SqlParameter("@BodyType", model.BodyType),
               new SqlParameter("@BranchAdmin", model.BranchAdmin),
               new SqlParameter("@HOAdmin", model.HOAdmin),
               new SqlParameter("@Tax", model.Tax),
               new SqlParameter("@Maintenance", model.Maintenance),
               new SqlParameter("@Tyre", model.Tyre),
               new SqlParameter("@TaxAndInsurance", model.TaxAndInsurance),
               new SqlParameter("@FC", model.FC),
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
                Wheels = dr.GetByte("Wheels"),
                BSType = new Core.BaseModels.Operations.ROI.ROITonnageModel
                {
                    BSType = dr.GetString("BSType"),
                },
                BodyType = dr.GetString("BodyType"),
                BranchAdmin = dr.GetDecimal("BranchAdmin"),
                HOAdmin = dr.GetDecimal("HOAdmin"),
                Tax = dr.GetDecimal("Tax"),
                Maintenance = dr.GetDecimal("Maintenance"),
                Tyre = dr.GetDecimal("Tyre"),
                TaxAndInsurance = dr.GetDecimal("TaxAndInsurance"),
                FC = dr.GetDecimal("FC"),
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
