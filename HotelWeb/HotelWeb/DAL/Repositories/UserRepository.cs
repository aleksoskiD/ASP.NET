using HotelWeb.DAL.Context;
using HotelWeb.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace HotelWeb.DAL.Repositories
{
    public interface IUserRepository
    {
        User ValidateUser(string userName, string password);
        User Create(string userName, string password, string address, string city, string contactNo);


        bool DeleteUser(Guid id);
    }

    public class UserRepository : IUserRepository
    {
        private HotelContext context;
        public UserRepository(HotelContext context)
        {
            this.context = context;         
        }

        

        /// <summary>
        /// Check if user exist in the system
        /// </summary>
        /// <param name="userName">Email address</param>
        /// <param name="password">Password</param>
        /// <returns>User object or null</returns>
        public User ValidateUser(string userName, string password)
        {
            if(string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            string hashedPassword = CalculateMD5Hash(password.ToLower());

            return context.Users.Where(x => x.UserName.ToLower() == userName.ToLower() && x.Password.ToLower() == hashedPassword.ToLower()).FirstOrDefault();
            
        }


        /// <summary>
        /// Create a new user in the system
        /// </summary>
        /// <param name="userName">Email address</param>
        /// <param name="password">Password</param>
        /// <param name="address">Address</param>
        /// <param name="city">City</param>
        /// <param name="contactNo">Contact Number</param>
        /// <returns>User object or null</returns>
        public User Create(string userName, string password, string address, string city, string contactNo)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(contactNo))
            {
                return null;
            }

            if (ValidateUser(userName, password) != null)
            {
                return null; // user already exist
            }

            var user = new User();
            user.Id = Guid.NewGuid();
            user.UserName = userName;
            user.Password = CalculateMD5Hash(password);
            user.Address = address;
            user.City = city;
            user.ContactNo = contactNo;           
            user.CreatedOn = DateTime.Now;

            // get customer role
            var customerRole = context.Roles.Where(x => x.Name.ToLower() == "customer").FirstOrDefault();
            if (customerRole == null) return null;

            // assign the role
            var userRole = new UserRole();
            userRole.Id = Guid.NewGuid();
            userRole.UserId = user.Id;
            userRole.RoleId = customerRole.Id;
           
            user.UserRoles.Add(userRole);            
            // add & save user data
            context.Users.Add(user);

            if (context.SaveChanges() > 0)
            {
                // Success
                return user;
            }
            else
            {
                // Failed
                return null;
            }
        }

        public bool DeleteUser(Guid id)
        {
            var user = context.Users.Where(x => x.Id == id).FirstOrDefault();
            if (user == null) return false;

            context.Users.Remove(user);

            return context.SaveChanges() > 0;
        }


        private string CalculateMD5Hash(string input)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            inputBytes = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in inputBytes)
            {
                sb.Append(b.ToString("x2").ToLower());
            }

            return sb.ToString();
        }
    }
}