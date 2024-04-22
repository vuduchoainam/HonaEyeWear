using HonaEyeWear.Models;
using HonaEyeWear.Repositories;

namespace HonaEyeWear.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _categoryRepository.AddAsync(category);
        }

        public async Task SaveChangesAsync()
        {
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task<Category> GetByIdCategoryAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<int> CountCategoriesWithSlugAsync(string slug)
        {
            var categories = await _categoryRepository.FindAsync(x => x.Slug == slug);
            return categories.Count();
        }
        public async Task UpdateCategoryAsync(Category category)
        {
            _categoryRepository.UpdateAsync(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category != null)
            {
                await _categoryRepository.RemoveAsync(category);
            }
        }
    }
}
