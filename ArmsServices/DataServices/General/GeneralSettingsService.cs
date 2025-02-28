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

        // Method to select all general settings
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

        // Method to update a general setting
        public void Update(GeneralSettingsModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("Id",model.SettingId),
               new SqlParameter("@Value", model.Value),
               new SqlParameter("UserID",model.UserInfo.UserID),
               new SqlParameter("@SettingName", model.KeyString)
            };

            //Iservice.ExecuteNonQuery("[usp.User.GeneralSettings.Update]", parameters);
            Iservice.ExecuteNonQuery("[usp.User.Entity.ConfigTable.Update]", parameters);
        }

        // Helper method to map data record to GeneralSettingsModel
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
                KeyString = dr.GetString("KeyString"),
            UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserId"),
                },
            };
        }

        // Method to select value options for a specific setting
        public IEnumerable<ValueOptions> SelectValues(GeneralSettingsModel model)
        {
            //foreach (IDataRecord dr in Iservice.GetDataReader("[usp.User.GeneralSettings.Select]", null))
            //{
            //    yield return GetModel(dr);
            //}
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@SettingName", model.KeyString)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.ConfigTable.ValueOptions.Select]", parameters))
            {
                yield return GetModels(dr);
            }
        }

        // Helper method to map data record to ValueOptions
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
