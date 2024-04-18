using HonaEyeWear.Controllers;
using HonaEyeWear.Data;
using HonaEyeWear.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HonaEyeWear.Areas.Admin.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("Admin/ManageRole")]
    public class ManageRoleController : BaseController<IdentityRole>
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;


        public ManageRoleController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager) : base(context)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet("")] // Chỉ rõ là phương thức GET
        public async Task<ActionResult> Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        [HttpGet("Create")]
        public async Task<ActionResult> Create()
        {
            return View();
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create(IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(role.Name))
                    {
                        ModelState.AddModelError("Name", "Chưa nhập tên role");
                    }
                    else
                    {
                        // Save the role
                        var result = await _roleManager.CreateAsync(new IdentityRole { Name = role.Name });
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(role);
        }

        [HttpPost("Delete/{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "ID không hợp lệ" });
            }

            try
            {
                // Kiểm tra xem role có tồn tại không
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return Json(new { success = false, message = "Role không tồn tại" });
                }

                // Xóa role
                var result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return Json(new { success = true, message = "Xóa role thành công" });
                }
                else
                {
                    // Nếu có lỗi khi xóa role, trả về message lỗi
                    return Json(new { success = false, message = string.Join(", ", result.Errors.Select(e => e.Description)) });
                }
            }
            catch (Exception ex)
            {
                // Log lỗi cho mục đích debug
                Console.WriteLine(ex.Message);
                return Json(new { success = false, message = "Lỗi khi xóa role" });
            }
        }

        [Route("RoleList")]
        [HttpGet]
        public async Task<ActionResult> RoleList()
        {
            var roles = _roleManager.Roles.ToList();
            var users = _userManager.Users.ToList();
            var roleUserList = new List<IdentityUserRole<string>>();

            foreach (var role in roles)
            {
                var userIds = _userManager.GetUsersInRoleAsync(role.Name)
                    .Result
                    .Select(u => u.Id)
                    .ToList();

                roleUserList.AddRange(userIds.Select(userId => new IdentityUserRole<string> { RoleId = role.Id, UserId = userId }));
            }

            ViewBag.Roles = roles;
            ViewBag.Users = users;
            return View(roleUserList);
        }

        [Route("RoleList/SetRole")]
        [HttpGet]
        public async Task<ActionResult> SetRole()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var users = await _userManager.Users.ToListAsync();

            ViewBag.Roles = roles;
            ViewBag.Users = users;

            return View();
        }

        [Route("RoleList/SetRole")]
        [HttpPost]
        public async Task<ActionResult> SetRole(IFormCollection formCollection)
        {
            List<string> errors = new List<string>();

            try
            {
                var roleId = formCollection["RoleId"].ToString();
                var userId = formCollection["UserId"].ToString();

                var getRole = await _roleManager.FindByIdAsync(roleId);
                var getUser = await _userManager.FindByIdAsync(userId);

                if (getRole == null)
                {
                    errors.Add("Không tìm thấy Role này.");
                }

                if (getUser == null)
                {
                    errors.Add("Không tìm thấy User này.");
                }

                if (errors.Count == 0)
                {
                    if (!await _userManager.IsInRoleAsync(getUser, getRole.Name))
                    {
                        // Add user to the role if not already in the role
                        var result = await _userManager.AddToRoleAsync(getUser, getRole.Name);

                        if (result.Succeeded)
                        {
                            return RedirectToAction("RoleList");
                        }
                        else
                        {
                            // Handle the case where adding the user to the role fails
                            errors.AddRange(result.Errors.Select(error => error.Description));
                        }
                    }
                    else
                    {
                        errors.Add("User đã có role này.");
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }

            TempData["Errors"] = errors;
            return RedirectToAction("RoleList");
        }

        [HttpPost("DeleteRole")]
        public async Task<ActionResult> DeleteRole(Guid? UserId, Guid? RoleId)
        {
            try
            {
                if (!RoleId.HasValue || !UserId.HasValue)
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin." });
                }

                var getRole = await _roleManager.FindByIdAsync(RoleId.ToString());
                var getUser = await _userManager.FindByIdAsync(UserId.ToString());

                if (getRole == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy Role này." });
                }

                if (getUser == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy User này." });
                }

                // Check if the user is in the role
                if (await _userManager.IsInRoleAsync(getUser, getRole.Name))
                {
                    // Remove the user from the role
                    var result = await _userManager.RemoveFromRoleAsync(getUser, getRole.Name);

                    if (result.Succeeded)
                    {
                        return Json(new { success = true, message = "Xóa set role thành công" });
                    }
                    else
                    {
                        return Json(new { success = false, message = string.Join(", ", result.Errors.Select(e => e.Description)) });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "User không thuộc role này." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
