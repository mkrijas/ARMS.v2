using ArmsModels.BaseModels;
using ArmsModels.BaseModels.General;
using ArmsServices;
using ArmsServices.DataServices;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

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
            //foreach (IDataRecord dr in Iservice.GetDataReader("[usp.User.GeneralSettings.Select]", null))
            //{
            //    yield return GetModel(dr);
            //}
            foreach(IDataRecord dr in Iservice.GetDataReader("[usp.User.Entity.ConfigTable.Select]", null))
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
               new SqlParameter("@SettingName", model.SettingName)
            };

            //Iservice.ExecuteNonQuery("[usp.User.GeneralSettings.Update]", parameters);
            Iservice.ExecuteNonQuery("[usp.User.Entity.ConfigTable.Update]", parameters);
        }

        private GeneralSettingsModel GetModel(IDataRecord dr)
        {
            return new GeneralSettingsModel
            {
                SettingId = dr.GetString("ID"),
                SettingName = dr.GetString("Description"),
                Value = dr.GetString("Value"),
                ValueOptions = dr.GetString("ValueOptions"),
                selectedValue = new KeyValuePair<string, string>(
                    dr["Value"].ToString(),
                    dr["Value"].ToString()),
                ValueSelectType = dr.GetString("ValueSelectType"),
            UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserId"),
                },
            };
        }

        public IEnumerable<ValueOptions> SelectValues(GeneralSettingsModel model)
        {
            //foreach (IDataRecord dr in Iservice.GetDataReader("[usp.User.GeneralSettings.Select]", null))
            //{
            //    yield return GetModel(dr);
            //}
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@SettingName", model.SettingName)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.ConfigTable.ValueOptions.Select]", parameters))
            {
                yield return GetModels(dr);
            }
        }

        private ValueOptions GetModels(IDataRecord dr)
        {
            return new ValueOptions
            {
                id = dr.GetString("id"),
                val = dr.GetString("val")
            };
        }
    }
}
