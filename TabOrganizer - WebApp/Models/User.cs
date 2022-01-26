using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TabOrganizer___WebApp.Models
{
    public class User
    {
        public int Id { get; set; }
        //[Required]
        public string Username { get; set; }
        //[Required]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Role { get; set; }
        public string Token { get; set; }


    }
}
