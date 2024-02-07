using ArmsModels.BaseModels;
using Core.BaseModels.Finance.Transactions;
using System.Collections.Generic;

namespace Core.IDataServices.Finance.Transactions
{
    public interface IFastTagService
    {
        IEnumerable<FastTagModel> SelectForExcel(List<FastTagList> model);
        IEnumerable<FastTagModel> SelectByBranch(int? BranchID);
        FastTagTollModel Update(FastTagTollModel model);
    }
}
