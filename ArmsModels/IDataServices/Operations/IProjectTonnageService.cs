using Core.BaseModels.Operations;
using System;
using System.Collections.Generic;

namespace Core.IDataServices.Operations
{
    public interface IProjectTonnageService
    {
        IEnumerable<ProjectTonnageModel> Select(int? BranchID);
        IEnumerable<ProjectTonnageModel> SelectProjectedTonnage(string SelectedBranches, DateTime? Date);
    }
}