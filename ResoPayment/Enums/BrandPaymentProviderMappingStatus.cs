using System.ComponentModel;

namespace ResoPayment.Enums;

public enum BrandPaymentProviderMappingStatus
{
    [Description("Active")]
    Active
}

public enum CreatePaymentReturnType
{
    [Description("URL")]
    Url,
    [Description("QR")]
    Qr
}