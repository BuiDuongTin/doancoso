using Microsoft.AspNetCore.Identity;

namespace Quản_lý_khu_chung_cư.Models
{
    public class NguoiDung : IdentityUser
    {
        public string HoTen { get; set; }
        public string SoDienThoai { get; set; }
        public DateTime NgayTao { get; set; }

        // Quan hệ
        public ICollection<HoaDon> HoaDons { get; set; }
        public ICollection<DatTienIch> DatTienIchs { get; set; }
        public ICollection<PhanAnh> PhanAnhs { get; set; }

        // Thông tin cư trú
        public int? CanHoId { get; set; }
        public CanHo CanHo { get; set; }

        public int? BietThuId { get; set; }
        public BietThu BietThu { get; set; }      
    }

}
