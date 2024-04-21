using HonaEyeWear.Models;

namespace HonaEyeWear.Services
{
    public interface IPropertyValueService
    {
        Task<IEnumerable<PropertyValue>> GetAllPropertyValueAsync();
        Task AddPropertyValueAsync(PropertyValue propertyValue);
        Task SaveChangesAsync();
        Task<PropertyValue> GetByIdPropertyValueAsync(int id);
        Task UpdatePropertyValueAsync(PropertyValue propertyValue);
        Task DeletePropertyValueAsync(int id);
    }
}
