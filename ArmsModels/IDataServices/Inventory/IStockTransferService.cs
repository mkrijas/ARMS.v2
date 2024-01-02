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
        IEnumerable<StockTransferInitiationModel> SelectOutGoing(int? BranchID);
        IEnumerable<StockTransferInitiationModel> SelectIncoming(int? BranchID);
        IEnumerable<InventoryItemEntryModel> SelectItemsList(int? InvTranID);
        StockTransferInitiationModel Initiate(StockTransferInitiationModel model);  //Edit
        StockTransferEndModel UpdateDelivery(StockTransferEndModel model);  //Accept
        StockTransferInitiationModel SelectSandB(int? InvTranID);
        int TransferCancel(int? InvTranID, string UserID);  //Cancel
        StockTransferInitiationModel RejectOrder(StockTransferInitiationModel model);  //Reject
        int Approve(int? ID, string UserID, string Remarks);
    }
}