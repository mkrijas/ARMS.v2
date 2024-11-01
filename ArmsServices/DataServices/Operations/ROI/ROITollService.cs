using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class ROITollService : IROITollService
    {
        IDbService Iservice;

        public ROITollService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public ROITollModel Update(ROITollModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@Wheels", model.Wheels),
               new SqlParameter("@RouteID", model.Route.RouteID),
               new SqlParameter("@Toll", model.Toll),
               new SqlParameter("@FromDate", model.FromDate),
               new SqlParameter("@ToDate", model.ToDate),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.Toll.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<ROITollModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", 0),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.Toll.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public ROITollModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
            };
            ROITollModel model = new ROITollModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.Toll.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private ROITollModel GetModel(IDataRecord dr)
        {
            return new ROITollModel
            {
                ID = dr.GetInt32("ID"),
                Wheels = dr.GetByte("Wheels"),
                Route = new RouteModel
                {
                    RouteID = dr.GetInt32("RouteID"),
                    RouteName = dr.GetString("RouteName")
                },
                Toll = dr.GetDecimal("Toll"),
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
