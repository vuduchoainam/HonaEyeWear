using HonaEyeWear.Models;
using HonaEyeWear.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace HonaEyeWear.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/ManageCategory")]
    public class ManageCategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public ManageCategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        private string RemoveVietnameseDiacritics(string str)
        {
            str = str.Replace("à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ|À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ", "a");
            str = str.Replace("è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ|È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ", "e");
            str = str.Replace("ì|í|ị|ỉ|ĩ|Ì|Í|Ị|Ỉ|Ĩ", "i");
            str = str.Replace("ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ|Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ", "o");
            str = str.Replace("ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ|Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ", "u");
            str = str.Replace("ỳ|ý|ỵ|ỷ|ỹ|Ỳ|Ý|Ỵ|Ỷ|Ỹ", "y");
            str = str.Replace(" ", "-");
            str = str.Replace("đ", "d").Replace("Đ", "D");
            str = Regex.Replace(str.Normalize(NormalizationForm.FormD), @"[\u0300-\u036f]", string.Empty);
            return str;
        }
        private string GenerateSlug(string name)
        {
            // Chuyển đổi tất cả các ký tự thành chữ thường
            name = name.ToLower();
            // Xóa các dấu tiếng Việt và chuẩn hóa
            name = RemoveVietnameseDiacritics(name);
            // Loại bỏ khoảng trắng ở đầu và cuối chuỗi
            name = name.Trim('-');
            // Nếu tên sau các bước trên là rỗng, trả về một giá trị mặc định
            if (string.IsNullOrEmpty(name))
            {
                return "default-slug";
            }
            return name;
        }

        [HttpGet("")]
        public async Task<ActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return View(categories.ToList());
        }

        [HttpGet("Create")]
        public async Task<ActionResult> Create()
        {
            return View();
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = GenerateSlug(category.Name);
                category.CreatedAt = DateTime.Now;
                category.UpdatedAt = DateTime.Now;

                // Kiểm tra xem slug đã tồn tại chưa
                var originalSlug = category.Slug;
                var slugIndex = 1;
                while (await _categoryService.CountCategoriesWithSlugAsync(category.Slug) > 0)
                {
                    // Nếu slug đã tồn tại, thử với một phiên bản mới
                    category.Slug = $"{originalSlug}-{slugIndex}";
                    slugIndex++;
                }
                // Thêm category mới vào cơ sở dữ liệu
                _categoryService.AddCategoryAsync(category);
                await _categoryService.SaveChangesAsync();

                return RedirectToAction("Index", "ManageCategory", new { area = "Admin" });
            }
            return View(category);
        }

        [HttpGet("Edit/{id}")]
        public async Task<ActionResult> Edit(int id)
        {
            var category = await _categoryService.GetByIdCategoryAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, Category updatedCategory)
        {
            if (id != updatedCategory.Id)
            {
                return NotFound();
            }
            try
            {
                // Lấy thông tin category cũ từ cơ sở dữ liệu
                var oldCategory = await _categoryService.GetByIdCategoryAsync(id);

                // Cập nhật các trường Name, Slug, ParentId, và UpdatedAt
                oldCategory.Name = updatedCategory.Name;
                oldCategory.Slug = GenerateSlug(updatedCategory.Name);
                oldCategory.ParentId = updatedCategory.ParentId;
                oldCategory.UpdatedAt = DateTime.Now;
                // Lưu các thay đổi vào cơ sở dữ liệu
                await _categoryService.SaveChangesAsync();

                return RedirectToAction("Index", "ManageCategory", new { area = "Admin" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi cố gắng cập nhật category. Vui lòng thử lại sau.");
                Console.WriteLine(ex.Message);
                return View(updatedCategory);
            }
        }

        [HttpPost("Delete/{id}")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "ID không hợp lệ" });
            }
            try
            {
                await _categoryService.DeleteCategoryAsync(id.Value);
                await _categoryService.SaveChangesAsync();
                return Json(new { success = true, message = "Xóa danh mục thành công" });
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine(ex.Message);
                return Json(new { success = false, message = "Lỗi khi xóa danh mục" });
            }
        }
    }
}
