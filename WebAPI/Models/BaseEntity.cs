using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class BaseEntity
    {
        public int Id { get; set; } 
        public string UserModified{get;set;}
        public DateTime DateModified { get; set; } = DateTime.Now;
    }
}