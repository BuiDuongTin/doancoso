using Quản_lý_khu_chung_cư.Models;

namespace Quản_lý_khu_chung_cư.Services.VnPay
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
