using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Reflection.PortableExecutable;
using System.Reflection;


namespace ArmsServices.DataServices
{
    public class TripAdvanceService : ITripAdvanceService
    {
        IDbService Iservice;

        public TripAdvanceService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int? TripAdvanceID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripFuelID", TripAdvanceID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Operation.Trips.Advance.Delete]", parameters);
        }

        public TripAdvanceModel Select(int? TripAdvanceID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripAdvanceID),
               new SqlParameter("@Operation", "SelectByID"),
            };
            TripAdvanceModel model = null;
            foreach (var reader in Iservice.GetDataReader("[usp.Operation.Trips.Advance.Select]", parameters))
            {
                model = GetModel(reader);
            }
            return model;
        }

        public IEnumerable<TripAdvanceModel> SelectByTrip(long? TripID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripID),
               new SqlParameter("@Operation", "SelectByTrip"),
            };
            foreach (var reader in Iservice.GetDataReader("[usp.Operation.Trips.Advance.Select]", parameters))
            {
                yield return GetModel(reader);
            }
        }

        public TripAdvanceModel GetTotal(long? TripID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripID),
               new SqlParameter("@Operation", "GetTotal"),
            };
            TripAdvanceModel model = new();
            foreach (var reader in Iservice.GetDataReader("[usp.Operation.Trips.Advance.Select]", parameters))
            {
                model = GetModel(reader);
            }
            return model;
        }

        public TripAdvanceModel Update(TripAdvanceModel model)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripAdvanceID", model.TripAdvanceID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@TripID", model.TripID),
               new SqlParameter("@DriverID", model.DriverID),
               new SqlParameter("@Amount", model.Amount),
               new SqlParameter("@CoaID", model.CoaID),
               new SqlParameter("@UsageCode", model.UsageCode),
               new SqlParameter("@DocumentTypeID", model.DocumentTypeID),
               new SqlParameter("@DocumentID", model.DocumentID),
               new SqlParameter("@DocType", model.DocType),
            };

            foreach (var reader in Iservice.GetDataReader("[usp.Operation.Trips.Advance.Update]", parameters))
            {
                model = GetModel(reader);
            }
            return model;
        }

        public TripAdvanceModel Cancel(int? @DocumentTypeID, int? @DocumentID)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DocumentTypeID", @DocumentTypeID),
               new SqlParameter("@DocumentID", @DocumentID),
            };
            TripAdvanceModel model = new();
            foreach (var reader in Iservice.GetDataReader("[usp.Operation.Trips.Advance.Cancel]", parameters))
            {
                model = GetModel(reader);
            }
            return model;
        }

        public IEnumerable<TripAdvanceModel> GetAdvanceReceivables(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@Operation", "GetUnsettled"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.Trips.Advance.Select]", parameters))
            {
                yield return new TripAdvanceModel()
                {
                    TripID = dr.GetInt64("TripID"),
                    BranchID = dr.GetInt32("BranchID"),
                    TripNo = dr.GetString("TripNo"),
                    DriverID = dr.GetInt32("DriverID"),
                    DriverName = dr.GetString("DriverName"),
                    DriverMobile = dr.GetString("Mobile"),
                    CoaID = dr.GetInt32("CoaID"),
                    AccountCode = dr.GetString("AccountCode"),
                    AccountName = dr.GetString("AccountName"),
                    UsageCode = dr.GetString("UsageCode"),
                    UsageDescription = dr.GetString("Description"),
                    Amount = dr.GetDecimal("Amount"),   
                    DocType = dr.GetString("DocType")
                };
            }
        }

        private TripAdvanceModel GetModel(IDataRecord reader)
        {
            return new TripAdvanceModel
            {
                TripAdvanceID = reader.GetInt64("TripAdvanceID"),
                BranchID = reader.GetInt32("BranchID"),
                TripID = reader.GetInt64("TripID"),
                DriverID = reader.GetInt32("DriverID"),
                Amount = reader.GetDecimal("Amount"),
                CoaID = reader.GetInt32("CoaID"),
                UsageCode = reader.GetString("UsageCode"),
                DocumentTypeID = reader.GetInt32("DocumentTypeID"),
                DocumentID = reader.GetInt32("DocumentID"),
                DocType = reader.GetString("DocType")
            };
        }
    }
}
