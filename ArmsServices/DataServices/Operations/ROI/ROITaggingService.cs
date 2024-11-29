using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ArmsModels.BaseModels;
using System.Runtime.CompilerServices;


namespace ArmsServices.DataServices
{
    public class ROITaggingService : IROITaggingService
    {
        IDbService Iservice;

        public ROITaggingService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public ROITaggingModel Update(ROITaggingModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@Wheels", model.Wheels),
               new SqlParameter("@BodyType", model.BodyType),
               new SqlParameter("@OrderID", model.Order.OrderID),
               new SqlParameter("@TaggingAmount", model.TaggingAmount),
               new SqlParameter("@LoadingAmount", model.LoadingAmount),
               new SqlParameter("@FromDate", model.FromDate),
               new SqlParameter("@ToDate", model.ToDate),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.Tagging.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<ROITaggingModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", 0),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.Tagging.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public ROITaggingModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
            };
            ROITaggingModel model = new ROITaggingModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.Tagging.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private ROITaggingModel GetModel(IDataRecord dr)
        {
            return new ROITaggingModel
            {
                ID = dr.GetInt32("ID"),
                Wheels = dr.GetByte("Wheels"),
                BodyType = dr.GetString("BodyType"),
                Order = new OrderModel
                {
                    OrderID = dr.GetInt32("OrderID"),
                    OrderName = dr.GetString("OrderName")
                },
                TaggingAmount = dr.GetDecimal("TaggingAmount"),
                LoadingAmount = dr.GetDecimal("LoadingAmount"),
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
