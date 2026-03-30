namespace Quản_lý_khu_chung_cư.Models
{
    public class RaVao
    {
        public int Id { get; set; }
        public string TenKhach { get; set; }
        public string MucDich { get; set; }
        public DateTime ThoiGianVao { get; set; }
        public DateTime? ThoiGianRa { get; set; }

        public string NguoiDungId { get; set; } // Changed to string for IdentityUser compatibility
        public NguoiDung NguoiDung { get; set; }
    }

}
