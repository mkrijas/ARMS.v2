using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IExpenseMappingServices
    {
        IEnumerable<ExpenseMapping> Select();
        IEnumerable<ExpenseMapping> SelectByArea(string Area);
        ExpenseMapping Update(ExpenseMapping model);
        ExpenseMapping SelectByID(int ID);

        int Delete(int? ID, string UserID);
    }
        public class ExpenseMappingServices : IExpenseMappingServices
    {
        IDbService Iservice; 
        public ExpenseMappingServices(IDbService iservice)
        {
            Iservice = iservice;
        }
        public ExpenseMapping Update(ExpenseMapping model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ExpenseID", model.ExpenseID),
               new SqlParameter("@ExpenseTitle", model.ExpenseTitle),
               new SqlParameter("@Area", model.Area),
               new SqlParameter("@MappedCoaID", model.MappedCoaID.CoaID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.ExpenseMapping.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
        public IEnumerable<ExpenseMapping> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ExpenseID", 0)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.ExpenseMapping.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }
        public IEnumerable<ExpenseMapping> SelectByArea(string Area)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByArea"),
               new SqlParameter("@Area", Area),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.ExpenseMapping.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }
        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ExpenseID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Operation.ExpenseMapping.Delete]", parameters);
        }
        private ExpenseMapping GetModel(IDataRecord dr)
        {
            return new ExpenseMapping()
            {
                ExpenseID = dr.GetInt32("ExpenseID"),
                ExpenseTitle = dr.GetString("ExpenseTitle"),
                ExpenseCode = dr.GetString("ExpenseCode"),
                Area =  dr.GetString("Area"),
                MappedCoaID = new ChartOfAccountModel() { CoaID = dr.GetInt32("CoaID") },
                //MappedCoaID = new ChartOfAccountModel() { AccountName = dr.GetString("AccountName") },
                //MappedCoaID.CoaID = dr.GetInt32("MappedCoaID"),
                AccountName = dr.GetString("AccountName"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel()
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                }
            };
        }
        public ExpenseMapping SelectByID(int ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ExpenseID", ID),
               new SqlParameter("@Operation", "ByID"),
            };
            ExpenseMapping model = new ExpenseMapping();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.ExpenseMapping.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
    }
   
}
