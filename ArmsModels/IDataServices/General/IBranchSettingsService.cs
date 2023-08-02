using ArmsModels.BaseModels;
using ArmsModels.BaseModels.General;
using ArmsModels.SharedModels;
using Core.BaseModels.Finance.Transactions;
using System.Collections.Generic;

namespace ArmsServices.DataServices
{
    public interface IBranchSettingsService
    {
        IEnumerable<SettingsModel> SelectByID(int? BranchID);
        SettingsModel Update(int? ID, List<int?> RecordStatusList, string UserID);
        bool IsEnabled(int? BranchID, int? OptionID);
    }
}