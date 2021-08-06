using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Exceptions
{
    public class PasswordDontMatchException: Exception
    {
        public PasswordDontMatchException() : base("Password don't match!")
        {

        }
    }
}
