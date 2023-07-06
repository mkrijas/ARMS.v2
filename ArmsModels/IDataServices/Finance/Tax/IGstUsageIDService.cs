
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IGstUsageIDService
    {
        GstUsageCodeModel Update(GstUsageCodeModel model);
        GstUsageCodeModel SelectByCode(string Code);
        IEnumerable<GstUsageCodeModel> SelectByAccount(int AccountID , DateTime? entryDate);
        IEnumerable<GstUsageCodeModel> SelectByArea(string Area, DateTime? entryDate);
        IEnumerable<GstUsageCodeModel> SelectByTaxRate(decimal TaxRate, DateTime? entryDate);
        IEnumerable<GstUsageCodeModel> SelectBySAC(string SAC, DateTime? entryDate);
        IEnumerable<GstUsageCodeModel> FilterByText(string FilterText, DateTime? entryDate);
        int Delete(int? ID, string UserID);
        IEnumerable<GstUsageCodeModel> Select(DateTime? entryDate);
        IEnumerable<GstRateModel> GetGstRates();      
        IEnumerable<GstUsageCodeModel> SelectByTaxRateAccount(int? rateId, int? acID);
        decimal? GetGstRate(string UsageCode,DateTime? EntryDate);
    }
}