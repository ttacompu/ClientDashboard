using CGSH.ClientDashboard.BusinessEntitity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGSH.ClientDashboard.Interface.DataAccess
{
    public interface IEntityDataAccess
    {
         Task<List<Client>> Get(string searchString);
    }
}
