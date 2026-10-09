using System;

public class DateTimeConverter
{
    public DateTimeConverter()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static DateTime ConvertToIST(DateTime dateTime)
    {
        DateTime utcTime = dateTime.ToUniversalTime();
        TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime istTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, istZone);
        return istTime;
    }
}
