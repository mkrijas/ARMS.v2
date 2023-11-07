using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IBranchService
    {
        BranchModel Update(BranchModel model);  //Edit
        BranchModel SelectByID(int? ID);
        string GetBranchName(int? BranchID);
        int Delete(int? AddressID, string UserID);  //Delete
        IEnumerable<BranchModel> Select();
        int AddContact(int? BranchID, ContactModel contact);  //Edit
        IEnumerable<ContactModel> GetContacts(int? PartyID);
        BranchModel ValidateGstNo(int? PlaceID);
    }
}