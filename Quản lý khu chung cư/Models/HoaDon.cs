namespace Quản_lý_khu_chung_cư.Models
{
    public class HoaDon
    {
        public int Id { get; set; } 
        public string PhuongThucThanhToan { get; set; } // VNPay, MoMo, ZaloPay
        public decimal TongTien { get; set; }
        public DateTime NgayThanhToan { get; set; }
        public string TrangThai { get; set; } // DaThanhToan, ChoXuLy, ThatBai

        public string NguoiDungId { get; set; } // Changed to string for IdentityUser compatibility
        public NguoiDung NguoiDung { get; set; }
    }

}
