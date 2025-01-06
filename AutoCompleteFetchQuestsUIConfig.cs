using BepInEx.Configuration;
using System.IO;


namespace AutoCompleteFetchQuests;
  internal static class AutoCompleteFetchQuestsUIConfig
  {
    internal static ConfigEntry<bool> autoCompleteEnabled;
    internal static string XmlPath { get; private set; }

    internal static void LoadConfig(ConfigFile config)
    {
      AutoCompleteFetchQuestsUIConfig.autoCompleteEnabled = config.Bind("AutoCompleteFetchQuests", "Enabled AutoCompletion", true, "Enables autocompletion of fetch quests when loading the quest board");
    }

    internal static void InitializeXmlPath(string xmlPath)
    {
      if (File.Exists(xmlPath))
        AutoCompleteFetchQuestsUIConfig.XmlPath = xmlPath;
      else
        AutoCompleteFetchQuestsUIConfig.XmlPath = string.Empty;
    }

  }

  

