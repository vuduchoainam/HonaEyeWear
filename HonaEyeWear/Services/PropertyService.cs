using HonaEyeWear.Models;
using HonaEyeWear.Repositories;

namespace HonaEyeWear.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IRepository<Property> _propertyRepository;

        public PropertyService(IRepository<Property> propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task AddPropertyAsync(Property property)
        {
            await _propertyRepository.AddAsync(property);
        }

        public async Task DeletePropertyAsync(int id)
        {
            var property = await _propertyRepository.GetByIdAsync(id);
            if (property != null)
            {
                await _propertyRepository.RemoveAsync(property);
            }
        }

        public async Task<IEnumerable<Property>> GetAllPropertiesAsync()
        {
            return await _propertyRepository.GetAllAsync();
        }

        public async Task<Property> GetByIdPropertyAsync(int id)
        {
            return await _propertyRepository.GetByIdAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await _propertyRepository.SaveChangesAsync();
        }

        public async Task UpdatePropertyAsync(Property property)
        {
            _propertyRepository.UpdateAsync(property);
        }
    }
}
