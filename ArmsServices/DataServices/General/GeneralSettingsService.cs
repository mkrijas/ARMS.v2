using ArmsModels.BaseModels;
using ArmsModels.BaseModels.General;
using ArmsServices;
using ArmsServices.DataServices;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.DataServices.General
{
    public class GeneralSettingsService : IGeneralSettingsService
    {
        IDbService Iservice;
        public GeneralSettingsService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public IEnumerable<GeneralSettingsModel> Select()
        {
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.User.GeneralSettings.Select]", null))
            {
                yield return GetModel(dr);
            }
        }
        public void Update(GeneralSettingsModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("Id",model.SettingId),
               new SqlParameter("@Value", model.Value),
               new SqlParameter("UserID",model.UserInfo.UserID),
            };

            Iservice.ExecuteNonQuery("[usp.User.GeneralSettings.Update]", parameters);
        }

        private GeneralSettingsModel GetModel(IDataRecord dr)
        {
            return new GeneralSettingsModel
            {
                SettingId = dr.GetInt32("ID"),
                SettingName = dr.GetString("Description"),
                Value = dr.GetString("Value"),
                ValueOptions = dr.GetString("ValueOptions"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserId"),
                },
            };
        }
    }
}
