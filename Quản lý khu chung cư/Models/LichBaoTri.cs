namespace Quản_lý_khu_chung_cư.Models
{
    public class LichBaoTri
    {
        public int Id { get; set; } 
        public DateTime NgayBaoTri { get; set; }
        public string NoiDung { get; set; }

        public int TienIchId { get; set; }
        public TienIch TienIch { get; set; }
    }

}
