using System.Text;

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

    public static string BasicNumberMaskFormatter(string text, string format)
    {
        var sb = new StringBuilder();
        int textIndex = 0;

        foreach (var symbol in format)
        {
            if (symbol == '0')
            {
                if (textIndex < text.Length)
                {
                    sb.Append(text[textIndex]);
                    textIndex++;
                }
                else
                {
                    sb.Append('0');
                }
            }
            else
            {
                sb.Append(symbol);
            }
        }

        return sb.ToString();
    }
}