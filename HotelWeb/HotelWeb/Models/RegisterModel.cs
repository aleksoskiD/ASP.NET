using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HotelWeb.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Внесете корисничко име!")]
        [Display(Name = "Корисничко име")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Внесете лозинка!")]
        [StringLength(100, ErrorMessage = "Лозинката {0} треба да биде долга {2} карактери најмалку.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Лозинка")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Внесете адреса!")]
        [Display(Name = "Адреса")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Внесете град!")]
        [Display(Name = "Град")]
        public string City { get; set; }

        [Required(ErrorMessage = "Внесете телефон!")]
        [Display(Name = "Телефон")]
        public string ContactNo { get; set; }


    }
}