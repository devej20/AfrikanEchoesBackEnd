using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AfrikanEchoes.Models.Users
{
    public class AuthenticateModel
    {
        [Required(ErrorMessage = "Please Provide Valid Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please Provide Valid Password")]
        public string Password { get; set; }
    }
    public class LoginPageModel
    {
        [Required(ErrorMessage = "Please Provide Valid Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please Provide Valid Password")]
        public string Password { get; set; }
    }
}
