using System;
using System.Collections.Generic;
using System.IO;

public class ConfigReader
{
    private static readonly string configFilePath = @"C:\Users\westinghouse\Desktop\Project1\C#\Printer\.config";
    private static Dictionary<string, string> configValues = new Dictionary<string, string>();

    static ConfigReader()
    {
        try
        {
            LoadConfig();
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to initialize ConfigReader", ex);
        }
    }

    private static void LoadConfig()
    {
        if (!File.Exists(configFilePath))
        {
            throw new FileNotFoundException("Configuration file not found", configFilePath);
        }

        var lines = File.ReadAllLines(configFilePath);
        foreach (var line in lines)
        {
            var parts = line.Split(new[] { '=' }, 2);
            if (parts.Length == 2)
            {
                var key = parts[0].Trim();
                var value = parts[1].Trim();
                configValues[key] = value;
            }
        }
    }

    public static string GetValue(string key)
    {
        if (configValues.TryGetValue(key, out var value))
        {
            return value;
        }

        throw new KeyNotFoundException($"Key '{key}' not found in configuration.");
    }
}
