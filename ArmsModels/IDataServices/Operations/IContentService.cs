using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IContentService
    {
        Task<ContentModel> Update(ContentModel model);
        Task<ContentModel> SelectByID(int? ID);
        Task<int> Delete(int? ContentID, string UserID);
        IAsyncEnumerable<ContentModel> Select(int? ContentID);
    }
}