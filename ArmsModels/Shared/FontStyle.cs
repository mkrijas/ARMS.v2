using System.Globalization;

public static class FontStyle
{
    public static string ConvertToCamelCase(string input)
    {
        CultureInfo cultureInfo = CultureInfo.CurrentCulture;
        TextInfo textInfo = cultureInfo.TextInfo;
        return textInfo.ToTitleCase(input.ToLower());
    }
}