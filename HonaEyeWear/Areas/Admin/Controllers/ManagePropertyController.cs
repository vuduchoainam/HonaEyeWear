using HonaEyeWear.Models;
using HonaEyeWear.Services;
using Microsoft.AspNetCore.Mvc;

namespace HonaEyeWear.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/ManageProperty")]
    public class ManagePropertyController : Controller
    {
        private readonly IPropertyService _propertyService;
        public ManagePropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        [HttpGet("")]
        public async Task<ActionResult> Index()
        {
            var properties = await _propertyService.GetAllPropertiesAsync();
            return View(properties.ToList());
        }

        [HttpGet("Create")]
        public async Task<ActionResult> Create()
        {
            return View();
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(Property property)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra tính hợp lệ của dữ liệu đã được binding vào đối tượng Property
                var name = property.Name;

                // Gọi phương thức AddPropertyAsync sau khi dữ liệu đã được binding
                _propertyService.AddPropertyAsync(property);
                await _propertyService.SaveChangesAsync();

                return RedirectToAction("Index", "ManageProperty", new { area = "Admin" });
            }
            return View(property);
        }


        [HttpGet("Edit/{id}")]
        public async Task<ActionResult> Edit(int id)
        {
            var property = await _propertyService.GetByIdPropertyAsync(id);
            if (property == null)
            {
                return NotFound();
            }
            return View(property);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, Property updatedProperty)
        {
            if (id != updatedProperty.Id)
            {
                return NotFound();
            }
            try
            {
                var oldProperty = await _propertyService.GetByIdPropertyAsync(id);
                oldProperty.Name = updatedProperty.Name;
                await _propertyService.SaveChangesAsync();

                return RedirectToAction("Index", "ManageProperty", new { area = "Admin" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi cố gắng cập nhật thuộc tính. Vui lòng thử lại sau.");
                Console.WriteLine(ex.Message);
                return View(updatedProperty);
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
                await _propertyService.DeletePropertyAsync(id.Value);
                await _propertyService.SaveChangesAsync();
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
