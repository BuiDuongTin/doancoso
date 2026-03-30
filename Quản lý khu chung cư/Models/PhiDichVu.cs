namespace Quản_lý_khu_chung_cư.Models
{
    public class PhiDichVu
    {
        public int Id { get; set; }
        public string LoaiPhi { get; set; } // Điện, Nước, Vệ sinh, Bảo trì,...
        public decimal SoTien { get; set; }
        public DateTime HanThanhToan { get; set; }
        public bool DaThanhToan { get; set; }

        // New property
        public string NguoiDungId { get; set; } 
        public NguoiDung NguoiDung { get; set; }
    }

}
