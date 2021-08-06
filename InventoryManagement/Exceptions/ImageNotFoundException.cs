using System;

namespace InventoryManagement.Exceptions
{
    public class ImageNotFoundException: Exception
    {
        public ImageNotFoundException(): base("Image not found")
        {

        }
        public ImageNotFoundException(string type) : base(string.Format("Image not found on {0}", type))
        {

        }
    }
}
