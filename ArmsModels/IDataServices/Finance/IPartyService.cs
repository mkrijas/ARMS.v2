using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Reflection;


namespace ArmsServices.DataServices
{
    public interface IPartyService
    {       
        PartyModel Update(PartyModel model);
        PartyModel SelectByID(int? ID);
        int Delete(int? PartyID, string UserID);
        IEnumerable<PartyModel> Select(int? PartyID);
        IEnumerable<PartyModel> SelectByCode(string PartyCode,string NatureOfBusiness);     
        IEnumerable<ContactModel> GetContacts(int? PartyID);
        IEnumerable<PartyModel> GetCustomers(string Code);
        IEnumerable<PartyModel> GetVendors(string Code);
        IEnumerable<PartyModel> GetRenters(string Code);
        int AddContact(int? PartyID, ContactModel contact,string ContactID);
        int? GetVendorPayableCoaID(int? VendorID);
        int? GetVendorDepositCoaID(int? VendorID);
        int? GetVendorPrepaymentCoaID(int? VendorID);
        int? GetCustomerReceivableCoaID(int? CustomerID);
        int? GetCustomerDepositCoaID(int? CustomerID);
        int? GetCustomerPrepaymentCoaID(int? CustomerID);
        int? GetRenterRentCoaID(int? RenterID);
        int? GetRenterDepositCoaID(int? RenterID);
        int? GetRenterOtherCoaID(int? RenterID);
        bool IsLocal(int? PartyID, int? BranchID);
        int? GetPartyCoaID(int? PartyID, string BusinessNature, string NatureOfTransaction);
    }
}
