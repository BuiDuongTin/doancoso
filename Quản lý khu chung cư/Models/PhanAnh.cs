namespace Quản_lý_khu_chung_cư.Models
{
    public class PhanAnh
    {
        public int Id { get; set; }
        public string LoaiSuCo { get; set; } // Điện, Nước, An ninh, Vệ sinh,...
        public string NoiDung { get; set; }
        public string TrangThai { get; set; } // DangXuLy, DaXuLy, TuChoi
        public DateTime NgayTao { get; set; }

        public string NguoiDungId { get; set; } // Changed to string for IdentityUser compatibility
        public NguoiDung NguoiDung { get; set; }
    }

}
