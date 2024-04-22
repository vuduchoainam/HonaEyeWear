using HonaEyeWear.Models;

namespace HonaEyeWear.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task AddCategoryAsync(Category category);
        Task SaveChangesAsync();
        Task<Category> GetByIdCategoryAsync(int id);
        Task<int> CountCategoriesWithSlugAsync(string slug);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
    }
}
