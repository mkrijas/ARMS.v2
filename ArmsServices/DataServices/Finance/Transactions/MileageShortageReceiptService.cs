using ArmsModels.BaseModels.Finance.Transactions;
using ArmsServices;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ArmsServices.DataServices;
using Core.BaseModels.Finance.Transactions;

namespace DAL.DataServices.Finance.Transactions
{
    public class MileageShortageReceiptService: IMileageShortageReceiptService
    {
        IDbService Iservice;

        public MileageShortageReceiptService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@MileageShortageReceiptID", ID),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.MileageShortageReceipt.Delete]", parameters);

        }

        public IEnumerable<MileageShortageReceiptModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),

            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.MileageShortageReceipt.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<MileageShortageReceiptModel> SelectByApproved(int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)

            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.MileageShortageReceipt.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<MileageShortageReceiptModel> SelectByUnapproved(int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByUnapproved"),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)

            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.MileageShortageReceipt.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public MileageShortageReceiptModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@MileageShortageReceiptID", ID),
               new SqlParameter("@Operation", "GetEntries")
            };
            MileageShortageReceiptModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.MileageShortageReceipt.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
        public int Approve(int? MileageShortageReceiptID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@MileageShortageReceiptID", MileageShortageReceiptID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.MileageShortageReceipt.Approve]", parameters);
        }

        public int Reverse(int? MileageShortageReceiptID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@MileageShortageReceiptID", MileageShortageReceiptID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.MileageShortageReceipt.Reverse]", parameters);
        }
        public MileageShortageReceiptModel Update(MileageShortageReceiptModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@MileageShortageReceiptID", model.MileageShortageReceiptID),
               new SqlParameter("@ReceiptMode", model.ReceiptMode),
               new SqlParameter("@ArdCode", model.ArdCode),
               new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
               new SqlParameter("@Reference", model.Reference),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DocDate", model.DocumentDate),
               new SqlParameter("@DocNumber", model.DocumentNumber),
               new SqlParameter("@CostCenter", model.CostCenter),
               new SqlParameter("@Dimension", model.Dimension),
               new SqlParameter("@TripID", model.TripID),
               new SqlParameter("@DriverID", model.DriverID),
               new SqlParameter("@AllottedMileage", model.AllottedMileage),
               new SqlParameter("@AllottedDistance", model.AllottedDistance),
               new SqlParameter("@FuelPrice", model.FuelPrice),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.MileageShortageReceipt.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
        private MileageShortageReceiptModel GetModel(IDataRecord dr)
        {
            return new MileageShortageReceiptModel
            {
                MileageShortageReceiptID = dr.GetInt32("MileageShortageReceiptID"),
                NatureOfTransaction = dr.GetString("NatureOfTransaction"),
                ReceiptMode = dr.GetString("ReceiptMode"),
                ArdCode = dr.GetString("ArdCode"),
                TripID = dr.GetInt32("TripID"),
                DriverID = dr.GetInt32("DriverID"),
                AllottedMileage = dr.GetDecimal("AllottedMileage"),
                AllottedDistance = dr.GetDecimal("AllottedDistance"),
                FuelPrice = dr.GetDecimal("FuelPrice"),
                Reference = dr.GetString("Reference"),
                AuthLevelId = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
                BranchID = dr.GetInt32("BranchID"),
                DocumentDate = dr.GetDateTime("DocDate"),
                DocumentNumber = dr.GetString("DocNumber"),
                MID = dr.GetInt32("MID"),
                CostCenter = dr.GetInt32("CostCenter"),
                Dimension = dr.GetInt32("Dimension"),
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
    }
}
