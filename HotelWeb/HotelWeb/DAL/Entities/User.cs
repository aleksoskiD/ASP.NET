using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HotelWeb.DAL.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ContactNo { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<Reservation> UserReservations { get; set; }


        public User()
        {
            this.UserRoles = new List<UserRole>();
            this.UserReservations = new List<Reservation>();
        }
    }
}