using System;
using System.Collections.Generic;
using StoreDL;
using StoreModels;

namespace StoreBL
{
    public class StoreBL
    {
        private StoreRepoDB repo;
        public StoreBL(StoreRepoDB repo)
        {
            this.repo = repo;
        }
        public User GetUserByName(string userName)
        {
            List<User> users = repo.GetUsers(user => user.UserName.Equals(userName));
            if (users.Count == 0) return null;
            if (users.Count > 1) throw new Exception($"Multiple users with the same name: {userName}");
            return users[0];
        }
        public string CreateUser(User user)
        {
           if (user.UserName.Equals(""))
            {
                return "Validation error: cannot add a user with an empty name";
            }
           if (repo.GetUsers(u => u.UserName.Equals(user.UserName)).Count > 0)
            {
                return "Cannot add user: Name already taken";
            }
            if (!repo.AddUser(user))
            {
                return "Error: Database write failed";
            }
            return null;
        }
        public List<User> GetUsers()
        {
            return repo.GetUsers(user => true);
        }
        public string CreateProduct(Product product)
        {
            if (!repo.AddProduct(product))
            {
                return "Error: Database write failed";
            }
            return null;
        }
        public List<Product> GetProducts()
        {
            return repo.GetProducts(product => true);
        }
        public List<Inventory> GetLocationsOfProduct(int productId)
        {
            return repo.GetLocationsOfProduct(productId, loc => true);
        }
    }
}
