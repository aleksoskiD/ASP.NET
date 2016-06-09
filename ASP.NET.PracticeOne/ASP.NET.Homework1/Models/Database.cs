using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET.Homework1.Models
{
    public static class IncrementId
    {
        public static int CountId = 0;
    }

    public class Database
    {

        List<User> users = new List<User>{
            new User{ID=0, Name="Daniel", Surname="Aleksoski", Email="adaniel709@gmail.com", Username="admin", Password="admin#123"}
        };

        public Database()
        {
            if (HttpContext.Current.Application["Equipment"] == null)
                HttpContext.Current.Application["Equipment"] = new List<Equipment>();
        }

        public List<Equipment> GetEquipment()
        {
                return HttpContext.Current.Application["Equipment"] as List<Equipment>;
         }

        public void AddEquipment(Equipment item)
        {
            //get current list od products we have in memory
            List<Equipment> dbEquipment = HttpContext.Current.Application["Equipment"] as List<Equipment>;

            //add the new product
            item.ID = IncrementId.CountId;

            dbEquipment.Add(item);

            IncrementId.CountId++;
            //return back in memory the new list with added
            HttpContext.Current.Application["Equipment"] = dbEquipment;
        }
        
        public void EditEquipment(Equipment item)
        {
            //get current list od products we have in memory
            List<Equipment> dbEquipment = HttpContext.Current.Application["Equipment"] as List<Equipment>;

            //index of item for editnig
            var itemEq = dbEquipment.FirstOrDefault(x => x.ID == item.ID);
            var index = dbEquipment.IndexOf(itemEq);

            //removing existing item
            dbEquipment.RemoveAt(index);

            //add the edited item
            dbEquipment.Insert(index, item);

            //return back in memory the new list with added
            HttpContext.Current.Application["Equipment"] = dbEquipment;
        }

        public void DeleteEquipment(Equipment item)
        {
            //get current list od products we have in memory
            List<Equipment> dbEquipment = HttpContext.Current.Application["Equipment"] as List<Equipment>;

            var itemEq = dbEquipment.FirstOrDefault(x => x.ID == item.ID);
            var index = dbEquipment.IndexOf(itemEq);

            dbEquipment.RemoveAt(index);

            //return back in memory the new list with deleted
            HttpContext.Current.Application["Equipment"] = dbEquipment;
        }

        public List<User> GetUsers()
        {
            if (users != null)
            {
                return users;
            }
            else
            {
                return HttpContext.Current.Application["User"] as List<User>;
            }
        }
    }

}