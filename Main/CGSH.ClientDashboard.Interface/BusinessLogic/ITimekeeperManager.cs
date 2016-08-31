using CGSH.ClientDashboard.BusinessEntitity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGSH.ClientDashboard.Interface.BusinessLogic
{
    public interface ITimekeeperManager
    {
        Task<List<Person>> GetPartnersAndSeniorCounsel(string apiKey, string clientGroupNumber, DateTime startDate, DateTime endDate, int threshold);
        Task<List<Person>> Get(string apiKey, string clientGroupNumber, DateTime startDate, DateTime endDate, int threshold);

        Task<string> ValidateInputParams(string apiKey, string clientGroupNumber, DateTime startDate, DateTime endDate, int threshold);
    }
}
