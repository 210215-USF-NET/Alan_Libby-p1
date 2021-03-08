using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreModels
{
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
    }
}
