namespace Climacae.Api.Extensions;

public static class DateTimeExtensions
{
    /// <summary>
    /// Gets the start of the week for the given <paramref name="dt"/>, based on the specified <paramref name="startOfWeek"/>.
    /// </summary>
    /// <param name="dt">The DateTime object representing a specific date.</param>
    /// <param name="startOfWeek">The day of the week to consider as the start of the week (e.g., Sunday, Monday).</param>
    /// <returns>A DateTime representing the start of the week.</returns>
    public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
    {
        int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;

        return dt.AddDays(-1 * diff).Date;
    }


    /// <summary>
    /// Gets the end of the week for the given <paramref name="dt"/>, based on the specified <paramref name="startOfWeek"/>.
    /// The end of the week is calculated as 6 days after the start of the week.
    /// </summary>
    /// <param name="dt">The DateTime object representing a specific date.</param>
    /// <param name="startOfWeek">The day of the week to consider as the start of the week (e.g., Sunday, Monday).</param>
    /// <returns>A DateTime representing the end of the week, which is 6 days after the start of the week.</returns>
    public static DateTime EndOfWeek(this DateTime dt, DayOfWeek startOfWeek)
    {
        var start = dt.StartOfWeek(startOfWeek);

        var end = start.AddDays(6);

        return end.EndOfDay();
    }

    /// <summary>
    /// Gets the first day of the month for the given <paramref name="dt"/>.
    /// </summary>
    /// <param name="dt">The DateTime object representing a specific date.</param>
    /// <returns>A DateTime representing the first day of the month for the given date.</returns>
    public static DateTime StartOfMonth(this DateTime dt)
    {
        return new DateTime(dt.Year, dt.Month, 1);
    }

    /// <summary>
    /// Gets the last day of the month for the given <paramref name="dt"/>.
    /// The time is set to the end of the day (23:59:59).
    /// </summary>
    /// <param name="dt">The DateTime object representing a specific date.</param>
    /// <returns>A DateTime representing the last day of the month for the given date, at 23:59:59.</returns>
    public static DateTime EndOfMonth(this DateTime dt)
    {
        var lastDayOfMonth = DateTime.DaysInMonth(dt.Year, dt.Month);
        var date = new DateTime(dt.Year, dt.Month, lastDayOfMonth).Date;

        return date.EndOfDay(); ;
    }


    /// <summary>
    /// Gets the end of the day for the given <paramref name="dt"/>. This sets the time to 23:59:59.
    /// </summary>
    /// <param name="dt">The DateTime object representing a specific date.</param>
    /// <returns>A DateTime representing the end of the day (23:59:59) for the given date.</returns>
    public static DateTime EndOfDay(this DateTime dt)
    {
        var endOfDay = dt.Date
            .AddHours(23)
            .AddMinutes(59)
            .AddSeconds(59);

        return endOfDay;
    }

    /// <summary>
    /// Returns a list of DateTime objects starting from the specified initial date up until today.
    /// </summary>
    /// <param name="initialDate">The starting date from which the list of DateTime objects will be generated.</param>
    /// <returns>A list of DateTime objects from the initial date up until today's date.</returns>
    public static List<DateTime> GetDateTimeListUntilToday(this DateTime initialDate)
    {
        List<DateTime> allDates = [];

        for (var i = initialDate; i <= DateTime.Now; i = i.AddDays(1))
        {
            allDates.Add(i.Date);
        }

        return allDates;
    }

    /// <summary>
    /// Parses a DateTime object into a string representation without any separator between the year, month, and day.
    /// The format is: "yyyyMMdd".
    /// </summary>
    /// <param name="dt">The DateTime object to be parsed into a string.</param>
    /// <returns>A string representing the DateTime in the format "yyyyMMdd".</returns>
    public static string ParseStringFromDateWithoutSeparator(this DateTime dt)
    {
        return $"{dt.Year}{dt.Month:D2}{dt.Day:D2}";
    }
}