using ArmsServices;
using Core.BaseModels.Operations.ROI;
using Core.IDataServices.Operations.ROI;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ArmsModels.BaseModels;
using ArmsModels.SharedModels;

namespace DAL.DataServices.Operations.ROI
{
    public class ROITimeService : IROITimeService
    {
        IDbService Iservice;
        public ROITimeService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public IEnumerable<ROIWheelSpeedModel> SelectWheelSpeed(int? RowNo)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@RowNo", RowNo)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.WheelSpeed.Select]", parameters))
            {
                yield return WheelSpeedModel(dr);
            }
        }

        public ROIWheelSpeedModel UpdateWheelSpeed(ROIWheelSpeedModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@Wheels", model.Wheels),
               new SqlParameter("@Speed", model.Speed),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.WheelSpeed.Update]", parameters))
            {
                return null;
            }
            return null;
        }

        public IEnumerable<ROILoadAndUnloadModel> SelectLoadUnload(int? RowNo,int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@RowNo", RowNo),
                new SqlParameter("BranchID",BranchID)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.LoadAndUnloadDuration.Select]", parameters))
            {
                yield return LoadUnloadModel(dr);
            }
        }

        public ROILoadAndUnloadModel UpdateLoadUnload(ROILoadAndUnloadModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@OrderID", model.Order.OrderID),
               new SqlParameter("@LoadingDuration", model.LoadingDuration),
               new SqlParameter("@UnLoadingDuration", model.UnLoadingDuration),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.LoadAndUnloadDuration.Update]", parameters))
            {
                return null;
            }
            return null;
        }

        private ROIWheelSpeedModel WheelSpeedModel(IDataRecord dr)
        {
            return new ROIWheelSpeedModel
            {
                ID = dr?.GetInt32("ID"),
                Wheels = dr?.GetByte("Wheels"),
                Speed = dr.GetDecimal("Speed"),
                UserInfo = new UserInfoModel
                {
                    UserID = dr.GetString("UserID"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    RecordStatus = dr?.GetByte("RecordStatus"),
                }
            };
        }

        private ROILoadAndUnloadModel LoadUnloadModel(IDataRecord dr)
        {
            return new ROILoadAndUnloadModel
            {
                ID = dr?.GetInt32("ID"),
                Order = new OrderModel
                {
                    OrderID = dr?.GetInt32("OrderID"),
                    OrderName = dr?.GetString("OrderName"),
                },
                LoadingDuration = dr?.GetDecimal("LoadingDuration"),
                UnLoadingDuration = dr?.GetDecimal("UnLoadingDuration"),
                UserInfo = new UserInfoModel
                {
                    UserID = dr.GetString("UserID"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    RecordStatus = dr?.GetByte("RecordStatus"),
                }
            };
        }
    }
}
