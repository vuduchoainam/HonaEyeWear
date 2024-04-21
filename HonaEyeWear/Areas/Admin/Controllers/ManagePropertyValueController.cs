using HonaEyeWear.Models;
using HonaEyeWear.Services;
using Microsoft.AspNetCore.Mvc;

namespace HonaEyeWear.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/ManagePropertyValue")]
    public class ManagePropertyValueController : Controller
    {
        private readonly IPropertyValueService _propertyValueService;
        private readonly IPropertyService _propertyService;
        public ManagePropertyValueController(IPropertyValueService propertyValueService, IPropertyService propertyService)
        {
            _propertyValueService = propertyValueService;
            _propertyService = propertyService;
        }


        [Route("")]
        public async Task<ActionResult> Index()
        {
            var properties = await _propertyService.GetAllPropertiesAsync();
            ViewBag.Properties = properties;
            var propertyValues = await _propertyValueService.GetAllPropertyValueAsync();
            return View(propertyValues.ToList());
        }

        [HttpGet("Create")]
        public async Task<ActionResult> Create()
        {
            var properties = await _propertyService.GetAllPropertiesAsync();
            ViewBag.Properties = properties;
            return View();
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(PropertyValue propertyValue, int propertyId)
        {
            if (ModelState.IsValid)
            {
                // Lấy thông tin của thuộc tính từ PropertyService
                var property = await _propertyService.GetByIdPropertyAsync(propertyId);
                if (property != null)
                {
                    // Gán thuộc tính cho PropertyValue
                    propertyValue.PropertyId = propertyId; // Gán propertyId thay vì PropertyId
                    var value = propertyValue.Value;
                    // Thêm PropertyValue vào cơ sở dữ liệu
                    _propertyValueService.AddPropertyValueAsync(propertyValue);
                    await _propertyValueService.SaveChangesAsync();

                    return RedirectToAction("Index", "ManagePropertyValue", new { area = "Admin" });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Thuộc tính không tồn tại.");
                }
            }
                // Nếu ModelState không hợp lệ hoặc không tìm thấy thuộc tính, hiển thị lại view Create với ModelState cũ
                // và truyền danh sách thuộc tính vào ViewBag để hiển thị trong dropdown
                var properties = await _propertyService.GetAllPropertiesAsync();
            ViewBag.Properties = properties;
            return View(propertyValue);
        }

        [HttpGet("Edit/{id}")]
        public async Task<ActionResult> Edit(int id)
        {
            var propertyValue = await _propertyValueService.GetByIdPropertyValueAsync(id);
            if (propertyValue == null)
            {
                return NotFound();
            }

            var properties = await _propertyService.GetAllPropertiesAsync();
            ViewBag.Properties = properties;

            return View(propertyValue);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, PropertyValue updatedPropertyValue)
        {
            // Kiểm tra xem id của đối tượng cần cập nhật có trùng khớp không
            if (id != updatedPropertyValue.Id)
            {
                return NotFound();
            }
            try
            {
                // Lấy đối tượng cần cập nhật từ cơ sở dữ liệu
                var oldPropertyValue = await _propertyValueService.GetByIdPropertyValueAsync(id);
                // Kiểm tra xem đối tượng cần cập nhật có tồn tại không
                if (oldPropertyValue == null)
                {
                    return NotFound();
                }
                // Cập nhật dữ liệu của đối tượng cần cập nhật
                oldPropertyValue.Value = updatedPropertyValue.Value;
                oldPropertyValue.PropertyId = updatedPropertyValue.PropertyId;
                await _propertyValueService.SaveChangesAsync();
                // Chuyển hướng về trang danh sách thuộc tính
                return RedirectToAction("Index", "ManagePropertyValue", new { area = "Admin" });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                ModelState.AddModelError("", "Có lỗi xảy ra khi cố gắng cập nhật thuộc tính. Vui lòng thử lại sau.");
                Console.WriteLine(ex.Message);
                return View(updatedPropertyValue);
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
                await _propertyValueService.DeletePropertyValueAsync(id.Value);
                await _propertyValueService.SaveChangesAsync();
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
