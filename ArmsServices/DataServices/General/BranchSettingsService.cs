using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ArmsModels.BaseModels.General;
using Core.BaseModels.Finance.Transactions;
using ArmsModels.SharedModels;
using System.Reflection;
using System.Linq;

namespace ArmsServices.DataServices
{
    public class BranchSettingsService : IBranchSettingsService
    {
        IDbService Iservice;
        public BranchSettingsService(IDbService iservice)
        {
            Iservice = iservice;
        }


        public IEnumerable<SettingsModel> SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@BranchID", ID),
            };
            {
                foreach (IDataRecord dr in Iservice.GetDataReader("[usp.entity.Branch.SettingsMaster.SelectByID]", parameters))
                {
                    yield return GetModel(dr);
                }
            }
        }
        public SettingsModel GetModel(IDataRecord dr)
        {
            return new SettingsModel
            {
                SettingsID = dr.GetInt32("SettingsID"),
                SettingsName = dr.GetString("SettingsName"),
                SettingsDescription = dr.GetString("SettingsDescription"),
                RecordStatus = dr.GetBoolean("IsSet"),
            };
        }


        //////////////////////////////////


        public SettingsModel Update(int? ID, List<int?> RecordStatusList, UserInfoModel UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", ID),
               new SqlParameter("@Settings", RecordStatusList.ToDataTable()),
               new SqlParameter("@UserID", UserID.UserID),
            };
            Iservice.ExecuteNonQuery("[usp.entity.Branch.SettingsMaster.Update]", parameters);
            return null;
        }

        public bool IsEnabled(int? BranchID, int? OptionID)
        {
            var list = SelectByID(BranchID);
            return list.Where(x => x.SettingsID == OptionID).Select(x => x.RecordStatus.Value).First();
        }
    }
}