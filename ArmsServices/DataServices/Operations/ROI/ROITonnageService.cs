using ArmsServices;
using Core.BaseModels.Finance.Transactions;
using Core.IDataServices.Operations.ROI;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Core.BaseModels.Operations.ROI;
using System;
using ArmsModels.BaseModels;
using ArmsModels.SharedModels;

namespace DAL.DataServices.Operations.ROI
{
    public class ROITonnageService : IROITonnageService
    {
        IDbService Iservice;
        public ROITonnageService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public IEnumerable<ROITonnageModel> SelectBSType()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Operation", "BSTYPE"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.Common.Select]", parameters))
            {
                yield return Model(dr);
            }
        }

        public IEnumerable<ROITonnageModel> SelectWheels()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Operation", "WHEELS"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.Common.Select]", parameters))
            {
                yield return Model(dr);
            }
        }

        public IEnumerable<ROITonnageModel> Select(int? RowNo, int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@RowNo", RowNo),
                new SqlParameter("BranchID",BranchID)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.Tonnage.Select]", parameters))
            {
                yield return Model(dr);
            }
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.ROI.Tonnage.Delete]", parameters);
        }

        public ROITonnageModel Update(ROITonnageModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@OrderID", model.Order.OrderID),
               new SqlParameter("@RouteID", model.Route.RouteID),
               new SqlParameter("@Wheels", model.Wheels),
               new SqlParameter("@BSType", model.BSType),
               new SqlParameter("@FromTonnage", model.FromTonnage),
               new SqlParameter("@ToTonnage", model.ToTonnage),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.Tonnage.Update]", parameters))
            {
                return null;
            }
            return null;
        }

        private ROITonnageModel Model(IDataRecord dr)
        {
            return new ROITonnageModel
            {
                ID = dr?.GetInt32("ID"),
                Wheels = dr?.GetByte("Wheels"),
                BSType = dr.GetString("BSType"),
                FromTonnage = dr?.GetDecimal("FromTonnage"),
                ToTonnage = dr?.GetDecimal("ToTonnage"),
                Order = new OrderModel
                {
                    OrderID = dr.GetInt32("OrderID"),
                    OrderName = dr.GetString("OrderName")
                },
                Route = new RouteModel
                {
                    RouteID = dr.GetInt32("RouteID"),
                    RouteName = dr.GetString("RouteName")
                },
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