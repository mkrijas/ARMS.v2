using ArmsModels.BaseModels;
using ArmsServices;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Core.BaseModels.Operations;
using ArmsModels.SharedModels;
using Core.IDataServices.Operations;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;

namespace DAL.DataServices.Operations
{
    public class ProjectTonnageService : IProjectTonnageService
    {
        IDbService Iservice;
        public ProjectTonnageService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public IEnumerable<ProjectTonnageModel> Select(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.ProjectTonnage.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<ProjectTonnageModel> SelectProjectedTonnage(string SelectedBranches, DateTime? Date)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchIDs", SelectedBranches),
               new SqlParameter("@Date", Date),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[rptProjectedTonnage]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        private ProjectTonnageModel GetModel(IDataRecord dr)
        {
            return new ProjectTonnageModel
            {
                ID = dr.GetInt32("ID"),
                Branch = new BranchModel
                {
                    BranchID = dr.GetInt32("BranchID"),
                    BranchName = dr.GetString("BranchName"),
                },
                BodyType = dr.GetString("BodyType"),
                Wheels = dr.GetInt32("Wheels"),
                Tonnage = dr.GetDecimal("Tonnage"),
                CountTrips = dr.GetInt32("CountTrips"),
                Freight = dr.GetDecimal("Freight"),
                ProjectedTrips = dr.GetInt32("ProjectedTrips"),
                ProjectedTonnage = dr.GetDecimal("ProjectedTonnage"),
                UserInfo = new UserInfoModel
                {
                    UserID = dr.GetString("UserID"),
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                }
            };
        }
    }
}
