using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Exceptions
{
    public class InvalidUsernamePasswordException : Exception
    {
        public InvalidUsernamePasswordException() : base("Invalid Username/Password!!")
        {

        }
    }
}
