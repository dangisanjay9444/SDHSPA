using System.Threading.Tasks;
using WebAPI.Data.Repo;
using WebAPI.Interfaces;

namespace WebAPI.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext dc;
        public UnitOfWork(DataContext _dc)
        {
            dc = _dc;
            
        }
        public ICityRepository cityRepository => 
            new CityRepository(dc);

        public IUserRepository userRepository => 
            new UserRepository(dc);

        public IPropertyRepository propertyRepository => 

            new PropertyRepository(dc);

        public IPropertyTypeRepository propertyTypeRepository => 
           
            new PropertyTypeRepository(dc);

        public IFurnishingTypeRepository furnishingTypeRepository => 
            new FurnishingTypeRepository(dc);

        public async Task<bool> SaveAsync()
        {
            return await dc.SaveChangesAsync() > 0;
        }

    }
}