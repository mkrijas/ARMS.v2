using ArmsModels.BaseModels;
using Core.BaseModels.Finance.Transactions;
using System.Collections.Generic;

namespace Core.IDataServices.Finance.Transactions
{
    public interface IFastTagService
    {
        IEnumerable<FastTagModel> MatchTrucks(List<FastTagList> model);
        IEnumerable<FastTagProcessModel> SelectPendingFTDoc();
        IEnumerable<FastTagTollModel> GetUploadViewInComplete(int? ID);
        IEnumerable<FastTagTollModel> GetUploadViewComplete(int? ID);
        FastTagTollModel GetUploadViewModel(int? FastTagUploadID);
        IEnumerable<FastTagModel> GetUploadViewCollection(int? FastTagUploadID);
        IEnumerable<FastTagModel> GetUploadViewSelectedCollection(int? FastTagUploadID);
        IEnumerable<FastTagTollModel> GetProcessView(int? BranchID);
        FastTagProcessModel GetProcessViewModel(int? FastTagProcessID);
        IEnumerable<FastTagModel> GetProcessViewCollection(int? FastTagProcessID);
        IEnumerable<FastTagModel> SelectByBranch(int? FastTagUploadID, int BranchID);
        FastTagTollModel UpdateNew(FastTagTollModel model);
        FastTagProcessModel UpdateProcess(FastTagProcessModel model);
        int Approve(int? FastTagProcessID, string UserID, string Remarks);  //Approve
    }
}