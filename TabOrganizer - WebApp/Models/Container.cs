using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TabOrganizer___WebApp.Models
{
    public class Container
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreation { get; set; }
        public int UserId { get; set; }

        public IList<Website> Websites { get; set; }
    }
}
