using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class ROIDriverIncentiveService : IROIDriverIncentiveService
    {
        IDbService Iservice;

        public ROIDriverIncentiveService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public ROIDriverIncentiveModel Update(ROIDriverIncentiveModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@Wheels", model.Wheels),
               new SqlParameter("@BSType", model.BSType.BSType),
               new SqlParameter("@StateID", model.State.StateID),
               new SqlParameter("@OrderID", model.Order.OrderID),
               new SqlParameter("@LoadingMTFrom", model.LoadingMTFrom),
               new SqlParameter("@LoadingMTTo", model.LoadingMTTo),
               new SqlParameter("@Amount", model.Amount),
               new SqlParameter("@FromDate", model.FromDate),
               new SqlParameter("@ToDate", model.ToDate),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.DriverIncentive.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<ROIDriverIncentiveModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", 0),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.DriverIncentive.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public ROIDriverIncentiveModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
            };
            ROIDriverIncentiveModel model = new ROIDriverIncentiveModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.DriverIncentive.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private ROIDriverIncentiveModel GetModel(IDataRecord dr)
        {
            return new ROIDriverIncentiveModel
            {
                ID = dr.GetInt32("ID"),
                Wheels = dr.GetByte("Wheels"),
                BSType = new Core.BaseModels.Operations.ROI.ROITonnageModel
                {
                    BSType = dr.GetString("BSType"),
                },
                Order = new OrderModel
                {
                    OrderID = dr.GetInt32("OrderID"),
                    OrderName = dr.GetString("OrderName")
                },
                State = new StateModel
                {
                    StateID = dr.GetInt32("StateID"),
                    StateName = dr.GetString("StateName")
                },
                LoadingMTFrom = dr.GetDecimal("LoadingMTFrom"),
                LoadingMTTo = dr.GetDecimal("LoadingMTTo"),
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
