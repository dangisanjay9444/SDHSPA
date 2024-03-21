using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class City: BaseEntity
    {       
        public string Name { get; set; }
        [Required]
        public int CountryId { get; set; }
        public Country Country { get; set; }
        
    }
}