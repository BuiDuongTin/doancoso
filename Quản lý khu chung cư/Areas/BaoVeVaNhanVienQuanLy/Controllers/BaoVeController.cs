using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quản_lý_khu_chung_cư.Models;

namespace Quản_lý_khu_chung_cư.Areas.BaoVeVaNhanVienQuanLy.Controllers
{
    [Area("BaoVeVaNhanVienQuanLy")]
    public class BaoVeController : Controller
    {
        private ApplicationDbContext _context;

        public BaoVeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách phản ánh
        public async Task<IActionResult> DanhSachPhanAnh()
        {
            var danhSach = await _context.PhanAnh
                                .Include(p => p.NguoiDung)
                                .ToListAsync();
            return View(danhSach);
        }


        // Cập nhật trạng thái phản ánh
        [HttpPost]
        public IActionResult CapNhatTrangThai(int id, string trangThai)
        {
            var phanAnh = _context.PhanAnh.FirstOrDefault(p => p.Id == id);
            if (phanAnh == null)
            {
                return NotFound();
            }

            // Cập nhật trạng thái
            phanAnh.TrangThai = trangThai;
            _context.SaveChanges();

            // Quay lại danh sách phản ánh
            return RedirectToAction("DanhSachPhanAnh");
        }



        // Hiển thị danh sách lịch bảo trì
        public async Task<IActionResult> DanhSachBaoTri()
        {
            var baoTri = await _context.LichBaoTri.Include(l => l.TienIch).ToListAsync();
            return View(baoTri);
        }
    }
}
