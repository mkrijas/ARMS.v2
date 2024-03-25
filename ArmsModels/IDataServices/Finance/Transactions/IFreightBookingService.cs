using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IFreightBillingService
    {
        int? UpdateFinalInvoice(BillingModel model);  
        int? UpdateProformaInvoice(ProformaInvoiceModel model);  //EditProformaInvoice
        int? ApproveProformaInvoice(int? ProformaInvoiceID, string userID, string Remarks,string InvoiceNumber);  //Approve
        int? ReverseProformaInvoice(int? ProformaInvoiceID, string userID);  //ReverseProformaInvoice
        int? UpdateConsolidatedDraftBill(ConsolidatedDraftBillModel model);  //EditConsolidatedDraftBill
        BillingModel SelectFinalInvoice(int? ID);
        ProformaInvoiceModel SelectProformaInvoice(int? ID);
        ConsolidatedDraftBillModel SelectConsolidatedDraftBill(int? ID);
        int ReverseFinalInvoice(int? ID, string UserID, string Remarks);
        int DeleteProformaInvoice(int? ID,string UserID, string Remarks);  //Delete
        int ReverseConsolidatedDraftBill(int? ID, string UserID);  //ReverseConsolidatedDraftBill
        IEnumerable<ConsolidatedDraftBillModel> SelectPendingConsolidatedDraftBillList(int? ID,int? BranchId);
        IEnumerable<ProformaInvoiceModel> SelectPendingProformaInvoiceList(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<ProformaInvoiceModel> SelectProformaInvoiceList(int? BranchID, int? ID, int? NumberOfRecords, string searchTerm);
        IEnumerable<GcTariffModel> GetPending(int? OrderID, short? TariffTypeID);
        IEnumerable<GcTariffModel> GetPending(int? OrderID, short? TariffTypeID, int? GcTypeID, DateTime? begin, DateTime? end);
        IEnumerable<GcTariffModel> GenerateTariffs(int? OrderID, short? TariffTypeID, int? GcTypeID, DateTime? begin, DateTime? end);
        IEnumerable<GcTariffModel> GetBilled(int? ConsolidatedDraftBillID);
        GstModel GetGstRate(int? DraftBillID);
    }
}