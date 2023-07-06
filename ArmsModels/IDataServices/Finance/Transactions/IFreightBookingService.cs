
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
        int? UpdateProformaInvoice(ProformaInvoiceModel model);
        int? ApproveProformaInvoice(int? ProformaInvoiceID, string userID, string Remarks);
        int? ReverseProformaInvoice(int? ProformaInvoiceID, string userID);
        int? UpdateConsolidatedDraftBill(ConsolidatedDraftBillModel model);
        BillingModel SelectFinalInvoice(int? ID);
        ProformaInvoiceModel SelectProformaInvoice(int? ID);
        ConsolidatedDraftBillModel SelectConsolidatedDraftBill(int? ID);
        int ReverseFinalInvoice(int? ID, string UserID, string Remarks);
        int DeleteProformaInvoice(int? ID,string UserID, string Remarks);
        int ReverseConsolidatedDraftBill(int? ID, string UserID);
        IEnumerable<ConsolidatedDraftBillModel> SelectPendingConsolidatedDraftBillList(int? ID,int? BranchId);
        IEnumerable<ProformaInvoiceModel> SelectPendingProformaInvoiceList(int? NumberOfRecords, string searchTerm);
        IEnumerable<ProformaInvoiceModel> SelectProformaInvoiceList(int? ID, int? NumberOfRecords, string searchTerm);
        IEnumerable<GcTariffModel> GetPending(int? OrderID, short? TariffTypeID);
        IEnumerable<GcTariffModel> GetPending(int? OrderID, short? TariffTypeID, DateTime? begin, DateTime? end);
        IEnumerable<GcTariffModel> GetBilled(int? ConsolidatedDraftBillID);
        GstModel GetGstRate(int? DraftBillID);
    }
}