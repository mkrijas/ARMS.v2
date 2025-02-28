using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ArmsModels.BaseModels;
using System.Linq;


namespace ArmsServices.DataServices
{
    public class ROIAdBlueRatioService : IROIAdBlueRatioService
    {
        IDbService Iservice;
        public ROIAdBlueRatioService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public IEnumerable<ROIAdBlueStdModel> Select(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", 0),
               new SqlParameter("BranchID",BranchID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.AdBlue.Select]", parameters))
            {
                yield return GetModel(dr);
            }           
        }

        public ROIAdBlueStdModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
            };
           
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.AdBlue.Select]", parameters))
            {
               return GetModel(dr);
            }
            return null;
        }

        public ROIAdBlueStdModel Update(ROIAdBlueStdModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@Wheels", model.Wheels),              
               new SqlParameter("@AdBlueRatio", model.AdBlueRatio),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.AdBlue.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }


        private ROIAdBlueStdModel GetModel(IDataRecord dr)
        {
            return new ROIAdBlueStdModel
            {
                ID = dr.GetInt32("ID"),
                Wheels = dr.GetByte("Wheels"),               
                BSType = dr.GetString("BsType"),               
                AdBlueRatio = dr.GetDecimal("AdBlueRatio"),
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
