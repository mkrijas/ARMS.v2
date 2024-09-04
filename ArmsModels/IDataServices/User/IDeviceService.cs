using ArmsModels.BaseModels;
using Core.BaseModels.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.IDataServices.User
{
    public interface IDeviceService
    {
        IEnumerable<DeviceModel> Select(string operation);
        Task<int>  Approve(DeviceModel model);
        Task<int> Deny(DeviceModel model);
    }
}
