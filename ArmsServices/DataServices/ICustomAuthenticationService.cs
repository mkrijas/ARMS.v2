using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public interface ICustomAuthenticationService
    {
        Task Logout();
    }
}
