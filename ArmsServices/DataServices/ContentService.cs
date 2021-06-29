using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IContentService
    {
        Task<ContentModel> Update(ContentModel model);
        Task<ContentModel> SelectByID(int ID);
        Task<int> Delete(int ContentID, string UserID);
        IAsyncEnumerable<ContentModel> Select(int? ContentID);
    }

    public class ContentService : IContentService
    {
        IDbService Iservice;

        public ContentService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public async Task<ContentModel> Update(ContentModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ContentID", model.ContentID),
               new SqlParameter("@ContentName", model.ContentName),
               new SqlParameter("@PrimaryUnit", model.PrimaryUnit),
               new SqlParameter("@SecondaryUnit", model.SecondaryUnit),
               new SqlParameter("@UnitRatio", model.UnitRatio),
             
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            await foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Gc.Content.Update]", parameters))
            {
                model = await GetModel(dr);
            }
            return model;
        }
        public async Task<ContentModel> SelectByID(int ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ContentID", ID),
            };
            ContentModel model = new ContentModel();
            await foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Gc.Content.Select]", parameters))
            {
                model = await GetModel(dr);
            }
            return model;
        }
        public async Task<int> Delete(int ContentID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ContentID", ContentID),               
               new SqlParameter("@UserID", UserID),
            };            
            return await Iservice.ExecuteNonQuery("[usp.Gc.Content.Delete]", parameters);
        }
        public async IAsyncEnumerable<ContentModel> Select(int? ContentID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ContentID", ContentID)               
            };
            await foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Gc.Content.Select]", parameters))
            {
                yield return await GetModel(dr);      
            }
        }

        private async Task<ContentModel> GetModel(IDataRecord dr)
        {
            return new ContentModel
            {
                ContentID = dr.GetInt16("ContentID"),
                ContentName = dr.GetString("ContentName"),
                PrimaryUnit = dr.GetString("PrimaryUnit"),
                SecondaryUnit = dr.SafeGetString("SecondaryUnit"),
                UnitRatio = dr.GetDecimal("UnitRatio"),
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
