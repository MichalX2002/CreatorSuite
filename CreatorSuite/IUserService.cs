using System.Collections.Generic;

namespace CreatorSuite
{
    public interface IUserService
    {
        User GetById(int id);

        User Authenticate(string username, string password);
        User Create(User user, string password);
        void Update(User user, string password = null);
        bool Delete(int id);
    }
}
