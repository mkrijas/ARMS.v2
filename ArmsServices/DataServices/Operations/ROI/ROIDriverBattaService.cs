using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class ROIDriverBattaService : IROIDriverBattaService
    {
        IDbService Iservice;

        public ROIDriverBattaService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public ROIDriverBattaModel Update(ROIDriverBattaModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@Wheels", model.Wheels),
               new SqlParameter("@BodyType", model.BodyType),
               new SqlParameter("@FromStdKM", model.FromStdKM),
               new SqlParameter("@ToStdKM", model.ToStdKM),
               new SqlParameter("@LoadingMTFrom", model.LoadingMTFrom),
               new SqlParameter("@LoadingMTTo", model.LoadingMTTo),
               new SqlParameter("@DriverBatta", model.DriverBatta),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.DriverBatta.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<ROIDriverBattaModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", 0),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.DriverBatta.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public ROIDriverBattaModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
            };
            ROIDriverBattaModel model = new ROIDriverBattaModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.DriverBatta.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private ROIDriverBattaModel GetModel(IDataRecord dr)
        {
            return new ROIDriverBattaModel
            {
                ID = dr.GetInt32("ID"),
                Wheels = dr.GetByte("Wheels"),
                BodyType = dr.GetString("BodyType"),
                FromStdKM = dr.GetDecimal("FromStdKM"),
                ToStdKM = dr.GetDecimal("ToStdKM"),
                LoadingMTFrom = dr.GetDecimal("LoadingMTFrom"),
                LoadingMTTo = dr.GetDecimal("LoadingMTTo"),
                DriverBatta = dr.GetDecimal("DriverBatta"),
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
