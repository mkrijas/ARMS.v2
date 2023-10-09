using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public class InsuranceClaimService : IInsuranceClaimService
    {
        IDbService Iservice;

        public InsuranceClaimService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int? InsuranceClaimID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@InsuranceClaimID", InsuranceClaimID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.FMS.Breakdown.Delete]", parameters);
        }



        public IEnumerable<InsuranceClaimModel> Select(int? InsuranceClaimID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@InsuranceClaimID", InsuranceClaimID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.InsuranceClaim.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public InsuranceClaimModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@InsuranceClaimID", ID),
            };
            InsuranceClaimModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.InsuranceClaim.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public InsuranceClaimModel Update(InsuranceClaimModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@InsuranceClaimID", model.InsuranceClaimID),
               new SqlParameter("@Images", string.Join(";",model.Images)),
               new SqlParameter("@BreakdownID", model.BreakdownID),
               new SqlParameter("@InsuranceID", model.InsuranceID),
               new SqlParameter("@IsOpen", model.IsOpen),
               new SqlParameter("@Notes", model.Notes),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.InsuranceClaim.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<InsuranceClaimEventMasterModel> GetEventList(int? limiter)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@limiter", limiter),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.InsuranceClaim.EventMaster.Select]", parameters))
            {
                yield return GetEventMasterModel(dr);
            }
        }
        public InsuranceClaimEventMasterModel UpdateEventList(InsuranceClaimEventMasterModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Description", model.Description),
               new SqlParameter("@IcemID",model.IcemID),
               new SqlParameter("@IsMandatory", model.IsMandatory),
               new SqlParameter("@Order", model.Order),
               new SqlParameter("@Title", model.Title),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.InsuranceClaim.EventMaster.Update]", parameters))
            {
                model = GetEventMasterModel(dr);
            }
            return model;
        }

        public InsuranceClaimEventStatusModel UpdateClaimEvent(InsuranceClaimEventStatusModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@EventDate", model.EventDate),
               new SqlParameter("@IcemID", model.IcemID),
               new SqlParameter("@IcesID", model.IcesID),
               new SqlParameter("@InsuranceClaimID", model.InsuranceClaimID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.InsuranceClaim.EventStatus.Update]", parameters))
            {
                model = GetEventStatusModel(dr);
            }
            return model;
        }

        public IEnumerable<InsuranceClaimEventStatusModel> GetEventStatusList(int? InsuranceClaimID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@InsuranceClaimID", InsuranceClaimID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.InsuranceClaim.EventStatus.Select]", parameters))
            {
                yield return GetEventStatusModel(dr);
            }
        }
        private InsuranceClaimModel GetModel(IDataRecord dr)
        {
            return new InsuranceClaimModel
            {
                InsuranceClaimID = dr.GetInt32("InsuranceClaimID"),
                Images = dr.GetString("Images").Split(";").ToList(),
                BreakdownID = dr.GetInt32("BreakdownID"),
                InsuranceID = dr.GetInt32("InsuranceID"),
                IsOpen = dr.GetBoolean("IsOpen"),
                Notes = dr.GetString("Notes"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        private InsuranceClaimEventMasterModel GetEventMasterModel(IDataRecord dr)
        {
            return new InsuranceClaimEventMasterModel
            {
                Description = dr.GetString("Description"),
                IcemID = dr.GetInt32("IcemID"),
                IsMandatory = dr.GetBoolean("IsMandatory"),
                Order = dr.GetInt32("Order"),
                Title = dr.GetString("Title"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        private InsuranceClaimEventStatusModel GetEventStatusModel(IDataRecord dr)
        {
            return new InsuranceClaimEventStatusModel
            {
                EventDate = dr.GetDateTime("EventDate"),
                IcemID = dr.GetInt32("IcemID"),
                IcesID = dr.GetInt32("IcesID"),
                Title = dr.GetString("Title"),
                InsuranceClaimID = dr.GetInt32("InsuranceClaimID"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        public InsuranceClaimModel SelectByBreakdownID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BreakdownID", ID),
            };
            InsuranceClaimModel model = null;
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.InsuranceClaim.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public int OrderMoveUpward(int? Order)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Order", Order),
            };
            return Iservice.ExecuteNonQuery("[usp.FMS.InsuranceClaim.EventMaster.MoveUp]", parameters);           
        }
    }
}
