using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class ROILoadingChargesService : IROILoadingChargesService
    {
        IDbService Iservice;

        public ROILoadingChargesService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public ROILoadingChargesModel Update(ROILoadingChargesModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@Wheels", model.Wheels),
               new SqlParameter("@BodyType", model.BodyType),
               new SqlParameter("@OrderID", model.Order?.OrderID),
               new SqlParameter("@RateBasis", model.RateBasis),
               new SqlParameter("@LoadingMTFrom", model.LoadingMTFrom),
               new SqlParameter("@LoadingMTTo", model.LoadingMTTo),
               new SqlParameter("@Amount", model.Amount),
               new SqlParameter("@FromDate", model.FromDate),
               new SqlParameter("@ToDate", model.ToDate),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.LoadingCharges.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<ROILoadingChargesModel> Select(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", 0),
               new SqlParameter("BranchID",BranchID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.LoadingCharges.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public ROILoadingChargesModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
            };
            ROILoadingChargesModel model = new ROILoadingChargesModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.LoadingCharges.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private ROILoadingChargesModel GetModel(IDataRecord dr)
        {
            return new ROILoadingChargesModel
            {
                ID = dr.GetInt32("ID"),
                Wheels = dr.GetByte("Wheels"),
                BodyType = dr.GetString("BodyType"),
                Order = new OrderModel
                {
                    OrderID = dr.GetInt32("OrderID"),
                    OrderName = dr.GetString("OrderName")
                },
                RateBasis = dr.GetString("RateBasis"),
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
