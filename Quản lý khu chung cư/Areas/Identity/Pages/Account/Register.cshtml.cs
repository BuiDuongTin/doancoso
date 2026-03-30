// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Quản_lý_khu_chung_cư.Models;

namespace Quản_lý_khu_chung_cư.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly UserManager<NguoiDung> _userManager;
        private readonly IUserStore<NguoiDung> _userStore;
        private readonly IUserEmailStore<NguoiDung> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public RegisterModel(
            RoleManager<IdentityRole> roleManager,
            UserManager<NguoiDung> userManager,
            IUserStore<NguoiDung> userStore,
            SignInManager<NguoiDung> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IConfiguration configuration)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            public string HoTen { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} phải dài ít nhất là {2} và tối đa là {1} ký tự.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp.")]
            public string ConfirmPassword { get; set; }   

            [Required]
            [Phone]
            [Display(Name = "Phone Number")]
            public string SoDienThoai { get; set; }

            public string? VaiTro { get; set; }
            [ValidateNever]
            public IEnumerable<SelectListItem> RoleList { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!_roleManager.RoleExistsAsync(VaiTro.Role_Khach).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(VaiTro.Role_Khach)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(VaiTro.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(VaiTro.Role_QuanLy)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(VaiTro.Role_BaoVe)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(VaiTro.Role_CuDan)).GetAwaiter().GetResult();
            }
            Input = new()
            {
                RoleList = _roleManager.Roles.Select(u => u.Name).Select(x => new SelectListItem
                {
                    Text = x,
                    Value = x
                })
            };

            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.HoTen = Input.HoTen;
                user.SoDienThoai = Input.SoDienThoai;
                user.NgayTao = DateTime.Now;
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    if (!String.IsNullOrEmpty(Input.VaiTro))
                    {
                        await _userManager.AddToRoleAsync(user, Input.VaiTro);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, VaiTro.Role_Khach);
                    }

                    if (Input.VaiTro == VaiTro.Role_Khach)
                    {
                        // Tự động xác nhận email cho vai trò "Role_Khach"
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        await _userManager.ConfirmEmailAsync(user, code);

                        // Đăng nhập người dùng sau khi xác nhận email
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var verificationLink = Url.Page(
                            "/Account/VerifyEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, token = code },
                            protocol: Request.Scheme);

                        var managerEmail = "lekhang1617@gmail.com"; // Thay thế bằng email của Quản lý
                        await SendVerificationEmailAsync(managerEmail, verificationLink, user.HoTen, user.Email, Input.VaiTro);
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private async Task SendVerificationEmailAsync(string managerEmail, string verificationLink, string fullName, string email, string role)
        {
            var emailSettings = _configuration.GetSection("EmailSettings").Get<EmailSettings>();

            var smtpClient = new SmtpClient(emailSettings.SmtpServer)
            {
                Port = emailSettings.SmtpPort,
                Credentials = new NetworkCredential(emailSettings.SmtpUsername, emailSettings.SmtpPassword),
                EnableSsl = emailSettings.EnableSsl,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(emailSettings.SmtpUsername, "CityHaven"),
                Subject = "Xác thực đăng ký người dùng",
                Body = $@"
                    <p>Thông tin người đăng ký:</p>
                    <ul>
                        <li><strong>Tên:</strong> {fullName}</li>
                        <li><strong>Email:</strong> {email}</li>
                        <li><strong>Vai trò:</strong> {role}</li>
                    </ul>
                    <p>Vui lòng xác thực đăng ký người dùng mới bằng cách nhấp vào liên kết sau: <a href='{HtmlEncoder.Default.Encode(verificationLink)}'>Xác thực tài khoản</a></p>",
                IsBodyHtml = true,
            };

            mailMessage.To.Add(managerEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }



        private NguoiDung CreateUser()
        {
            try
            {
                return Activator.CreateInstance<NguoiDung>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(NguoiDung)}'. " +
                    $"Ensure that '{nameof(NguoiDung)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<NguoiDung> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<NguoiDung>)_userStore;
        }
    }

    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public bool EnableSsl { get; set; }
    }
}
