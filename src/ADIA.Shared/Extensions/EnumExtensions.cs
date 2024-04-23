using System.ComponentModel;

namespace ADIA.Shared.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        if (field == null)
        {
            return string.Empty;
        }
        var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))!;

        return attribute.Description ?? value.ToString();
    }
}