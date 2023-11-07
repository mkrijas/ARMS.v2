using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IAddressService
    {
        AddressModel Update(AddressModel model);  //Edit
        AddressModel SelectByID(int? AddressID);
        int Delete(int? AddressID, string UserID);  //Delete
        IEnumerable<AddressModel> Select(int? AddressID);
    }
}