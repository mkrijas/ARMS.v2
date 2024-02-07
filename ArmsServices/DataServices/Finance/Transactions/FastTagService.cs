using ArmsModels.BaseModels;
using Core.IDataServices.Finance.Transactions;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Core.BaseModels.Finance.Transactions;
using ArmsServices;

namespace DAL.DataServices.Finance.Transactions
{
    public class FastTagService : IFastTagService
    {
        IDbService Iservice;
        public FastTagService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public IEnumerable<FastTagModel> SelectForExcel(List<FastTagList> model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ForExcel"),
               new SqlParameter("@ExcelList", model.ToDataTable()),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[Finance.Transactions.FastTag.Excel.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<FastTagModel> SelectByBranch(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Operation", "ForBranch"),
                new SqlParameter("@BranchID", BranchID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[Finance.Transactions.FastTag.Excel.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public FastTagTollModel Update(FastTagTollModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@FastTagID", model.FastTagID),
               new SqlParameter("@DocumentNumber", model.DocumentNumber),
               new SqlParameter("@DocDate", model.DocumentDate),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@CoaID", model.PaymentCoaID),
               new SqlParameter("@ArdCode", model.PaymentArdCode),
               new SqlParameter("@PaymentTool", model.PaymentTool),
               new SqlParameter("@BankCharges", model.BankCharges),
               new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@TransactionList", model.FastTagModelList.ToDataTable()),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[Finance.Transactions.FastTag.Update]", parameters))
            {
                model = GetMainModel(dr);
            }
            return model;
        }

        private FastTagTollModel GetMainModel(IDataRecord dr)
        {
            return new FastTagTollModel
            {

            };
        }

        private FastTagModel GetModel(IDataRecord dr)
        {
            return new FastTagModel
            {
                TransactionDateTime = dr.GetDateTime("TollCrossTime"),
                NumberPlate = dr.GetString("RegNo"),
                BranchID = dr.GetInt32("BranchID"),
                BranchName = dr.GetString("BranchName"),
                TripID = dr.GetInt64("TripID"),
                TruckID = dr.GetInt32("TruckID"),
                TripPrefix = dr.GetString("TripPrefix"),
                TripNumber = dr.GetInt64("TripNumber"),
            };
        }
    }
}