using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class ROIMileageService : IROIMileageService
    {
        IDbService Iservice;

        public ROIMileageService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public ROIMileageModel Update(ROIMileageModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@Wheels", model.Wheels),
               new SqlParameter("@BSType", model.BSType),
               new SqlParameter("@BodyType", model.BodyType),
               new SqlParameter("@OrderID", model.Order.OrderID),
               new SqlParameter("@LoadingMTFrom", model.LoadingMTFrom),
               new SqlParameter("@LoadingMTTo", model.LoadingMTTo),
               new SqlParameter("@Mileage", model.Mileage),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.Mileage.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<ROIMileageModel> Select(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", 0),
               new SqlParameter("BranchID",BranchID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.Mileage.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public ROIMileageModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
            };
            ROIMileageModel model = new ROIMileageModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.Mileage.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private ROIMileageModel GetModel(IDataRecord dr)
        {
            return new ROIMileageModel
            {
                ID = dr.GetInt32("ID"),
                Wheels = dr.GetByte("Wheels"),
                BodyType = dr.GetString("BodyType"),
                BSType = dr.GetString("BSType"),
                Order = new OrderModel
                {
                    OrderID = dr.GetInt32("OrderID"),
                    OrderName = dr.GetString("OrderName")
                },
                LoadingMTFrom = dr.GetDecimal("LoadingMTFrom"),
                LoadingMTTo = dr.GetDecimal("LoadingMTTo"),
                Mileage = dr.GetDecimal("Mileage"),
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
