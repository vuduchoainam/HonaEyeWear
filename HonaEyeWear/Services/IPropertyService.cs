using HonaEyeWear.Models;

namespace HonaEyeWear.Services
{
    public interface IPropertyService
    {
        Task<IEnumerable<Property>> GetAllPropertiesAsync();
        Task AddPropertyAsync(Property property);
        Task UpdatePropertyAsync(Property property);
        Task DeletePropertyAsync(int id);
        Task<Property> GetByIdPropertyAsync(int id);
        Task SaveChangesAsync();
    }
}
