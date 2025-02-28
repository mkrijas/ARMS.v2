using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ArmsModels.BaseModels;
using System.Text.RegularExpressions;


namespace ArmsServices.DataServices
{
    public class ROITaxAndFCExpenseService : IROITaxAndFCExpenseService
    {
        IDbService Iservice;

        public ROITaxAndFCExpenseService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public ROITaxAndFCExpenseModel Update(ROITaxAndFCExpenseModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@TruckID", model.Truck.TruckID),
               new SqlParameter("@TaxAndInsurance", model.TaxAndInsurance),
               new SqlParameter("@FC", model.FC),
               new SqlParameter("@FromDate", model.FromDate),
               new SqlParameter("@ToDate", model.ToDate),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.TaxAndFCExpense.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<ROITaxAndFCExpenseModel> Select(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.TaxAndFCExpense.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public ROITaxAndFCExpenseModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
            };
            ROITaxAndFCExpenseModel model = new ROITaxAndFCExpenseModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.TaxAndFCExpense.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private ROITaxAndFCExpenseModel GetModel(IDataRecord dr)
        {
            return new ROITaxAndFCExpenseModel
            {
                ID = dr.GetInt32("ID"),
                Truck = new TruckModel
                {
                    TruckID = dr.GetInt32("TruckID"),
                    RegNo = dr.GetString("RegNo")
                },
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
