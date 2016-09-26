using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HotelWeb.DAL.Entities
{
    [Table("UserRoles")]
    public class UserRole
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int RoleId { get; set; }


        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }
}