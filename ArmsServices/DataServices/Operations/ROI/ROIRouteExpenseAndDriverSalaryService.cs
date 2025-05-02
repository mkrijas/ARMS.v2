using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class ROIRouteExpenseAndDriverSalaryService : IROIRouteExpenseAndDriverSalaryService
    {
        IDbService Iservice;

        public ROIRouteExpenseAndDriverSalaryService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public ROIRouteExpenseAndDriverSalaryModel Update(ROIRouteExpenseAndDriverSalaryModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@RouteExpense", model.RouteExpense),
               new SqlParameter("@DriverSalary", model.DriverSalary),
               new SqlParameter("@FromDate", model.FromDate),
               new SqlParameter("@ToDate", model.ToDate),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.RouteExpenseAndDriverSalary.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<ROIRouteExpenseAndDriverSalaryModel> Select(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.RouteExpenseAndDriverSalary.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        IEnumerable<ROIRouteExpenseAndDriverSalaryModel> IROIRouteExpenseAndDriverSalaryService.SelectByBranch(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", ID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.RouteExpenseAndDriverSalary.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        private ROIRouteExpenseAndDriverSalaryModel GetModel(IDataRecord dr)
        {
            return new ROIRouteExpenseAndDriverSalaryModel
            {
                ID = dr.GetInt32("ID"),
                BranchID = dr.GetInt32("BranchID"),
                RouteExpense = dr.GetDecimal("RouteExpense"),
                DriverSalary = dr.GetDecimal("DriverSalary"),
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
