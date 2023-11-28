using ArmsModels.BaseModels.General;
using System.Collections.Generic;

namespace ArmsServices.DataServices
{
    public interface IGeneralSettingsService
    {
        IEnumerable<GeneralSettingsModel> Select();
        void Update(GeneralSettingsModel model);
    }
}
