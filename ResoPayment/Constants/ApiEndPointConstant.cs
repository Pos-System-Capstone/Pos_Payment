namespace ResoPayment.Constants;

public static class ApiEndPointConstant
{
	static ApiEndPointConstant()
	{

	}

	public const string RootEndPoint = "/api";
	public const string ApiVersion = "/v1";
	public const string ApiEndpoint = RootEndPoint + ApiVersion;

	public static class Order
	{
		public const string OrderEndpoint = ApiEndpoint + "/orders";
	}

	public static class Payment
	{
		public const string PaymentEndpoint = ApiEndpoint + "/payments";
		public const string ZaloPayEndpoint = PaymentEndpoint + "/zalopay";
		public const string VnPayEndpoint = PaymentEndpoint + "/vnpay";
        public const string PaymentProviderEndpoint = PaymentEndpoint + "/payment-providers";
        public const string CheckTransactionStatus = ApiEndpoint + "/check-transaction-status";
        public const string VietQrEndpoint = PaymentEndpoint + "/vietqr";
	}

	public static class Brand
	{
		public const string BrandEndpoint = ApiEndpoint + "/brands";

		public const string CreateBrandPaymentProviderMappingEndPoint =
			BrandEndpoint + "/{id}/brandpaymentprovider";
	}

}