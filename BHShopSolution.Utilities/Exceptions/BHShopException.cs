using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHShopSolution.Utilities.Exceptions
{
    public class BHShopException : Exception
    {
        public BHShopException() { }

        public BHShopException(string message)
            : base(message) { }

        public BHShopException(string message, Exception inner)
            : base(message, inner) { }
    }
}
