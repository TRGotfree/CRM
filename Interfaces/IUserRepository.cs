using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.Models;

namespace CRM.Interfaces
{
    public interface IUserRepository
    {
        User GetUser(string login, string password);

        User CreateUser(string login, string password, string name);

        void UpdateUser(User userDataToUpdate);

        void DeleteUser(User userToDelete);
    }
}
