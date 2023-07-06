using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IOrderService
    {
        Task<OrderModel> Update(OrderModel model);
        Task<OrderModel> SelectByID(int? ID);
        IAsyncEnumerable<OrderModel> SelectByBranch(int? BranchID);
        Task<int> Delete(int? OrderID, string UserID);
        IAsyncEnumerable<OrderModel> Select(int? OrderID);
        Task<int> BranchOrderUpdate(int? BranchID, int? OrderID, string UserID, string operation);
    }
}