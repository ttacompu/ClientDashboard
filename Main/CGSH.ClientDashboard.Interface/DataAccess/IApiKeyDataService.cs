using CGSH.ClientDashboard.BusinessEntitity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGSH.ClientDashboard.Interface.DataAccess
{
    public interface IApiKeyDataService
    {
        Task<List<ApiKey>> All();
        Task<ApiKey> Get(long id);

        Task<ApiKey> Save(ApiKey item);
        Task<bool> Delete(long id);
    }
}
