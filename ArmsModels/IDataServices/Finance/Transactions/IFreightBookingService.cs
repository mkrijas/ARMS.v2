using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArmsServices.DataServices
{
    public interface IFreightBillingService : IbaseInterface<ProformaInvoiceModel>
    {
        int? UpdateFinalInvoice(BillingModel model);  
        int? Approve(int? ProformaInvoiceID, string userID, string Remarks,string InvoiceNumber, string InvoiceRefNumber, DateTime? InvoiceDate);  //Approve
         int? UpdateConsolidatedDraftBill(ConsolidatedDraftBillModel model);  //EditConsolidatedDraftBill
        BillingModel SelectFinalInvoice(int? ID);       
        ConsolidatedDraftBillModel SelectConsolidatedDraftBill(int? ID);      
        int ReverseConsolidatedDraftBill(int? ID, string UserID);  //ReverseConsolidatedDraftBill
        IEnumerable<ConsolidatedDraftBillModel> SelectPendingConsolidatedDraftBillList(int? ID,int? BranchId);     
        IEnumerable<GcTariffModel> GetPending(int? OrderID, short? TariffTypeID);
        IEnumerable<GcTariffModel> GetPending(int? PartyID,int? OrderID, short? TariffTypeID, int? GcTypeID, int? TruckID, DateTime? begin, DateTime? end);
        IEnumerable<GcTariffModel> GenerateTariffs(int? OrderID, short? TariffTypeID, int? GcTypeID, DateTime? begin, DateTime? end);
        IEnumerable<GcTariffModel> GetBilled(int? ConsolidatedDraftBillID);
        GstModel GetGstRate(int? DraftBillID);
        int DeleteApproved(ProformaInvoiceModel model);  //Cancel Bill
    }
}