using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace QLThuvien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] // chỉ Admin
    public class IdentityUsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityUsersController(UserManager<IdentityUser> userManager,
                                       RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // ======== VIEW MODELS ========
        public class RoleSelection
        {
            public string Name { get; set; } = "";
            public bool Selected { get; set; }
        }

        public class IdentityUserListItemVM
        {
            public string Id { get; set; } = "";
            public string? Name { get; set; }
            public string? Email { get; set; }
            public string? PhoneNumber { get; set; }
            public IList<string> Roles { get; set; } = new List<string>();
        }


        public class CreateUserVM
        {
            [Required, EmailAddress] public string Email { get; set; } = "";
            [Required, StringLength(100, MinimumLength = 6), DataType(DataType.Password)]
            public string Password { get; set; } = "";
            [DataType(DataType.Password), Compare("Password")]
            public string ConfirmPassword { get; set; } = "";

            [Required] public string Name { get; set; } = "";
            [Phone] public string? PhoneNumber { get; set; }

            [Required(ErrorMessage = "Chọn 1 vai trò")]
            public string? SelectedRole { get; set; }
            public List<RoleSelection> Roles { get; set; } = new();
        }


        public class EditUserVM
        {
            [Required] public string Id { get; set; } = "";
            [Required, EmailAddress] public string Email { get; set; } = "";

            [Required] public string Name { get; set; } = "";
            [Phone] public string? PhoneNumber { get; set; }

            [Required(ErrorMessage = "Chọn 1 vai trò")]
            public string? SelectedRole { get; set; }
            public List<RoleSelection> Roles { get; set; } = new();
        }

        public class ResetPasswordVM
        {
            [Required] public string Id { get; set; } = "";
            [Required, StringLength(100, MinimumLength = 6), DataType(DataType.Password)]
            public string NewPassword { get; set; } = "";
            [DataType(DataType.Password), Compare("NewPassword")]
            public string ConfirmPassword { get; set; } = "";
        }

        // ======== ACTIONS ========

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var vms = new List<IdentityUserListItemVM>();

            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                var claims = await _userManager.GetClaimsAsync(u);
                var name = GetFullNameClaim(claims);
                vms.Add(new IdentityUserListItemVM
                {
                    Id = u.Id,
                    Name = string.IsNullOrWhiteSpace(name) ? "(Chưa có tên)" : name,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Roles = roles
                });
            }
            return View(vms);
        }

        private static string? GetFullNameClaim(IEnumerable<Claim> claims)
            => claims.FirstOrDefault(c => c.Type == "full_name")?.Value;

        private async Task SetFullNameAsync(IdentityUser user, string name)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var existing = claims.FirstOrDefault(c => c.Type == "full_name");
            if (existing != null) await _userManager.RemoveClaimAsync(user, existing);
            await _userManager.AddClaimAsync(user, new Claim("full_name", name ?? ""));
        }


        public IActionResult Create()
        {
            var allRoles = _roleManager.Roles.Select(r => r.Name!).ToList();
            return View(new CreateUserVM
            {
                Roles = allRoles.Select(r => new RoleSelection { Name = r }).ToList(),
                SelectedRole = allRoles.FirstOrDefault()
            });
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Roles = _roleManager.Roles.Select(r => new RoleSelection { Name = r.Name! }).ToList();
                return View(model);
            }

            var user = new IdentityUser { UserName = model.Email, Email = model.Email, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                AddErrors(result);
                model.Roles = _roleManager.Roles.Select(r => new RoleSelection { Name = r.Name! }).ToList();
                return View(model);
            }
            if (!string.IsNullOrWhiteSpace(model.PhoneNumber))
                await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);

            await _userManager.AddClaimAsync(user, new Claim("full_name", model.Name));

            if (!string.IsNullOrEmpty(model.SelectedRole) &&
                await _roleManager.RoleExistsAsync(model.SelectedRole))
            {
                await _userManager.AddToRoleAsync(user, model.SelectedRole);
            }

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();

            var allRoles = _roleManager.Roles.Select(r => r.Name!).ToList();
            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = await _userManager.GetClaimsAsync(user);
            var nameClaim = claims.FirstOrDefault(c => c.Type == "full_name")?.Value ?? "";

            var vm = new EditUserVM
            {
                Id = user.Id,
                Email = user.Email ?? "",
                Name = nameClaim,
                PhoneNumber = user.PhoneNumber,
                Roles = allRoles.Select(r => new RoleSelection { Name = r }).ToList(),
                SelectedRole = userRoles.FirstOrDefault()
            };
            return View(vm);
        }



        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserVM model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user is null) return NotFound();

            if (!ModelState.IsValid)
            {
                model.Roles = _roleManager.Roles.Select(r => new RoleSelection { Name = r.Name! }).ToList();
                return View(model);
            }

            user.Email = model.Email;
            user.UserName = model.Email;
            var update = await _userManager.UpdateAsync(user);
            if (!update.Succeeded) { AddErrors(update); model.Roles = _roleManager.Roles.Select(r => new RoleSelection { Name = r.Name! }).ToList(); return View(model); }

            // Cập nhật tên (claim)
            await SetFullNameAsync(user, model.Name);

            // Cập nhật số điện thoại
            var phoneRes = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber ?? "");
            if (!phoneRes.Succeeded) { AddErrors(phoneRes); model.Roles = _roleManager.Roles.Select(r => new RoleSelection { Name = r.Name! }).ToList(); return View(model); }

            var current = await _userManager.GetRolesAsync(user);
            if (current.Any()) await _userManager.RemoveFromRolesAsync(user, current);
            if (!string.IsNullOrEmpty(model.SelectedRole) && await _roleManager.RoleExistsAsync(model.SelectedRole))
                await _userManager.AddToRoleAsync(user, model.SelectedRole);

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> ResetPassword(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();
            return View(new ResetPasswordVM { Id = id });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user is null) return NotFound();

            if (!ModelState.IsValid) return View(model);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
            if (!result.Succeeded)
            {
                AddErrors(result);
                return View(model);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();
            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.Roles = roles;
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is not null)
                await _userManager.DeleteAsync(user);

            return RedirectToAction(nameof(Index));
        }

        // ======== helpers ========
        private void AddErrors(IdentityResult result)
        {
            foreach (var e in result.Errors) ModelState.AddModelError(string.Empty, e.Description);
        }

        private async Task<CreateUserVM> FillRoles(CreateUserVM model)
        {
            var allRoles = await Task.FromResult(_roleManager.Roles.Select(r => r.Name!).ToList());
            model.Roles = allRoles.Select(r => new RoleSelection
            {
                Name = r,
                Selected = model.Roles?.Any(x => x.Name == r && x.Selected) == true
            }).ToList();
            return model;
        }

        private async Task<EditUserVM> FillRoles(EditUserVM model)
        {
            var allRoles = await Task.FromResult(_roleManager.Roles.Select(r => r.Name!).ToList());
            model.Roles = allRoles.Select(r => new RoleSelection
            {
                Name = r,
                Selected = model.Roles?.Any(x => x.Name == r && x.Selected) == true
            }).ToList();
            return model;
        }
    }
}
