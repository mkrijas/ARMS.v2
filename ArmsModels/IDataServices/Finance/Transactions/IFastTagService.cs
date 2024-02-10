using ArmsModels.BaseModels;
using Core.BaseModels.Finance.Transactions;
using System.Collections.Generic;

namespace Core.IDataServices.Finance.Transactions
{
    public interface IFastTagService
    {
        IEnumerable<FastTagModel> SelectForExcel(List<FastTagList> model);
        IEnumerable<FastTagTollModel> AllDocumentNumbers();
        IEnumerable<FastTagTollModel> GetUploadView(int? ID);
        FastTagTollModel GetUploadViewModel(int? ID);
        IEnumerable<FastTagModel> GetUploadViewCollection(int? FastTagUploadID);
        IEnumerable<FastTagTollModel> GetProcessView(int? ID);
        IEnumerable<FastTagModel> SelectByBranch(int? FastTagUploadID, int BranchID);
        FastTagTollModel UpdateNew(FastTagTollModel model);
        FastTagTollModel UpdateProcess(FastTagTollModel model);
    }
}