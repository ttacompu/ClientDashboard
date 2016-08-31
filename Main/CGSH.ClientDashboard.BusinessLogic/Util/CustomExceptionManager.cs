using CGSH.ClientDashboard.Interface.BusinessLogic;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGSH.ClientDashboard.BusinessLogic.Util
{
    public class CustomExceptionManager : ICustomExceptionManager
    {
        ExceptionManager _exceptionManager;
        public CustomExceptionManager(ExceptionManager exceptionManager) {
            _exceptionManager = exceptionManager;
        }
        public bool HandleException(Exception exceptionToHandle, string policyName, out Exception NewException)
        {
            return _exceptionManager.HandleException(exceptionToHandle, policyName, out NewException);
        }
    }
}
