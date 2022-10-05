using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IContraService
    {
        ContraModel Update(ContraModel model);
        ContraModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);
        IEnumerable<ContraModel> Select();
        int Approve(int? PID, string UserID);

    }
    public class ContraService : IContraService
    {
        IDbService Iservice;


        public ContraService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ContraID", ID),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.Contra.Delete]", parameters);

        }

        public IEnumerable<ContraModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),

            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Contra.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public ContraModel SelectByID(int? ID)
        {
            throw new NotImplementedException();
        }
        public int Approve(int? PID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ContraID", PID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Status", 1)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.Contra.Approve]", parameters);
        }
        public ContraModel Update(ContraModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ContraID", model.ContraID),
               new SqlParameter("@ContraModeHome", model.ContraModeHome),
               new SqlParameter("@CoaIDHome", model.CoaIDHome),
               new SqlParameter("@BranchIDOther", model.BranchIDOther),
               new SqlParameter("@ContraModeOther", model.ContraModeOther),
               new SqlParameter("@CoaIDOther", model.CoaIDOther),
               new SqlParameter("@Reference", model.EntryReference),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DocDate", model.DocumentDate),
               new SqlParameter("@DocNumber", model.DocumentNumber),
               new SqlParameter("@CostCenter", model.CostCenter),
               new SqlParameter("@Dimension", model.Dimension),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Contra.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
        private ContraModel GetModel(IDataRecord dr)
        {
            return new ContraModel
            {
              
                ContraID = dr.GetInt32("ContraID"),
                ContraModeHome = dr.GetString("ContraModeHome"),
                CoaIDHome = dr.GetInt32("CoaIDHome"),
                BranchIDOther = dr.GetInt32("BranchIDOther"),
                ContraModeOther = dr.GetString("ContraModeOther"),
                CoaIDOther = dr.GetInt32("CoaIDOther"),
                EntryReference = dr.GetString("EntryReference"),
                BranchID = dr.GetInt32("BranchID"),
                // BranchName = dr.GetString("BranchName"),
                ApprovedInfo = new ArmsModels.SharedModels.UserInfoModel()
                {
                    RecordStatus = dr.GetByte("ApprovedStatus"),
                    TimeStampField = dr.GetDateTime("ApprovedOn"),
                    UserID = dr.GetString("ApprovedBy"),
                },
                DocumentDate = dr.GetDateTime("DocDate"),
                DocumentNumber = dr.GetString("DocumentNumber"),
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
