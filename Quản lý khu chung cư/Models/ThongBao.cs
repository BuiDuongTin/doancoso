namespace Quản_lý_khu_chung_cư.Models
{
    public class ThongBao
    {
        public int Id { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public DateTime NgayTao { get; set; }

        // New properties
        public string NguoiNhanId { get; set; }
        public NguoiDung NguoiNhan { get; set; }
    }

}
