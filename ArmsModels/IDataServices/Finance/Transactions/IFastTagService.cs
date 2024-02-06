using ArmsModels.BaseModels;
using Core.BaseModels.Finance.Transactions;
using System.Collections.Generic;

namespace Core.IDataServices.Finance.Transactions
{
    public interface IFastTagService
    {
        IEnumerable<FastTagModel> Select(List<FastTagList> model);
        FastTagTollModel Update(FastTagTollModel model);
    }
}
