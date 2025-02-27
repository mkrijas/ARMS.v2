using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class ROIUnloadingChargesService : IROIUnloadingChargesService
    {
        IDbService Iservice;

        public ROIUnloadingChargesService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public ROIUnloadingChargesModel Update(ROIUnloadingChargesModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@RouteID", model.Route.RouteID),
               new SqlParameter("@RateBasis", model.RateBasis),
               new SqlParameter("@Wheels", model.Wheels),
               new SqlParameter("@BodyType", model.BodyType),
               new SqlParameter("@Amount", model.Amount),
               new SqlParameter("@FromDate", model.FromDate),
               new SqlParameter("@ToDate", model.ToDate),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.UnloadingCharges.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<ROIUnloadingChargesModel> Select(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", 0),
               new SqlParameter("BranchID",BranchID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.UnloadingCharges.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public ROIUnloadingChargesModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
            };
            ROIUnloadingChargesModel model = new ROIUnloadingChargesModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.UnloadingCharges.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private ROIUnloadingChargesModel GetModel(IDataRecord dr)
        {
            return new ROIUnloadingChargesModel
            {
                ID = dr.GetInt32("ID"),
                Route = new RouteModel
                {
                    RouteID = dr.GetInt32("RouteID"),
                    RouteName = dr.GetString("RouteName")
                },
                RateBasis = dr.GetString("RateBasis"),
                Wheels = dr.GetInt32("Wheels"),
                BodyType = dr.GetString("BodyType"),
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
