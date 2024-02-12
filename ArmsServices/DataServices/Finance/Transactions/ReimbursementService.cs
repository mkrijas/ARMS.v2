using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using ArmsServices;
using ArmsServices.DataServices;

namespace DAL.DataServices.Finance
{
    public class ReimbursementService : IReimbursementService
    {
        IDbService Iservice;


        public ReimbursementService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Approve(int? ID, string UserID, string Remarks)
        {
            throw new NotImplementedException();
        }

        public int Delete(int? ID, string UserID)
        {
            throw new NotImplementedException();
        }

        public int Reverse(int? ID, string UserID, string Remarks)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InterBranchReimbursementModel> Select(int? BranchID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InterBranchReimbursementModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            throw new NotImplementedException();
        }

        public InterBranchReimbursementModel SelectByID(int? ID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InterBranchReimbursementModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            throw new NotImplementedException();
        }

        public InterBranchReimbursementModel Update(InterBranchReimbursementModel model)
        {
            throw new NotImplementedException();
        }
    }
}
