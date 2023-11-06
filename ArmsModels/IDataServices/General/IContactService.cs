
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;



namespace ArmsServices.DataServices
{
    public interface IContactService
    {
        ContactModel Update(ContactModel model);  //Edit
        ContactModel SelectByID(int? ContactID);
        int Delete(int? ContactID, string UserID);  //Delete
        IEnumerable<ContactModel> Select(int? RefKey, string RefTable);
    }
}