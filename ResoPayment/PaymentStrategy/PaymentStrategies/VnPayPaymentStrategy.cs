using Newtonsoft.Json;
using ResoPayment.Enums;
using ResoPayment.Helpers;
using ResoPayment.Infrastructure.PaymentConfigModels;
using ResoPayment.Payload.Response;

namespace ResoPayment.PaymentStrategy.PaymentStrategies;

public class VnPayPaymentStrategy : IPaymentStrategy
{
    private readonly VnPayConfig _vnPayConfig;
    private readonly HttpContext _context;
    private readonly Guid _orderId;
    private readonly string _orderDescription;
    private readonly double _amount;
    private readonly string _returnUrl;
    private readonly string _vnpHashSecret;
    public VnPayPaymentStrategy(string VnPayConfigJson, HttpContext context, Guid orderId, string orderDescription, double amount, string returnUrl, string vnpHashSecret)
    {
        _context = context;
        _orderDescription = orderDescription;
        _amount = amount;
        _returnUrl = returnUrl;
        _vnpHashSecret = vnpHashSecret;
        _orderId = orderId;
        _vnPayConfig = JsonConvert.DeserializeObject<VnPayConfig>(VnPayConfigJson) ?? throw new InvalidOperationException();
    }

    public CreatePaymentResponse ExecutePayment()
    {
        var VnPayHelper = new VnPayLibrary();
        var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
        var tick = DateTime.Now.Ticks.ToString();
        var pay = new VnPayLibrary();
        var urlCallBack = _returnUrl;

        pay.AddRequestData("vnp_Version", _vnPayConfig.Version);
        pay.AddRequestData("vnp_Command", _vnPayConfig.Command);
        pay.AddRequestData("vnp_TmnCode", _vnPayConfig.TmnCode);
        pay.AddRequestData("vnp_Amount", ((int)_amount * 100).ToString());
        pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
        pay.AddRequestData("vnp_CurrCode", _vnPayConfig.CurrCode);
        pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(_context));
        pay.AddRequestData("vnp_Locale", "vn");
        pay.AddRequestData("vnp_OrderInfo", _orderDescription);
        pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
        pay.AddRequestData("vnp_TxnRef", _orderId.ToString());

        var paymentUrl = pay.CreateRequestUrl(_vnPayConfig.BaseUrl, _vnpHashSecret);

        CreatePaymentResponse createPaymentResponse = new CreatePaymentResponse();
        createPaymentResponse.Url = paymentUrl;
        createPaymentResponse.Message = "Sussess";
        createPaymentResponse.DisplayType = "URL";

        return createPaymentResponse;
    }
}