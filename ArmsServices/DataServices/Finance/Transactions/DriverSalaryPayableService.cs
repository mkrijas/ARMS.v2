using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Reflection;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

namespace ArmsServices.DataServices
{
    public class DriverSalaryPayableService : IDriverSalaryPayableService
    {
        IDbService Iservice;

        public DriverSalaryPayableService(IDbService iservice)
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
            return Iservice.ExecuteNonQuery("[usp.Finance.DriverSalaryPayable.Approve]", parameters);
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.DriverSalaryPayable.Delete]", parameters);
        }

        public IEnumerable<DriverSalaryPayableListModel> GetDetails(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetDetails"),
               new SqlParameter("@ID", ID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.DriverSalaryPayable.Select]", parameters))
            {
                yield return new DriverSalaryPayableListModel()
                {
                    ID = dr.GetInt32("ID"),
                    HeaderID = dr.GetInt32("HeaderID"),
                    DriverID = dr.GetInt32("DriverID"),
                    DriverName = dr.GetString("DriverName"),
                    DriverCode = dr.GetString("DriverCode"),
                    WorkDays = dr.GetDecimal("WorkDays"),
                    NoOfTrips = dr.GetInt32("NoOfTrips"),
                    RunKM = dr.GetInt32("RunKM"),
                    GrossSalary = dr.GetDecimal("GrossSalary"),
                    Deduction = dr.GetDecimal("Deduction"),
                    NetSalary = dr.GetDecimal("NetSalary"),
                    Reference = dr.GetString("Reference"),
                };
            }
        }

        public IEnumerable<DriverSalaryPayableListModel> GetLists(int? BranchID, DateTime? FromDate, DateTime? ToDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@FromDate", FromDate),
               new SqlParameter("@ToDate", ToDate),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.DriverSalaryPayable.List]", parameters))
            {
                yield return new DriverSalaryPayableListModel()
                {
                    DriverID = dr.GetInt32("DriverID"),
                    DriverName = dr.GetString("DriverName"),
                    DriverCode = dr.GetString("DriverCode"),
                    WorkDays = dr.GetDecimal("WorkDays"),
                    NoOfTrips = dr.GetInt32("NoOfTrips"),
                    RunKM = dr.GetInt32("RunKM"),
                };
            }
        }

        public IEnumerable<DriverSalaryPayableModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.DriverSalaryPayable.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<DriverSalaryPayableModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.DriverSalaryPayable.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<DriverSalaryPayableModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByUnApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.DriverSalaryPayable.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public DriverSalaryPayableModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            DriverSalaryPayableModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.DriverSalaryPayable.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public DriverSalaryPayableModel Update(DriverSalaryPayableModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DocumentDate", model.DocumentDate),
               new SqlParameter("@DocumentNumber", model.DocumentNumber),
               new SqlParameter("@FromDate", model.FromDate),
               new SqlParameter("@ToDate", model.ToDate),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@UserID", model.UserInfo.UserID),
               new SqlParameter("@FilePath", model.FileName),
               new SqlParameter("@Particulars", model.DriversLists.ToDataTable()),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.DriverSalaryPayable.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private DriverSalaryPayableModel GetModel(IDataRecord dr)
        {
            return new DriverSalaryPayableModel
            {
                ID = dr.GetInt32("ID"),
                BranchID = dr.GetInt32("BranchID"),
                DocumentDate = dr.GetDateTime("DocumentDate"),
                DocumentNumber = dr.GetString("DocumentNumber"),
                FromDate = dr.GetDateTime("FromDate"),
                ToDate = dr.GetDateTime("ToDate"),
                AuthLevelId = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
                MID = dr.GetInt32("MID"),
                FileName = dr.GetString("FilePath"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
                Narration = dr.GetString("Narration"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        public int Reverse(int? ID, string UserID, string Remarks)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DriverSalaryPayableModel> Select(int? BranchID)
        {
            throw new NotImplementedException();
        }

        public int RemoveFile(int? ID, string UserID)
        {
            throw new NotImplementedException();
        }

        IEnumerable<DriverSalaryPayableModel> IbaseInterface<DriverSalaryPayableModel>.SelectByApproved(int? BranchID, int? NumberOfRecords, bool InterBranch, string searchTerm)
        {
            throw new NotImplementedException();
        }

        IEnumerable<DriverSalaryPayableModel> IbaseInterface<DriverSalaryPayableModel>.SelectByUnapproved(int? BranchID, int? NumberOfRecords, bool InterBranch, string searchTerm)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DriverPendingSalaryModel> GetDriverPendingSalary(int? DriverID, int? BranchID, string UsageCode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DriverID", DriverID),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@UsageCode", UsageCode),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.PendingSalary.Select]", parameters))
            {
                yield return new DriverPendingSalaryModel()
                {
                    Amount = dr.GetDecimal("Amount"),
                    EntryRef = dr.GetString("EntryRef"),
                    DocumentDate = dr.GetDateTime("DocumentDate"),
                    ArdCode = dr.GetString("ArdCode"),
                    Driver = new DriverModel()
                    {
                        DriverID = dr.GetInt32("DriverID"),
                        DriverCode = dr.GetString("DriverCode"),
                        Mobile = dr.GetString("Mobile"),
                        DriverName = dr.GetString("DriverName"),
                    }
                };
            }
        }
        
    }
}