using System;

namespace InventoryManagement.Exceptions
{
    [Serializable]
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException(): base("An account for this user is already existing")
        {

        }
        public UserAlreadyExistsException(string username): base(string.Format("Username {0} already exists!", username))
        {

        }
    }
}
