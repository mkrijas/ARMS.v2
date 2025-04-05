using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ArmsModels.BaseModels.General;
using Core.BaseModels.Finance.Transactions;
using ArmsModels.SharedModels;
using System.Reflection;
using System.Linq;
using System;

namespace ArmsServices.DataServices
{
    public class BranchSettingsService : IBranchSettingsService
    {
        IDbService Iservice;
        public BranchSettingsService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to select settings by branch ID
        public IEnumerable<SettingsModel> SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@BranchID", ID),
            };
            {
                foreach (IDataRecord dr in Iservice.GetDataReader("[usp.entity.Branch.Settings.SelectByID]", parameters))
                {
                    yield return GetModel(dr);
                }
            }
        }

        // Helper method to map data record to SettingsMode
        public SettingsModel GetModel(IDataRecord dr)
        {
            return new SettingsModel
            {
                SettingsID = dr.GetInt32("SettingsID"),
                SettingsName = dr.GetString("SettingsName"),
                SettingsDescription = dr.GetString("SettingsDescription"),
                RecordStatus = dr.GetBoolean("IsSet"),
                ValueInput = dr.GetBoolean("ValueInput"),
                Value = dr.GetString("Value")
            };
        }


        //////////////////////////////////

        // Method to update branch settings
        public SettingsModel Update(int? ID, List<SettingsModel> settingsList, string UserID)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("IntField", typeof(int));
            dataTable.Columns.Add("Value", typeof(string));

            foreach (var setting in settingsList.Where(s => s.RecordStatus == true))
            {
                dataTable.Rows.Add(setting.SettingsID, setting.Value ?? (object)DBNull.Value);
            }

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@BranchID", ID),
                new SqlParameter("@Settings", dataTable),
                new SqlParameter("@UserID", UserID),
            };

            Iservice.ExecuteNonQuery("[usp.entity.Branch.Settings.Update]", parameters);
            return null;
        }

        // Method to check if a specific setting is enabled for a branch
        public bool IsEnabled(int? BranchID, int? OptionID)
        {
            var list = SelectByID(BranchID);
            return list!= null && list.Any(x => x != null && x.RecordStatus != null && x.SettingsID == OptionID) ? list.Where(x => x != null && x.RecordStatus != null && x.SettingsID == OptionID).Select(x => x.RecordStatus.Value)?.First()??false:false;
        }

        public string GetValue(int? BranchID, int? OptionID)
        {
            var list = SelectByID(BranchID);
            return list.FirstOrDefault(x => x.SettingsID == OptionID)?.Value;
        }

    }
}