using System;
using System.IO;
using System.Text.Json;

namespace SiliconRadarControlPanel.Infrastructure;

public interface ISettings
{

}

public static class SettingsExtension
{
    public static void Save<T>(this T settings) where T : ISettings
    {
        var dirPath = AppContext.BaseDirectory + "/settings";
        var fileName = $".{settings.GetType().Name.ToLower()}";
        var filePath = $"{dirPath}/{fileName}.json";
        Directory.CreateDirectory(dirPath);
        string jsonString = settings.SaveToStr();
        File.WriteAllText(filePath, jsonString);
    }
    public static void Read<T>(this T settings) where T : ISettings
    {
        var dirPath = AppContext.BaseDirectory + "/settings";
        var fileName = $".{settings.GetType().Name.ToLower()}";
        var filePath = $"{dirPath}/{fileName}.json";
        if (File.Exists(filePath) is false)
            return;
        var jsonString = File.ReadAllText(filePath);
        settings.ReadFromStr(jsonString);
    }

    private static string SaveToStr<T>(this T settings) where T : ISettings
    {
        var jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        return JsonSerializer.Serialize(settings, jsonOptions);
    }
    private static void ReadFromStr<T>(this T settings, string str) where T : ISettings
    {
        var jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        var newSettings = JsonSerializer.Deserialize<T>(str, jsonOptions);
        CopyAllProperty(newSettings, settings);
    }
    private static void CopyAllProperty<T>(T source, T target)
    {
        var type = typeof(T);
        foreach (var sourceProperty in type.GetProperties())
        {
            var targetProperty = type.GetProperty(sourceProperty.Name);
            targetProperty.SetValue(target, sourceProperty.GetValue(source, null), null);
        }
    }
}
