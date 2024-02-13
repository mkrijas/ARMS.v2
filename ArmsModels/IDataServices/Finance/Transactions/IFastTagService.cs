using ArmsModels.BaseModels;
using Core.BaseModels.Finance.Transactions;
using System.Collections.Generic;

namespace Core.IDataServices.Finance.Transactions
{
    public interface IFastTagService
    {
        IEnumerable<FastTagModel> MatchTrucks(List<FastTagList> model);
        IEnumerable<FastTagTollModel> SelectPendingFTDoc();
        IEnumerable<FastTagTollModel> GetUploadView(int? ID);
        FastTagTollModel GetUploadViewModel(int? FastTagUploadID);
        IEnumerable<FastTagModel> GetUploadViewCollection(int? FastTagUploadID);
        IEnumerable<FastTagTollModel> GetProcessView(int? BranchID);
        FastTagTollModel GetProcessViewModel(int? FastTagProcessID);
        IEnumerable<FastTagModel> GetProcessViewCollection(int? FastTagProcessID);
        IEnumerable<FastTagModel> SelectByBranch(int? FastTagUploadID, int BranchID);
        FastTagTollModel UpdateNew(FastTagTollModel model);
        FastTagTollModel UpdateProcess(FastTagTollModel model);
    }
}