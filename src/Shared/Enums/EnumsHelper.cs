namespace Shared.Event.Enums;

public static class EnumsHelper
{
    public static string GetDescription(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? value.ToString();
    }
}