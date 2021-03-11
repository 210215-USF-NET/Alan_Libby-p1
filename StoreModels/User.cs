using System;
using System.Collections.Generic;

namespace StoreModels
{
    /// <summary>
    /// User model stores details about a customer or manager
    /// </summary>
    public class User
    {
        private string userName;
        public int UserId { get; set; }
        public string UserName
        {
            get { return userName; }
            set
            {
                if (value == null || value.Equals(""))
                {
                    throw new ArgumentNullException("Cannot set UserName to null or empty.");
                }
                userName = value;
            }
        }
        public bool isManager { get; set; }
        public List<Order> Orders { get; set; }
    }
}
