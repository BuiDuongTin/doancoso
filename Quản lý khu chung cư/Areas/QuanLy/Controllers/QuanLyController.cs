using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quản_lý_khu_chung_cư.Models;

namespace Quản_lý_khu_chung_cư.Areas.QuanLy.Controllers
{
    [Area("QuanLy")]
    public class QuanLyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<NguoiDung> _userManager;

        public QuanLyController(ApplicationDbContext context, UserManager<NguoiDung> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Quản lý danh sách cư dân

        public async Task<IActionResult> DanhSachCuDan()
        {
            var residents = await _context.NguoiDung.ToListAsync();
            return View(residents);
        }

        // Hiển thị danh sách dịch vụ
        public IActionResult DanhSachDichVu()
        {
            var dichVu = _context.PhiDichVu.ToList();
            return View(dichVu);
        }

        // Thêm dịch vụ
        [HttpPost]
        public IActionResult ThemDichVu(PhiDichVu dichVu)
        {
            if (ModelState.IsValid)
            {
                _context.PhiDichVu.Add(dichVu);
                _context.SaveChanges();
                return RedirectToAction("DanhSachDichVu");
            }
            return View(dichVu);
        }

        // Sửa dịch vụ
        [HttpPost]
        public IActionResult SuaDichVu(PhiDichVu dichVu)
        {
            if (ModelState.IsValid)
            {
                _context.PhiDichVu.Update(dichVu);
                _context.SaveChanges();
                return RedirectToAction("DanhSachDichVu");
            }
            return View(dichVu);
        }

        // Xóa dịch vụ
        [HttpPost]
        public IActionResult XoaDichVu(int id)
        {
            var dichVu = _context.PhiDichVu.Find(id);
            if (dichVu != null)
            {
                _context.PhiDichVu.Remove(dichVu);
                _context.SaveChanges();
            }
            return RedirectToAction("DanhSachDichVu");
        }
        // Xuất hóa đơn
        public async Task<IActionResult> XuatHoaDon(int id)
        {
            var invoice = await _context.HoaDon.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            return View(invoice);
        }

        // Xử lý phản hồi/sự cố của cư dân
        public async Task<IActionResult> PhanHoi()
        {
            var feedbacks = await _context.PhanAnh.ToListAsync();
            return View(feedbacks);
        }

        // Quản lý tiện ích (đặt chỗ, lịch bảo trì)
        public async Task<IActionResult> TienIch()
        {
            var utilities = await _context.TienIch.ToListAsync();
            return View(utilities);
        }

        public async Task<IActionResult> LichBaoTri()
        {
            var maintenanceSchedules = await _context.LichBaoTri.ToListAsync();
            return View(maintenanceSchedules);
        }

        // Gửi thông báo cho cư dân
        public IActionResult GuiThongBao()
        {
            ViewBag.Users = _context.NguoiDung.ToList();
            return View(new ThongBao());
        }

        [HttpPost]
        public async Task<IActionResult> GuiThongBao(ThongBao thongBao)
        {
            if (ModelState.IsValid)
            {
                thongBao.NgayTao = DateTime.Now;
                _context.ThongBao.Add(thongBao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(DanhSachCuDan));
            }
            ViewBag.Users = _context.NguoiDung.ToList();
            return View(thongBao);
        }
    }
}
