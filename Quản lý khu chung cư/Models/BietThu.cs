namespace Quản_lý_khu_chung_cư.Models
{
    public class BietThu
    {
        public int Id { get; set; }
        public string KhuVuc { get; set; }
        public string SoBietThu { get; set; }
        public string ChiTiet { get; set; } // Chi tiết mô tả
        public string TrangThai { get; set; } // Trạng thái: "Có sẵn", "Đã thuê", "Đã bán"

        //// Quan hệ
        //public string NguoiDungId { get; set; } // Người dùng thuê/mua
        //public NguoiDung NguoiDung { get; set; }
    }

}
