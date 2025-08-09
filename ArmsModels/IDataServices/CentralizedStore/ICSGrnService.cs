using System.Collections.Generic;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface ICSGrnService : IbaseInterface<CSGrnModel>
    {
        CSGrnModel Update(CSGrnModel model); 
        CSGrnModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);
        IEnumerable<CSGrnModel> Select();
        IEnumerable<CSGrnModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<CSGrnModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<CSGrnItemModel> GetItems(int? GrnID);
        int Approve(int? GrnID, string UserID, string Remarks);  //Approve
    }
}