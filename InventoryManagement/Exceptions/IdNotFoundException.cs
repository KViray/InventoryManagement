using System;

namespace InventoryManagement.Exceptions
{
    [Serializable]
    public class IdNotFoundException: Exception
    {
        public IdNotFoundException(): base("Id not found")
        {

        }
        public IdNotFoundException(string type): base(string.Format("Id not found on {0}", type))
        {

        }
    }
}
