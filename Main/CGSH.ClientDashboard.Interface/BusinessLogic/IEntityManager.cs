using CGSH.ClientDashboard.BusinessEntitity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CGSH.ClientDashboard.Interface.BusinessLogic
{
    public interface IEntityManager
    {
        Task<List<Client>> Get(string apiKey, string searchString);
    }
}
