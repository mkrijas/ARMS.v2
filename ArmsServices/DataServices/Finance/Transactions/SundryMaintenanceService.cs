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
    public class SundryMaintenanceService : ISundryMaintenanceService
    {
        IDbService Iservice;

        public SundryMaintenanceService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Approve(int? ID, string UserID, string Remark)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remark)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.SundryMaintenance.Approve]", parameters);
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.SundryMaintenance.Delete]", parameters);
        }

        public IEnumerable<SundryMaintenanceEntryModel> GetEntries(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetEntries"),
               new SqlParameter("@ID", ID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.SundryMaintenance.Select]", parameters))
            {
                yield return new SundryMaintenanceEntryModel()
                {
                    Amount = dr.GetDecimal("Amount"),
                    CGST = dr.GetDecimal("CGST"),
                    IGST = dr.GetDecimal("IGST"),
                    SGST = dr.GetDecimal("SGST"),
                    CoaID = dr.GetInt32("CoaID"),
                    ParentID = dr.GetInt32("ParentID"),
                    TDS = dr.GetDecimal("TDS"),
                    GstRate = dr.GetDecimal("GstRate"),
                    InvoiceNo = dr.GetString("InvoiceNo"),
                    InvoiceDate = dr.GetDateTime("InvoiceDate"),
                    Job = dr.GetInt32("JobID"),
                    JobTitle = dr.GetString("JobTitle"),
                    TruckRegNo = dr.GetString("TruckRegNo"),
                    TruckID = dr.GetInt32("TruckID"),
                    BranchID = dr.GetInt32("BranchID"),
                    UsageCode = dr.GetString("UsageCode"),
                    SubArdCode = dr.GetString("SubArdCode"),
                    UsageCodeDescription = dr.GetString("UsageDescription"),
                    ID = dr.GetInt32("ID"),
                    CostCenterVal = dr.GetString("CostCenter"),
                    DimensionVal = dr.GetString("Dimension"),                    
                    CostCenter  = dr.GetInt32("CostCenterID"),
                    Dimension = dr.GetInt32("DimensionID"),                   
                };
            }
        }

        public int Reverse(int? ID, string UserID, String Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.SundryMaintenance.Reverse]", parameters);
        }

        public IEnumerable<SundryMaintenanceModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.SundryMaintenance.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public SundryMaintenanceModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            SundryMaintenanceModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.SundryMaintenance.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<SundryMaintenanceModel> SelectByParty(int? PartyID, int? PartyBranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByParty"),
               new SqlParameter("@PartyID", PartyID),
               new SqlParameter("@PartyBranchID", PartyBranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.SundryMaintenance.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<SundryMaintenanceModel> SelectByPeriod(DateTime? begin, DateTime? end)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByPeriod"),
               new SqlParameter("@begin", begin),
               new SqlParameter("@end", end),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.SundryMaintenance.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public SundryMaintenanceModel Update(SundryMaintenanceModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@BreakDownType", model.BreakDownType),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DocumentDate", model.DocumentDate),
               new SqlParameter("@DocumentNumber", model.DocumentNumber),
               new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
               new SqlParameter("@Expenses", model.Entries.ToDataTable()),
               new SqlParameter("@FilePath", model.FileName),
               new SqlParameter("@PartyID", model.PartyInfo.PartyID),
               new SqlParameter("@PartyCode", model.PartyInfo.PartyCode),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.SundryMaintenance.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private SundryMaintenanceModel GetModel(IDataRecord dr)
        {
            return new SundryMaintenanceModel
            {
                ID = dr.GetInt32("ID"),
                BreakDownType = dr.GetString("BreakDownType"),
                BranchID = dr.GetInt32("BranchID"),
                DocumentDate = dr.GetDateTime("DocDate"),
                DocumentNumber = dr.GetString("DocNumber"),

                AuthLevelId = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
                MID = dr.GetInt32("MID"),
                FileName = dr.GetString("FilePath"),

                TotalAmount = dr.GetDecimal("TotalAmount"),
                Narration = dr.GetString("Narration"),
                PartyInfo = new PartyModel()
                {
                    PartyID = dr.GetInt32("PartyID"),
                    TradeName = dr.GetString("TradeName"),
                    PartyCode = dr.GetString("PartyCode"),
                },
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }


        public IEnumerable<SundryMaintenanceModel> Select(int? BranchID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SundryMaintenanceModel> SelectByApproved(int? BranchID, int? NumberOfRecords, bool InterBranch, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@InterBranch", InterBranch),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.SundryMaintenance.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }
        public IEnumerable<SundryMaintenanceModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, bool InterBranch, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByUnApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@InterBranch", InterBranch),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.SundryMaintenance.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }
    }
}