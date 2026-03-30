using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Quản_lý_khu_chung_cư.Models;

namespace Quản_lý_khu_chung_cư.Areas.CuDan.Controllers
{
    [Area("CuDan")]

    public class CuDanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CuDanController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Xem và thanh toán phí dịch vụ
        public IActionResult PhiDichVu()
        {
            var phiDichVu = _context.PhiDichVu.Where(p => p.DaThanhToan == false).ToList();
            return View(phiDichVu);
        }

        [HttpPost]
        public IActionResult ThanhToanPhiDichVu(int id)
        {
            var phi = _context.PhiDichVu.Find(id);
            if (phi != null)
            {
                phi.DaThanhToan = true;
                _context.SaveChanges();
            }
            return RedirectToAction("PhiDichVu");
        }

        // 2. Đặt tiện ích chung
        public IActionResult TienIch()
        {
            var tienIch = _context.TienIch.ToList();
            return View(tienIch);
        }

        [HttpPost]
        public IActionResult DatTienIch(int id, DateTime ngayDat, TimeSpan gioBatDau, TimeSpan gioKetThuc)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var datTienIch = new DatTienIch
            {
                TienIchId = id,
                NgayDat = ngayDat,
                GioBatDau = gioBatDau,
                GioKetThuc = gioKetThuc,
                TrangThai = "ChoDuyet",
                NguoiDungId = userId
            };
            _context.DatTienIch.Add(datTienIch);
            _context.SaveChanges();
            return RedirectToAction("TienIch");
        }

        // 3. Gửi phản hồi hoặc báo cáo sự cố
        public IActionResult GuiPhanHoi()
        {
            ViewBag.LoaiSuCoList = new List<string>
            {
                "Điện",
                "Nước",
                "An ninh",
                "Vệ sinh",
                "Khác"
            };
            return View();
        }

        [HttpPost]
        public IActionResult GuiPhanHoi(string loaiSuCo, string noiDung)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized(); // Người dùng chưa đăng nhập
            }

            var phanAnh = new PhanAnh
            {
                LoaiSuCo = loaiSuCo,
                NoiDung = noiDung,
                TrangThai = "DangXuLy",
                NgayTao = DateTime.Now,
                NguoiDungId = userId
            };
            _context.PhanAnh.Add(phanAnh);
            _context.SaveChanges();
            return RedirectToAction("GuiPhanHoi");
        }

        // 4. Xem thông báo
        public IActionResult ThongBao()
        {
            var thongBao = _context.ThongBao.OrderByDescending(t => t.NgayTao).ToList();
            return View(thongBao);
        }
    }
}
