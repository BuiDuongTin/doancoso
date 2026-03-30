namespace Quản_lý_khu_chung_cư.Models
{
    public class DatTienIch
    {
        public int Id { get; set; } 
        public DateTime NgayDat { get; set; }
        public TimeSpan GioBatDau { get; set; }
        public TimeSpan GioKetThuc { get; set; }
        public string TrangThai { get; set; } // DaXacNhan, ChoDuyet, DaHuy

        public int TienIchId { get; set; }
        public TienIch TienIch { get; set; }

        public string NguoiDungId { get; set; } // Changed to string for IdentityUser compatibility
        public NguoiDung NguoiDung { get; set; }
    }

}
