using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using Core.BaseModels.Inventory;

namespace ArmsServices.DataServices
{
    public interface IStockTransferService
    {
        StockTransferInitiationModel Update(StockTransferInitiationModel model);
    }
}