using ArmsModels.BaseModels;
using Core.IDataServices.Finance.Transactions;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Core.BaseModels.Finance.Transactions;
using ArmsServices;
using Microsoft.VisualBasic;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System;

namespace DAL.DataServices.Finance.Transactions
{
    public class FastTagService : IFastTagService
    {
        IDbService Iservice;
        public FastTagService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public IEnumerable<FastTagModel> MatchTrucks(List<FastTagList> model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ForExcel"),
               new SqlParameter("@ExcelList", model.ToDataTable()),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[Finance.Transactions.FastTag.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<FastTagTollModel> SelectPendingFTDoc()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Operation", "AllDocNo"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[Finance.Transactions.FastTag.Select]", parameters))
            {
                yield return GetMainModel(dr);
            }
        }

        public IEnumerable<FastTagTollModel> GetUploadView(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Operation", "UploadView"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[Finance.Transactions.FastTag.Select]", parameters))
            {
                yield return GetMainModel(dr);
            }
        }

        public FastTagTollModel GetUploadViewModel(int? FastTagUploadID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Operation", "UploadViewModel"),
                new SqlParameter("@FastTagUploadID", FastTagUploadID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[Finance.Transactions.FastTag.Select]", parameters))
            {
                return GetMainModel(dr);
            }
            return null;
        }

        public IEnumerable<FastTagModel> GetUploadViewCollection(int? FastTagUploadID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Operation", "UploadViewCollection"),
                new SqlParameter("@FastTagUploadID", FastTagUploadID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[Finance.Transactions.FastTag.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<FastTagTollModel> GetProcessView(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Operation", "ProcessView"),
                new SqlParameter("@BranchID", BranchID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[Finance.Transactions.FastTag.Select]", parameters))
            {
                yield return GetMainModel(dr);
            }
        }

        public FastTagTollModel GetProcessViewModel(int? FastTagProcessID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Operation", "ProcessViewModel"),
                new SqlParameter("@FastTagProcessID", FastTagProcessID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[Finance.Transactions.FastTag.Select]", parameters))
            {
                return GetMainModel(dr);
            }
            return null;
        }

        public IEnumerable<FastTagModel> GetProcessViewCollection(int? FastTagProcessID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Operation", "ProcessViewCollection"),
                new SqlParameter("@FastTagProcessID", FastTagProcessID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[Finance.Transactions.FastTag.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<FastTagModel> SelectByBranch(int? FastTagUploadID, int BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Operation", "ForBranch"),
                new SqlParameter("@FastTagUploadID", FastTagUploadID),
                new SqlParameter("@BranchID", BranchID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[Finance.Transactions.FastTag.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public FastTagTollModel UpdateNew(FastTagTollModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "New"),
               new SqlParameter("@FastTagUploadID", model.FastTagUploadID),
               new SqlParameter("@DocumentNumber", model.DocumentNumber),
               new SqlParameter("@DocDate", model.DocumentDate),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@CoaID", model.PaymentCoaID),
               new SqlParameter("@ArdCode", model.PaymentArdCode),
               new SqlParameter("@PaymentMode", model.PaymentMode),
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

        public FastTagTollModel UpdateProcess(FastTagTollModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "Process"),
               new SqlParameter("@FastTagUploadID", model.FastTagUploadID),
               new SqlParameter("@DocumentNumber", model.DocumentNumber),
               new SqlParameter("@DocDate", model.DocumentDate),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@CoaID", model.PaymentCoaID),
               new SqlParameter("@ArdCode", model.PaymentArdCode),
               new SqlParameter("@PaymentMode", model.PaymentMode),
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
                FastTagUploadID = dr?.GetInt32("FastTagUploadID"),
                DocumentNumber = dr.GetString("DocNumber"),
                ProcessDocumentNumber = dr.GetString("ProcessDocNumber"),
                DocumentDate = dr?.GetDateTime("DocDate"),
                BranchID = dr?.GetInt32("BranchID"),
                NatureOfTransaction = dr.GetString("NatureOfTransaction"),
                PaymentArdCode = dr.GetString("ArdCode"),
                PaymentCoaID = dr?.GetInt32("CoaID"),
                TotalAmount = dr?.GetDecimal("TotalAmount"),
                PaymentMode = dr.GetString("PaymentMode"),
                PaymentTool = dr.GetString("PaymentTool"),
                BankCharges = dr?.GetDecimal("BankCharges"),
                Narration = dr.GetString("Narration"),
                UserInfo =
                {
                    RecordStatus = dr?.GetByte("RecordStatus"),
                    UserID = dr.GetString("UserID")
                }
            };
        }

        private FastTagModel GetModel(IDataRecord dr)
        {
            return new FastTagModel
            {
                FastTagTollID = dr?.GetInt32("FastTagTollID"),
                TransactionDateTime = dr.GetDateTime("TollCrossTime"),
                NumberPlate = dr.GetString("RegNo"),
                BranchID = dr.GetInt32("BranchID"),
                BranchName = dr.GetString("BranchName"),
                TripID = dr.GetInt64("TripID"),
                TruckID = dr.GetInt32("TruckID"),
                TripPrefix = dr.GetString("TripPrefix"),
                TripNumber = dr.GetInt64("TripNumber"),
                PlazaCode = dr.GetString("PlazaCode"),
                Description = dr.GetString("Description"),
                TransactionID = dr.GetString("TransactionID"),
                Reimbursable = dr.GetBoolean("Reimbursable"),
                DebitAmount = (dr.GetDecimal("DebitAmount") ?? 0),
                RecordStatus = dr.GetByte("RecordStatus"),
            };
        }
    }
}