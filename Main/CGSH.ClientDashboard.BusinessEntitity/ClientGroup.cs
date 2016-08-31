using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGSH.ClientDashboard.BusinessEntitity
{
    /// <summary>
    /// Client Group Class
    /// </summary>
    public class ClientGroup
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ClientGroup()
        {
            this.Clients = new List<Client>();
        }

        /// <summary>
        /// Clients
        /// </summary>
        public List<Client> Clients { get; set; }

        /// <summary>
        /// Client Group Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Client Group Number
        /// </summary>
        public string Number { get; set; }
    }
}
