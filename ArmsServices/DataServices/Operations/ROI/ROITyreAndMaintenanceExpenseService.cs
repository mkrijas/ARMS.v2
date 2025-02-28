using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ArmsModels.BaseModels;
using System.Reflection.PortableExecutable;


namespace ArmsServices.DataServices
{
    public class ROITyreAndMaintenanceExpenseService : IROITyreAndMaintenanceExpenseService
    {
        IDbService Iservice;

        public ROITyreAndMaintenanceExpenseService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public ROITyreAndMaintenanceExpenseModel Update(ROITyreAndMaintenanceExpenseModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@TruckTypeID", model.TruckType.TruckTypeID),
               new SqlParameter("@BsType", model.BsType),
               new SqlParameter("@Maintenance", model.Maintenance),
               new SqlParameter("@Tyre", model.Tyre),
               new SqlParameter("@FromDate", model.FromDate),
               new SqlParameter("@ToDate", model.ToDate),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.TyreAndMaintenanceExpense.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<ROITyreAndMaintenanceExpenseModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", 0),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.TyreAndMaintenanceExpense.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public ROITyreAndMaintenanceExpenseModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
            };
            ROITyreAndMaintenanceExpenseModel model = new ROITyreAndMaintenanceExpenseModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.TyreAndMaintenanceExpense.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private ROITyreAndMaintenanceExpenseModel GetModel(IDataRecord dr)
        {
            return new ROITyreAndMaintenanceExpenseModel
            {
                ID = dr.GetInt32("ID"),
                TruckType = new TruckTypeModel
                {
                    TruckTypeID = dr.GetInt16("TruckTypeID"),
                    TruckType = dr.GetString("TruckType"),
                },
                BsType = dr.GetString("BsType"),
                Maintenance = dr.GetDecimal("Maintenance"),
                Tyre = dr.GetDecimal("Tyre"),
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
