using CGSH.ClientDashboard.BusinessEntitity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGSH.ClientDashboard.Interface.BusinessLogic
{
    public interface IApiKeyManager
    {
        Task<List<ApiKey>> All();
        Task<ApiKey> Get(long id);
        Task<bool> Delete(long id);

        Task<ApiKey> Save(ApiKey item);
        Task<bool> IsValid(string apiKey);
    }
        
}
