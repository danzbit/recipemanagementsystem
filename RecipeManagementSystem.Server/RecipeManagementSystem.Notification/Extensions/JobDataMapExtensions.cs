using System.Text.Json;
using Quartz;

namespace RecipeManagementSystem.Notification.Extensions;

public static class JobDataMapExtensions
{
    public static void PutTyped<T>(this JobDataMap map, T value)
    {
        var json = JsonSerializer.Serialize(value);
        map.Put(typeof(T).Name, json);
    }

    public static T GetTyped<T>(this JobDataMap map)
    {
        var json = map.GetString(typeof(T).Name);
        return JsonSerializer.Deserialize<T>(json);
    }
}