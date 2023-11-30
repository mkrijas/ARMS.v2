using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ArmsServices.DataServices
{
    public class OperationPostingGroupService : IOperationPostingGroupService
    {
        IDbService Iservice;

        public OperationPostingGroupService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public IEnumerable<OperationPostingGroupModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", 0),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.Operation.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public OperationPostingGroupModel Update(OperationPostingGroupModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.Id),
               new SqlParameter("@ADCode", model.ADCode),
               new SqlParameter("@Title", model.Title),
               new SqlParameter("@UsageCode",model.UsageCode.UsageCode),
               new SqlParameter("@CoaID",model.UsageCode.CoaID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.Operation.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private OperationPostingGroupModel GetModel(IDataRecord dr)
        {
            return new OperationPostingGroupModel
            {
                Id = dr.GetInt32("ID"),
                ADCode = dr.GetString("ADCode"),
                Title = dr.GetString("Title"),
                UsageCode = new GstUsageCodeModel()
                {
                    CoaID = dr.GetInt32("CoaID"),
                    UsageCode = dr.GetString("UsageCode"),
                    Description = dr.GetString("Description"),
                },
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