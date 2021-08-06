using System;

namespace InventoryManagement.Exceptions
{
    [Serializable]
    public class EmailNotVerifiedException : Exception
    {
        public EmailNotVerifiedException(): base("Email not yet verified. Go to your email to verify")
        {

        }
    }
}
