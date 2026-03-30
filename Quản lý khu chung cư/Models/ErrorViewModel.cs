namespace Quản_lý_khu_chung_cư.Models
{
    public class ErrorViewModel
    {
        public int Id { get; set; } 
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
