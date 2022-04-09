using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace SiliconRadarControlPanel.Infrastructure;

public interface IWritableSettings { }
public interface IReadableSettings { }

public static class SettingsExtentions
{
    private const string SettingsFileName = Ioc.SettingsFileName;

    public static void SaveToFile<T>(this T settings) where T : IWritableSettings
    {
        string settingsPath = $"{AppContext.BaseDirectory}/{SettingsFileName}";

        string? jsonString = File.ReadAllText(settingsPath);

        if (jsonString.TryUpdateJsonObject(settings, out jsonString) is false)
            return;

        File.WriteAllText(settingsPath, jsonString);
    }

    public static void LoadFromFile<T>(this T settings) where T : IReadableSettings
    {
        string settingsPath = $"{AppContext.BaseDirectory}/{SettingsFileName}";

        string jsonString = File.ReadAllText(settingsPath);

        JObject? jsonObject = JsonConvert.DeserializeObject<JObject>(jsonString);

        JToken? sectionObject = jsonObject?[typeof(T).Name];

        if (sectionObject == null)
            return;

        T? newSettings = sectionObject.ToObject<T?>();

        CopyAllProperty(newSettings, settings);
    }

    public static async Task SaveToFileAsync<T>(this T settings) where T : IWritableSettings
    {
        string settingsPath = $"{AppContext.BaseDirectory}/{SettingsFileName}";

        string? jsonString = await File.ReadAllTextAsync(settingsPath);

        if (jsonString.TryUpdateJsonObject(settings, out jsonString) is false)
            return;

        await File.WriteAllTextAsync(settingsPath, jsonString);
    }

    private static bool TryUpdateJsonObject<T>(this string inObject, T sectionObject, out string outObject)
    {
        JObject jsonObject = JsonConvert.DeserializeObject<JObject>(inObject) ?? new JObject();
        JObject jsonSectionObject = JObject.Parse(JsonConvert.SerializeObject(sectionObject));

        if (JToken.DeepEquals(jsonObject[typeof(T).Name], jsonSectionObject) is true)
        {
            outObject = string.Empty;
            return false;
        }

        jsonObject[typeof(T).Name] = jsonSectionObject;
        outObject = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
        return true;
    }

    private static void CopyAllProperty<T>(T source, T target)
    {
        Type type = typeof(T);
        foreach (PropertyInfo sourceProperty in type.GetProperties())
        {
            PropertyInfo? targetProperty = type.GetProperty(sourceProperty.Name);
            targetProperty?.SetValue(target, sourceProperty.GetValue(source, null), null);
        }
    }
}
