using CGSH.ClientDashboard.BusinessEntitity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGSH.ClientDashboard.Interface.BusinessLogic
{
    public interface ISearchManager
    {
        Task<List<ClientGroup>> Get(string apiKey, string searchString);
    }
}
