using BepInEx.Configuration;
using System.IO;


namespace AutoGacha;
  internal static class AutoGachaUIConfig
  {
    internal static ConfigEntry<bool> autoGachaEnabled;
    internal static string XmlPath { get; private set; }

    internal static void LoadConfig(ConfigFile config)
    {
      AutoGachaUIConfig.autoGachaEnabled = config.Bind("AutoGacha", "Enabled AutoGacha", true, "Enables auto Gacha ball dispensing and usage");
    }

    internal static void InitializeXmlPath(string xmlPath)
    {
      if (File.Exists(xmlPath))
        AutoGachaUIConfig.XmlPath = xmlPath;
      else
        AutoGachaUIConfig.XmlPath = string.Empty;
    }

  }

  

