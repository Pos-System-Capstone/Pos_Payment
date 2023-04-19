using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using ResoPayment.RedisModels;

namespace ResoPayment.Helpers;

public static class RedisHelper
{
	public static byte[] EncodeOrderData(OrderData orderData)
	{
		string serializedCustomerList = JsonConvert.SerializeObject(orderData);
		byte[] orderDataBytes = Encoding.UTF8.GetBytes(serializedCustomerList);
		return orderDataBytes;
	}

	public static DistributedCacheEntryOptions SetUpRedisEntryOptions()
	{
		var option = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
			.SetSlidingExpiration(TimeSpan.FromMinutes(2));
		return option;
	}

}