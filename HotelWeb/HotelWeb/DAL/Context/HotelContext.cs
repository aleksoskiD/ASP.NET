using HotelWeb.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HotelWeb.DAL.Context
{
    public class HotelContext : DbContext
    {
        public HotelContext() : base("name=HotelConnectionString")
        {
           // Database.CreateIfNotExists();
            //base.Configuration.LazyLoadingEnabled = false;      
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Room> Rooms { get; set; }
    }

  
}