namespace Quản_lý_khu_chung_cư.Models
{
    public class PaymentResponseModel
    {
        public string MoTaDonHang { get; set; }
        public string IdGiaoDich { get; set; }
        public string IdDonHang { get; set; }
        public string PhuongThucThanhToan { get; set; }
        public string IdThanhToan { get; set; }
        public bool ThanhCong { get; set; }
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; }
    }

}
