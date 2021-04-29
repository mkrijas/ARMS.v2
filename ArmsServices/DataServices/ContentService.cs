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
        ContentModel Update(ContentModel model);
        int Delete(int ContentID, string UserID);
        IEnumerable<ContentModel> Select(int? ContentID);
    }

    public class ContentService : IContentService
    {
        IDbService Iservice;

        public ContentService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public ContentModel Update(ContentModel model)
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

            ContentModel rmodel = new ContentModel();
            using (var reader = Iservice.GetDataReader("[usp.Gc.ContentsUpdate]", parameters))
            {
                while (reader.Read())
                {
                    rmodel = new ContentModel
                    {
                        ContentID = reader.GetInt16("ContentID"),
                        ContentName = reader.GetString("ContentName"),
                        PrimaryUnit = reader.GetString("PrimaryUnit"),
                        SecondaryUnit = reader.SafeGetString("SecondaryUnit"),
                        UnitRatio = reader.IsDBNull("UnitRatio") ?null:reader.GetDecimal("UnitRatio"),
                        UserInfo = new ArmsModels.SharedModels.UserInfoModel
                        {
                            RecordStatus = reader.GetByte("RecordStatus"),
                            TimeStampField = reader.GetDateTime("TimeStampField"),
                            UserID = reader.GetString("UserID"),
                        },
                    };
                }
            }
            return rmodel;
        }
        public int Delete(int ContentID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ContentID", ContentID),               
               new SqlParameter("@UserID", UserID),
            };            
            return Iservice.ExecuteNonQuery("[usp.Gc.ContentsDelete]", parameters);
        }
        public IEnumerable<ContentModel> Select(int? ContentID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ContentID", ContentID)               
            };

            using (var reader = Iservice.GetDataReader("[usp.Gc.ContentsSelect]", parameters))
            {
                while (reader.Read())
                {
                    yield return new ContentModel
                    {
                        ContentID = reader.GetInt16("ContentID"),
                        ContentName = reader.GetString("ContentName"),
                        PrimaryUnit = reader.GetString("PrimaryUnit"),
                        SecondaryUnit = reader.SafeGetString("SecondaryUnit"),
                        UnitRatio = reader.IsDBNull("UnitRatio") ? null : reader.GetDecimal("UnitRatio"),
                        UserInfo = new ArmsModels.SharedModels.UserInfoModel
                        {
                            RecordStatus = reader.GetByte("RecordStatus"),
                            TimeStampField = reader.GetDateTime("TimeStampField"),
                            UserID = reader.GetString("UserID"),
                        },
                    };
                }
            }
        }

    }
}
