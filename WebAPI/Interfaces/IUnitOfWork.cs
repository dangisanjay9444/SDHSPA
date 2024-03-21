using System.Threading.Tasks;
using WebAPI.Data.Repo;

namespace WebAPI.Interfaces
{
    public interface IUnitOfWork
    {
         ICityRepository cityRepository {get;}
         IUserRepository userRepository{get;}
         IPropertyRepository propertyRepository{get;}
         IPropertyTypeRepository propertyTypeRepository{get;}         
         IFurnishingTypeRepository furnishingTypeRepository{get;}
         Task<bool>SaveAsync();

    }
}