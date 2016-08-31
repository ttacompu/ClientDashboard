using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGSH.ClientDashboard.BusinessEntitity
{
    /// <summary>
    /// Client Class
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Client()
        {
            this.Matters = new List<Matter>();
        }

        /// <summary>
        /// Matters
        /// </summary>
        public List<Matter> Matters { get; set; }

        /// <summary>
        /// Client Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Client Number
        /// </summary>
        public string Number { get; set; }
    }
}
