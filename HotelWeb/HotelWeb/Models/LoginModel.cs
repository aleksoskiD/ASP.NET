using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HotelWeb.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage="Внесете корисничко име!")]
        [Display(Name = "Корисничко име")]
        public string UserName { get; set; }

        [Required(ErrorMessage="Внесете лозинка!")]
        [DataType(DataType.Password)]
        [Display(Name = "Лозинка")]
        public string Password { get; set; }

        [Display(Name = "Запамти ме?")]
        public bool RememberMe { get; set; }
    }
}