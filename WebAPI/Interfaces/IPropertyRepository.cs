using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebAPI.Models;
using Property = WebAPI.Models.Property;

namespace WebAPI.Interfaces
{
    public interface IPropertyRepository
    {
        Task<IEnumerable<Property>> GetPropertiesAsync(int sellRent);

       Task<Property> GetPropertyDetailAsync(int id);

        void AddProperty(Property property);

        void DeleteProperty(int id);
        
    }
}