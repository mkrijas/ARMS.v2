using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface ITripFuelService
    {
        TripFuelModel Update(TripFuelModel model);
        int Delete(int? TripFuelID, string UserID);
        TripFuelModel Select(int? TripFuelID);
        IEnumerable<TripFuelModel> SelectByTrip(long? TripID);

    }

    public class TripFuelService : ITripFuelService
    {
        IDbService Iservice;

        public TripFuelService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int? TripFuelID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripFuelID", TripFuelID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Operation.Trips.Fuel.Delete]", parameters);
        }



        public TripFuelModel Select(int? TripFuelID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripFuelID),
                new SqlParameter("@Operation", "SelectByID"),
            };

            TripFuelModel model = null;
            foreach (var reader in Iservice.GetDataReader("[usp.Operation.Trips.Fuel.Select]", parameters))
            {
                model = GetModel(reader);
            }
            return model;
        }

        public IEnumerable<TripFuelModel> SelectByTrip(long? TripID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripID),
                new SqlParameter("@Operation", "SelectByTrip"),
            };

            foreach (var reader in Iservice.GetDataReader("[usp.Operation.Trips.Fuel.Select]", parameters))
            {
                yield return GetModel(reader);
            }
        }

        public TripFuelModel Update(TripFuelModel model)
        {
            
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", model.TripID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@EntryDate", model.EntryDate),
               new SqlParameter("@FuelItemID", model.FuelItemID),
               new SqlParameter("@InvTranID", model.InvTranID),
               new SqlParameter("@IsPurchase", model.IsPurchase),
            
               new SqlParameter("@Quantity", model.Quantity),
               new SqlParameter("@RatePerLitre", model.RatePerLitre),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@TripFuelID", model.TripFuelID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
               // Tax Purcahse
               new SqlParameter("@vendorID", model.PartyBranch.PartyID),
               new SqlParameter("@DocumentDate", model.EntryDate),
           
               new SqlParameter("@InvoiceDate", model.EntryDate),
               new SqlParameter("@InvoiceNo", model.InvoiceNo),
        
               new SqlParameter("@CostCenter", model.Costcenter),
                new SqlParameter("@Dimension", model.Dimension),
           
            };

            foreach (var reader in Iservice.GetDataReader("[usp.Operation.Trips.Fuel.Update]", parameters))
            {
                //model = GetModel(reader);
            }
            return model;
        }

        private TripFuelModel GetModel(IDataRecord reader)
        {
            return new TripFuelModel
            {
                TripFuelID = reader.GetInt32("TripFuelID"),
                EntryDate = reader.GetDateTime("EntryDate"),
                FuelItemID = reader.GetInt32("FuelItemID"),
                //InvTranID = reader.GetInt32("InvTranID"),
                IsPurchase = reader.GetBoolean("IsPurchase"),
                //PurchaseID = reader.GetInt32("PurchaseID"),
                Quantity = reader.GetDecimal("Quantity"),
                RatePerLitre = reader.GetDecimal("RatePerLitre"),
                TotalAmount = reader.GetDecimal("TotalAmount"),
                BranchID = reader.GetInt32("BranchID"),
                TripID = reader.GetInt32("TripID"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = reader.GetByte("RecordStatus"),
                    TimeStampField = reader.GetDateTime("TimeStamp"),
                    UserID = reader.GetString("UserID"),
                },
            };
        }
    }
}
