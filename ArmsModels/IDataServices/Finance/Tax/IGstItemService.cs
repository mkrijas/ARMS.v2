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
    public interface IGstItemService
    {
        GstItemModel Update(GstItemModel model);
        GstItemModel SelectByID(int ID);
        GstItemModel SelectByItem(int ItemID, DateTime? entryDate);
        IEnumerable<GstItemModel> SelectByTaxRate(decimal TaxRate, DateTime? entryDate);
        IEnumerable<GstItemModel> FilterByText(string FilterText, DateTime? entryDate);
        int Delete(int ID, string UserID);
        IEnumerable<GstItemModel> SelectByItem(int ItemID);
        IEnumerable<GstItemModel> Select(DateTime? entryDate);
        IEnumerable<GstItemModel> SelectByDate(DateTime? entryDate);
        IEnumerable<GstInOutModel> GetInOut();
    }
}