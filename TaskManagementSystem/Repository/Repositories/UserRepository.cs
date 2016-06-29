using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        Database db = new Database();

        public bool Register(User user)
        {
            if(user != null)
            {
                user.Role = "User";
                user.DateCreated = DateTime.Now;
                db.Users.Add(user);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Delete(int id)
        {
            var user = GetById(id);
            if(user != null)
            {
                if(user.IsActive == false){
                    user.IsActive = true;
                    db.Entry(user).CurrentValues.SetValues(user);
                    db.SaveChanges();
                    return true;
                }else{
                    user.IsActive = false;
                    db.Entry(user).CurrentValues.SetValues(user);
                    db.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public List<User> GetAll()
        {
            return db.Users.ToList();
        }

        public User GetById(int id)
        {
            return db.Users.FirstOrDefault(x => x.ID == id);
        }

        public bool Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
