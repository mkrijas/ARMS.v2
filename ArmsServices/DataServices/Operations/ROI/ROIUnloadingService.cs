using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class ROIUnloadingService : IROIUnloadingService
    {
        IDbService Iservice;

        public ROIUnloadingService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public ROIUnloadingModel Update(ROIUnloadingModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@RouteID", model.Route.RouteID),
               new SqlParameter("@RateBasis", model.RateBasis),
               new SqlParameter("@Amount", model.Amount),
               new SqlParameter("@FromDate", model.FromDate),
               new SqlParameter("@ToDate", model.ToDate),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.Unloading.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<ROIUnloadingModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", 0),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.Unloading.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public ROIUnloadingModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
            };
            ROIUnloadingModel model = new ROIUnloadingModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.Unloading.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private ROIUnloadingModel GetModel(IDataRecord dr)
        {
            return new ROIUnloadingModel
            {
                ID = dr.GetInt32("ID"),
                Route = new RouteModel
                {
                    RouteID = dr.GetInt32("RouteID"),
                    RouteName = dr.GetString("RouteName")
                },
                RateBasis = dr.GetString("RateBasis"),
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
