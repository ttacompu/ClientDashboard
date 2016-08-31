using CGSH.ClientDashboard.BusinessEntitity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGSH.ClientDashboard.Interface.DataAccess
{
    public interface ITimekeeperDataAccess
    {
        Task<List<Person>> GetPartnersAndSeniorCounsel(string clientGroupNumber, DateTime startDate, DateTime endDate, int threshold);
        Task<List<Person>> Get(string clientGroupNumber, DateTime startDate,DateTime endDate,int threshold);
    }
}
