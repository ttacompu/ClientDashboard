using System.Threading.Tasks;
using System.Web.Http;

namespace CGSH.ClientDashboard.Interface.UI
{
    public interface IClientDashboardWebAPI<T> where T : class
    {
        Task<IHttpActionResult> All();
        Task<IHttpActionResult> Save(T t);
        Task<IHttpActionResult> Update(T t);
        Task<IHttpActionResult> Delete(long t);
    }
}
