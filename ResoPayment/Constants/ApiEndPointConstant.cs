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
}