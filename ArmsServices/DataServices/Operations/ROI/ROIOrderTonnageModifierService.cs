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
    public class ROIOrderTonnageModifierService : IROIOrderTonnageModifierService
    {
        IDbService Iservice;
        public ROIOrderTonnageModifierService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public IEnumerable<ROIOrderTonnageModifierModel> Select(int? RowNo, int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@RowNo", RowNo),
                new SqlParameter("BranchID",BranchID)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.Order_TonnageModifier.Select]", parameters))
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
            return Iservice.ExecuteNonQuery("[usp.ROI.Order_TonnageModifier.Delete]", parameters);
        }

        public ROIOrderTonnageModifierModel Update(ROIOrderTonnageModifierModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@OrderID", model.Order.OrderID),
               new SqlParameter("@Wheels", model.Wheels),
               new SqlParameter("@Modifier", model.Modifier),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.Order_TonnageModifier.Update]", parameters))
            {
                return null;
            }
            return null;
        }

        private ROIOrderTonnageModifierModel Model(IDataRecord dr)
        {
            return new ROIOrderTonnageModifierModel
            {
                ID = dr?.GetInt32("ID"),
                Order = new OrderModel
                {
                    OrderID = dr.GetInt32("OrderID"),
                    OrderName = dr.GetString("OrderName")
                },
                Wheels = dr.GetByte("Wheels"),
                Modifier = dr.GetInt32("Modifier"),
                UserInfo = new UserInfoModel
                {
                    UserID = dr.GetString("UserID"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    RecordStatus = dr?.GetByte("RecordStatus"),
                },                
            };
        }
    }
}