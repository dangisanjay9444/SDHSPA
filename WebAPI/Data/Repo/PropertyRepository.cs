using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Data.Repo
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly DataContext dc;
        public PropertyRepository(DataContext dc)
        {
            this.dc = dc;
            
        }
        public void AddProperty(Property property)
        {
            dc.Properties.Add(property);
        }

        public void DeleteProperty(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Property>> GetPropertiesAsync(int sellRent)
        {
            var properties = await dc.Properties
            .Include(p => p.PropertyType)
            .Include(p => p.City) // Include the City navigation property
               .ThenInclude(c => c.Country) // Then include the Country navigation property from City
            .Include(p => p.FurnishingType)
            .Include(p => p.Photos)
            .Where(p => p.SellRent == sellRent)
            .ToListAsync();
            return properties;
        }

        public async Task<Property> GetPropertyByIdAsync(int id)
        {
            var properties = await dc.Properties
            .Include(p => p.Photos)            
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();
            return properties;
        }

        public async Task<Property> GetPropertyDetailAsync(int id)
        {
            var properties = await dc.Properties
            .Include(p => p.PropertyType)
            .Include(p => p.City) // Include the City navigation property
               .ThenInclude(c => c.Country) // Then include the Country navigation property from City
            .Include(p => p.FurnishingType)
            .Include(p => p.Photos)
            .Where(p => p.Id == id)
            .FirstAsync();
            return properties;
        }
    }
}