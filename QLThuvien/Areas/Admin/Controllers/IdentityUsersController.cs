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
            public IList<string> Roles { get; set; } = new List<string>();
        }

        public class CreateUserVM
        {
            [Required] public string Name { get; set; } = "";
            [Required, EmailAddress] public string Email { get; set; } = "";
            [Required, StringLength(100, MinimumLength = 6), DataType(DataType.Password)]
            public string Password { get; set; } = "";
            [DataType(DataType.Password), Compare("Password")]
            public string ConfirmPassword { get; set; } = "";
            public List<RoleSelection> Roles { get; set; } = new();
        }

        public class EditUserVM
        {
            [Required] public string Id { get; set; } = "";
            [Required] public string Name { get; set; } = "";
            [Required, EmailAddress] public string Email { get; set; } = "";
            [Phone] public string? PhoneNumber { get; set; }
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
            var vm = new CreateUserVM
            {
                Roles = allRoles.Select(r => new RoleSelection { Name = r, Selected = false }).ToList()
            };
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserVM model)
        {
            if (!ModelState.IsValid)
                return View(await FillRoles(model));

            var user = new IdentityUser { UserName = model.Email, Email = model.Email, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                AddErrors(result);
                return View(await FillRoles(model));
            }

            //await SetFullNameAsync(user, model.Name);

            var selectedRoles = model.Roles.Where(x => x.Selected).Select(x => x.Name);
            foreach (var role in selectedRoles)
                if (await _roleManager.RoleExistsAsync(role))
                    await _userManager.AddToRoleAsync(user, role);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();

            var allRoles = _roleManager.Roles.Select(r => r.Name!).ToList();
            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = await _userManager.GetClaimsAsync(user);
            var name = GetFullNameClaim(claims) ?? "";

            var vm = new EditUserVM
            {
                Id = user.Id,
                Email = user.Email ?? "",
                Name = name,
                PhoneNumber = user.PhoneNumber,
                Roles = allRoles.Select(r => new RoleSelection
                {
                    Name = r,
                    Selected = userRoles.Contains(r)
                }).ToList()
            };
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserVM model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user is null) return NotFound();

            if (!ModelState.IsValid)
                return View(await FillRoles(model));

            user.Email = model.Email;
            user.UserName = model.Email;
            var update = await _userManager.UpdateAsync(user);
            await SetFullNameAsync(user, model.Name);

            if (!update.Succeeded)
            {
                AddErrors(update);
                return View(await FillRoles(model));
            }

            await SetFullNameAsync(user, model.Name);

            var phoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber ?? "");
            if (!phoneResult.Succeeded)
            {
                AddErrors(phoneResult);
                return View(await FillRoles(model));
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var selectedRoles = (model.Roles ?? new()).Where(x => x.Selected).Select(x => x.Name).ToArray();

            // Thêm vai trò mới
            var toAdd = selectedRoles.Except(currentRoles);
            if (toAdd.Any()) await _userManager.AddToRolesAsync(user, toAdd);

            // Gỡ vai trò bỏ chọn
            var toRemove = currentRoles.Except(selectedRoles);
            if (toRemove.Any()) await _userManager.RemoveFromRolesAsync(user, toRemove);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ResetPassword(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();
            return View(new ResetPasswordVM { Id = id });
        }

        // POST: /Admin/IdentityUsers/ResetPassword
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
