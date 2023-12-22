using ArmsServices;
using Core.IDataServices.Finance;

namespace DAL.DataServices.Finance
{
    public class CancellationReasonCodeService : ICancellationReasonCodeService
    {
        IDbService Iservice;

        public CancellationReasonCodeService(IDbService iservice)
        {
            Iservice = iservice;
        }
    }
}
