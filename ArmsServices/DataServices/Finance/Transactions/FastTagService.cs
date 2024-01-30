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

        public IEnumerable<FastTagModel> Select(List<FastTagList> model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ExcelList", model.ToDataTable()),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[Finance.Transactions.FastTag.Excel.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }
        
        private FastTagModel GetModel(IDataRecord dr)
        {
            return new FastTagModel
            {
                TransactionDateTime = dr.GetDateTime("TollCrossTime"),
                NumberPlate = dr.GetString("RegNo"),
                BranchID = dr.GetInt32("BranchID"),
                BranchName = dr.GetString("BranchName"),
                Trip = new TripModel()
                {
                    TripID = dr.GetInt64("TripID"),
                    TruckID = dr.GetInt32("TruckID"),
                    TripPrefix = dr.GetString("TripPrefix"),
                    TripNumber = dr.GetInt64("TripNumber"),
                },
            };
        }
    }
}