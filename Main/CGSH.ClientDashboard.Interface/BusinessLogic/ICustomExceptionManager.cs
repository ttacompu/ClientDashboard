using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGSH.ClientDashboard.Interface.BusinessLogic
{
    public interface ICustomExceptionManager
    {
        bool HandleException(Exception exceptionToHandle, string policyName, out Exception NewException );
    }
}
