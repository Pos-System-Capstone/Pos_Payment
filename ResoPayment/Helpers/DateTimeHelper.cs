namespace ResoPayment.Helpers;

public static class DateTimeHelper
{
	public static DateTime ConvertDateTimeToVietNamTimeZone()
	{
		TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
		DateTime localTime = DateTime.Now;
		DateTime utcTime = TimeZoneInfo.ConvertTime(localTime, TimeZoneInfo.Local, tz);
		return utcTime;
	}
}