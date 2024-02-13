using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using Microsoft.Extensions.Configuration;

namespace ArmsServices.DataServices
{
    public interface ITariffService
    {
        TariffModel Update(TariffModel model);  //Edit
        int Delete(int? ID, string UserID);  //Edit
        IEnumerable<TariffModel> Select();
        IEnumerable<TariffModel> SelectByOrder(int? OrderID);
        TariffModel SelectByID(int? ID);
        IEnumerable<TariffFormulaModel> SelectFormulas();
        TariffFormulaModel SelectFormulaByID(short? ID);
        IEnumerable<TariffTypeModel> SelectTariffTypes(string Area = "Operation");
        TariffTypeModel SelectTariffTypeByID(short? ID);
        TariffTypeModel UpdateTariffType(TariffTypeModel model);  //Edit
        string[] TariffGroups { get; }
        //IEnumerable<TariffModel> GetTariffs(string TariffGroup, int? OrderID, int? RouteID, int? Wheels);
        decimal? GetPrimaryFreight(int? OrderID, int? RouteID, int? Wheels, decimal? Qty, decimal? Frt);
        decimal? GetTariffAmount(GcSetModel GcSet, TariffModel Tariff);
        IEnumerable<TariffModel> GeneratePendingTariffs(long? RefID);
    }
}