using ArmsModels.BaseModels;
using System.Collections.Generic;

namespace ArmsServices.DataServices
{
    public interface IOperationPostingGroupService
    {
        OperationPostingGroupModel Update(OperationPostingGroupModel model);
        IEnumerable<OperationPostingGroupModel> Select();
    }
}