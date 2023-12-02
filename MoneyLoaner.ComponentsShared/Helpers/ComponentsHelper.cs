namespace MoneyLoaner.ComponentsShared.Helpers;

public static class ComponentsHelper
{
    public static string FormatPhoneNumber(string? number)
    {
        if (string.IsNullOrEmpty(number))
            return string.Empty;

        number = number.Replace(" ", "");

        if (number.Length == 9)
        {
            return $"+48 {number[..3]} {number.Substring(3, 3)} {number.Substring(6, 3)}";
        }
        else
        {
            return string.Empty;
        }
    }
}