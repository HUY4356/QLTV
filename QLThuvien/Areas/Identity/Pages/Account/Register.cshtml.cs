#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
// --- BỔ SUNG CÁC USING CẦN THIẾT ---
using QLThuvien.Models;
using Microsoft.EntityFrameworkCore;
// ------------------------------------

namespace QLThuvien.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _sender;
        // --- BỔ SUNG THUVUBCONTEXT ---
        private readonly ThuVienDbContext _thuVienDbContext;
        // -----------------------------

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender sender,
            // --- BỔ SUNG THAM SỐ VÀO CONSTRUCTOR ---
            ThuVienDbContext thuVienDbContext)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _sender = sender;
            // --- GÁN GIÁ TRỊ ---
            _thuVienDbContext = thuVienDbContext;
            // ------------------
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            // --- CÁC TRƯỜNG THÔNG TIN CÁ NHÂN MỚI ---
            [Required]
            [Display(Name = "Họ và tên")]
            public string Fullname { get; set; }

            [Phone]
            [Display(Name = "Số điện thoại")]
            public string PhoneNumber { get; set; }
            // ------------------------------------------
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                // --- BỔ SUNG SỐ ĐIỆN THOẠI VÀO IDENTITY USER ---
                user.PhoneNumber = Input.PhoneNumber;
                // ---------------------------------------------

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    // Gán vai trò mặc định là "DocGia" cho người dùng mới
                    await _userManager.AddToRoleAsync(user, "DocGia");

                    // ========== BƯỚC ĐỒNG BỘ QUAN TRỌNG NHẤT ==========
                    try
                    {
                        // Tìm vai trò "DocGia" trong bảng Role của ứng dụng
                        var appDocGiaRole = await _thuVienDbContext.Roles.FirstOrDefaultAsync(r => r.Name == "DocGia");
                        if (appDocGiaRole != null)
                        {
                            // Tạo một đối tượng User mới để lưu vào database của bạn
                            var appUser = new User
                            {
                                Fullname = Input.Fullname, // Lấy từ form đăng ký
                                Email = Input.Email,
                                Password = "hashed_by_identity", // Mật khẩu này không dùng để đăng nhập
                                Phone = Input.PhoneNumber,
                                RoleId = appDocGiaRole.Id, // Gán ID của vai trò Độc giả
                                CreatedAt = DateTime.UtcNow
                            };
                            _thuVienDbContext.Users.Add(appUser);
                            await _thuVienDbContext.SaveChangesAsync();
                            _logger.LogInformation($"Đã đồng bộ thành công user {Input.Email} vào bảng User của ứng dụng.");
                        }
                        else
                        {
                            _logger.LogError("Không tìm thấy vai trò 'DocGia' trong bảng Role của ứng dụng.");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Lỗi nghiêm trọng khi đồng bộ người dùng mới vào bảng User.");
                        // Cân nhắc xóa Identity user vừa tạo để tránh không nhất quán
                        await _userManager.DeleteAsync(user);
                        ModelState.AddModelError(string.Empty, "Đã có lỗi xảy ra trong quá trình tạo tài khoản, vui lòng thử lại.");
                        return Page();
                    }
                    // =======================================================

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _sender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
