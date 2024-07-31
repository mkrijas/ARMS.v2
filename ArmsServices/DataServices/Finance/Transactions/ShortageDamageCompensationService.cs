using ArmsModels.BaseModels.Finance.Transactions;
using ArmsServices;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ArmsServices.DataServices;
using Core.BaseModels.Finance.Transactions;
using ArmsModels.BaseModels.General;
using ArmsServices.DataServices.General;
using ArmsModels.BaseModels;


namespace DAL.DataServices.Finance.Transactions
{
    public class ShortageDamageCompensationService : IShortageDamageCompensationService
    {
        IDbService Iservice;
        IConfigTable configTable;
        public ShortageDamageCompensationService(IDbService iservice, IConfigTable IconfigTable)
        {
            Iservice = iservice;
            configTable = IconfigTable;
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DamageID", ID),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.ShortageDamageCompensation.Delete]", parameters);

        }

        public IEnumerable<ShortageDamageCompensationModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),

            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.ShortageDamageCompensation.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<ShortageDamageCompensationModel> SelectByGcSetID(long? GcSetID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByGcSetID"),
               new SqlParameter("@GcSetID", GcSetID)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.ShortageDamageCompensation.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<ShortageDamageCompensationModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)

            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.ShortageDamageCompensation.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<ShortageDamageCompensationModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByUnapproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)

            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.ShortageDamageCompensation.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public ShortageDamageCompensationModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DamageID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            ShortageDamageCompensationModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.ShortageDamageCompensation.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
        public int Approve(int? DamageID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DamageID", DamageID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.ShortageDamageCompensation.Approve]", parameters);
        }

        public int Reverse(int? DamageID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DamageID", DamageID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.ShortageDamageCompensation.Reverse]", parameters);
        }
        public ShortageDamageCompensationModel Update(ShortageDamageCompensationModel model)
        {        
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DamageID", model.DamageID),
               new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
               new SqlParameter("@Reference", model.Reference),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DocDate", model.DocumentDate),
               new SqlParameter("@DocNumber", model.DocumentNumber),
               new SqlParameter("@GcSetID", model.GcSetID),
               new SqlParameter("@DriverID", model.DriverID),
               new SqlParameter("@FilePath", model.FileName),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@DamagedQty", model.DamagedQty),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.ShortageDamageCompensation.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
        private ShortageDamageCompensationModel GetModel(IDataRecord dr)
        {
            return new ShortageDamageCompensationModel
            {
                DamageID = dr.GetInt32("DamageID"),
                NatureOfTransaction = dr.GetString("NatureOfTransaction"),
                GcSetID = dr.GetInt64("GcSetID"),
                TripID = dr.GetInt64("TripID"),
                DriverID = dr.GetInt32("DriverID"),
                Reference = dr.GetString("Reference"),
                AuthLevelId = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
                BranchID = dr.GetInt32("BranchID"),
                FileName = dr.GetString("FilePath"),
                DocumentDate = dr.GetDateTime("DocDate"),
                DocumentNumber = dr.GetString("DocNumber"),
                MID = dr.GetInt32("MID"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
                DamagedQty = dr.GetDecimal("DamagedQty"),
                Narration = dr.GetString("Narration"),

                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        public IEnumerable<TripAdvanceModel> GetDamageReceivables(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@Operation", "GetDamageReceivables"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.ShortageDamageCompensation.Select]", parameters))
            {
                yield return new TripAdvanceModel()
                {
                    GcSetID = dr.GetInt64("GcSetID"),
                    BranchID = dr.GetInt32("BranchID"),
                    DriverID = dr.GetInt32("DriverID"),
                    DriverName = dr.GetString("DriverName"),
                    DriverMobile = dr.GetString("Mobile"),
                    CoaID = dr.GetInt32("CoaID"),
                    AccountCode = dr.GetString("AccountCode"),
                    AccountName = dr.GetString("AccountName"),
                    UsageCode = dr.GetString("UsageCode"),
                    UsageDescription = dr.GetString("Description"),
                    Amount = dr.GetDecimal("Amount"),
                    GcNo = dr.GetString("GcNo"),
                    TripID = dr.GetInt64("TripID"),
                    TripNo = dr.GetString("TripNo"),
                    DocType = dr.GetString("DocType")
                };
            }
        }
    }
}
