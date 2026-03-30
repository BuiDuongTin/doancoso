namespace Quản_lý_khu_chung_cư.Models
{
    public class CanHo
    {
        public int Id { get; set; }
        public string ToaNha { get; set; }
        public int Tang { get; set; }
        public string SoCan { get; set; }
        public string ChiTiet { get; set; } 
        public string TrangThai { get; set; } // Trạng thái: "Có sẵn", "Đã thuê", "Đã bán"

        //// Quan hệ
        //public string NguoiDungId { get; set; } // Người dùng thuê/mua
        //public NguoiDung NguoiDung { get; set; }
    }

}
