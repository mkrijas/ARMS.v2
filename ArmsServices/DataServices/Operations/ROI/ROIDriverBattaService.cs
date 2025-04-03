using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class ROIDriverBattaService : IROIDriverBattaService
    {
        IDbService Iservice;

        public ROIDriverBattaService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public ROIDriverBattaModel Update(ROIDriverBattaModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@Wheels", model.Wheels),
               new SqlParameter("@BodyType", model.BodyType),
               new SqlParameter("@FromStdKM", model.FromStdKM),
               new SqlParameter("@ToStdKM", model.ToStdKM),
               new SqlParameter("@LoadingMTFrom", model.LoadingMTFrom),
               new SqlParameter("@LoadingMTTo", model.LoadingMTTo),
               new SqlParameter("@BranchID", model.Branch.BranchID),
               new SqlParameter("@Amount", model.Amount),
               new SqlParameter("@FromDate", model.FromDate),
               new SqlParameter("@ToDate", model.ToDate),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.DriverBatta.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<ROIDriverBattaModel> Select(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", 0),
               new SqlParameter("BranchID",BranchID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.DriverBatta.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public ROIDriverBattaModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
            };
            ROIDriverBattaModel model = new ROIDriverBattaModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.DriverBatta.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private ROIDriverBattaModel GetModel(IDataRecord dr)
        {
            return new ROIDriverBattaModel
            {
                ID = dr.GetInt32("ID"),
                Wheels = dr.GetByte("Wheels"),
                BodyType = dr.GetString("BodyType"),
                FromStdKM = dr.GetDecimal("FromStdKM"),
                ToStdKM = dr.GetDecimal("ToStdKM"),
                LoadingMTFrom = dr.GetDecimal("LoadingMTFrom"),
                LoadingMTTo = dr.GetDecimal("LoadingMTTo"),
                Branch = new BranchModel
                {
                    BranchID = dr.GetInt32("BranchID"),
                    BranchName = dr.GetString("BranchName")
                },
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

        public ROIDriverBattaInFrtPercentageModel UpdateInFrtPercentage(ROIDriverBattaInFrtPercentageModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@BranchID", model.Branch.BranchID),
               new SqlParameter("@Percentage", model.Percentage),
               new SqlParameter("@FromDate", model.FromDate),
               new SqlParameter("@ToDate", model.ToDate),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.DriverBattaInFrtPercentage.Update]", parameters))
            {
                model = GetModelInFrtPercentage(dr);
            }
            return model;
        }

        public IEnumerable<ROIDriverBattaInFrtPercentageModel> SelectInFrtPercentage(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", 0),
               new SqlParameter("BranchID",BranchID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.DriverBattaInFrtPercentage.Select]", parameters))
            {
                yield return GetModelInFrtPercentage(dr);
            }
        }

        public ROIDriverBattaInFrtPercentageModel SelectByIDInFrtPercentage(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
            };
            ROIDriverBattaInFrtPercentageModel model = new ROIDriverBattaInFrtPercentageModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.DriverBattaInFrtPercentage.Select]", parameters))
            {
                model = GetModelInFrtPercentage(dr);
            }
            return model;
        }

        private ROIDriverBattaInFrtPercentageModel GetModelInFrtPercentage(IDataRecord dr)
        {
            return new ROIDriverBattaInFrtPercentageModel
            {
                ID = dr.GetInt32("ID"),
                Branch = new BranchModel
                {
                    BranchID = dr.GetInt32("BranchID"),
                    BranchName = dr.GetString("BranchName")
                },
                Percentage = dr.GetDecimal("Percentage"),
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
