using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace CGSH.ClientDashboard.Exceptions
{
    /// <summary>
    /// Validation Exception
    /// </summary>
    [Serializable]
    public class ValidationException : System.Exception
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ValidationException()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ValidationException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ValidationException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        protected ValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

    }
}
