using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGSH.ClientDashboard.BusinessEntitity
{
    public class ApiKey
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <remarks>
        /// Initialize objects
        /// </remarks>
        public ApiKey()
        {
            Id = 0;
            ExpirationDate = DateTime.Now;
        }

        /// <summary>
        /// Application Name
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Expiration Date
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Api ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Api Key
        /// </summary>
        public string Key { get; set; }

    }
}
