using HonaEyeWear.Models;
using HonaEyeWear.Repositories;

namespace HonaEyeWear.Services
{
    public class PropertyValueService : IPropertyValueService
    {
        private readonly IRepository<PropertyValue> _propertyValueRepository;
        private readonly IRepository<Property> _propertyRepository;


        public PropertyValueService(IRepository<PropertyValue> propertyValueRepository, IRepository<Property> propertyRepository)
        {
            _propertyValueRepository = propertyValueRepository;
            _propertyRepository = propertyRepository;
        }

        public async Task AddPropertyValueAsync(PropertyValue propertyValue)
        {
            await _propertyValueRepository.AddAsync(propertyValue);
        }

        public async Task DeletePropertyValueAsync(int id)
        {
            var propertyValue = await _propertyValueRepository.GetByIdAsync(id);
            if (propertyValue != null)
            {
                await _propertyValueRepository.RemoveAsync(propertyValue);
            }
        }

        public async Task<IEnumerable<PropertyValue>> GetAllPropertyValueAsync()
        {
            return await _propertyValueRepository.GetAllAsync();
        }

        public async Task<PropertyValue> GetByIdPropertyValueAsync(int id)
        {
            return await _propertyValueRepository.GetByIdAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await _propertyValueRepository.SaveChangesAsync();
        }

        public async Task UpdatePropertyValueAsync(PropertyValue propertyValue)
        {
            _propertyValueRepository.UpdateAsync(propertyValue);
        }
    }
}
