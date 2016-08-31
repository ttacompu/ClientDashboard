using CGSH.ClientDashboard.BusinessEntitity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGSH.ClientDashboard.Interface.DataAccess
{
    public interface ISearchDataAccess
    {
        Task<List<ClientGroup>> Get(string searchString);
    }
}
