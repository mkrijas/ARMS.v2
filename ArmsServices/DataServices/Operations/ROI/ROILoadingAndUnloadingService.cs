using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class ROILoadingAndUnloadingService : IROILoadingAndUnloadingService
    {
        IDbService Iservice;

        public ROILoadingAndUnloadingService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public ROILoadingAndUnloadingModel Update(ROILoadingAndUnloadingModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@Wheels", model.Wheels),
               new SqlParameter("@BodyType", model.BodyType),
               new SqlParameter("@OrderID", model.Order.OrderID),
               new SqlParameter("@LoadingMTFrom", model.LoadingMTFrom),
               new SqlParameter("@LoadingMTTo", model.LoadingMTTo),
               new SqlParameter("@LoadingAndUnloading", model.LoadingAndUnloading),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.LoadingAndUnloading.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<ROILoadingAndUnloadingModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", 0),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.LoadingAndUnloading.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public ROILoadingAndUnloadingModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
            };
            ROILoadingAndUnloadingModel model = new ROILoadingAndUnloadingModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.LoadingAndUnloading.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private ROILoadingAndUnloadingModel GetModel(IDataRecord dr)
        {
            return new ROILoadingAndUnloadingModel
            {
                ID = dr.GetInt32("ID"),
                Wheels = dr.GetByte("Wheels"),
                BodyType = dr.GetString("BodyType"),
                Order = new OrderModel
                {
                    OrderID = dr.GetInt32("OrderID"),
                    OrderName = dr.GetString("OrderName")
                },
                LoadingMTFrom = dr.GetDecimal("LoadingMTFrom"),
                LoadingMTTo = dr.GetDecimal("LoadingMTTo"),
                LoadingAndUnloading = dr.GetDecimal("LoadingAndUnloading"),
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
