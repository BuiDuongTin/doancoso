using Microsoft.AspNetCore.Identity;
//< !--Sử dụng namespace Microsoft.AspNetCore.Identity để làm việc với hệ thống quản lý người dùng, bao gồm UserManager và các phương thức xác thực -->

using Microsoft.AspNetCore.Mvc;
//<!-- Sử dụng namespace Microsoft.AspNetCore.Mvc để xây dựng các controller và action -->

using Microsoft.AspNetCore.Mvc.RazorPages;
//<!-- Sử dụng namespace Microsoft.AspNetCore.Mvc.RazorPages để làm việc với Razor Pages -->

using Microsoft.Extensions.Logging;
//<!-- Sử dụng namespace Microsoft.Extensions.Logging để ghi log thông tin, cảnh báo hoặc lỗi -->

using System.Text;
//<!-- Sử dụng namespace System.Text để làm việc với các chuỗi và mã hóa, như Encoding.UTF8 -->

using System.Threading.Tasks;
//<!-- Sử dụng namespace System.Threading.Tasks để làm việc với các phương thức bất đồng bộ(async/await) -->

using Microsoft.AspNetCore.WebUtilities;
//<!-- Sử dụng namespace Microsoft.AspNetCore.WebUtilities để làm việc với các tiện ích web, như mã hóa và giải mã Base64Url -->

using Quản_lý_khu_chung_cư.Models;
//<!-- Sử dụng namespace BanNuocHoa.Models để truy cập lớp ApplicationUser -->

namespace Quản_lý_khu_chung_cư.Areas.Identity.Pages.Account
{
    //<!-- Định nghĩa namespace BanNuocHoa.Areas.Identity.Pages.Account để chứa các trang liên quan đến tài khoản người dùng -->

    public class VerifyEmailModel : PageModel
    {
        //<!-- Định nghĩa lớp VerifyEmailModel, kế thừa từ PageModel -->
        //<!-- Lớp này quản lý logic cho trang xác minh email -->

        private readonly UserManager<NguoiDung> _userManager;
        //<!-- Khai báo một trường private readonly để lưu trữ đối tượng UserManager<ApplicationUser> -->
        //<!-- Đối tượng này được sử dụng để quản lý người dùng, bao gồm xác minh email -->

        private readonly ILogger<VerifyEmailModel> _logger;
        //<!-- Khai báo một trường private readonly để lưu trữ đối tượng ILogger<VerifyEmailModel> -->
        //<!-- Đối tượng này được sử dụng để ghi log thông tin, cảnh báo hoặc lỗi -->

        public VerifyEmailModel(UserManager<NguoiDung> userManager, ILogger<VerifyEmailModel> logger)
        {
            _userManager = userManager;
            //< !--Gán đối tượng UserManager<ApplicationUser> được truyền vào constructor cho trường _userManager -->

            _logger = logger;
            //< !--Gán đối tượng ILogger<VerifyEmailModel> được truyền vào constructor cho trường _logger -->
        }

        public async Task<IActionResult> OnGetAsync(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
            return Page();
        }
    }
}
