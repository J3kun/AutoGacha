using BepInEx;
using EvilMask.Elin.ModOptions;
using EvilMask.Elin.ModOptions.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;


namespace AutoCompleteFetchQuests;

  public static class UIController
  {
    public static void RegisterUI()
    {
      foreach (object obj in ModManager.ListPluginObject)
      {
        if (obj is BaseUnityPlugin baseUnityPlugin && baseUnityPlugin.Info.Metadata.GUID == "evilmask.elinplugins.modoptions")
        {
          ModOptionController controller = ModOptionController.Register("j3kun.autocompletefetchquests.mod", "AutoCompleteFetchQuests");
          AutoCompletePlugin.bepLogger.LogInfo(controller.ToString());
          controller.SetTranslation("j3kun.autocompletefetchquests.mod", "AutoCompleteFetchQuests");
          string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
          if (directoryName != null)
          {
            AutoCompleteFetchQuestsUIConfig.InitializeXmlPath(Path.Combine(directoryName, "AutoCompleteFetchQuestsUIConfig.xml"));
          }
          if (File.Exists(AutoCompleteFetchQuestsUIConfig.XmlPath))
          {
            using (StreamReader streamReader = new StreamReader(AutoCompleteFetchQuestsUIConfig.XmlPath))
              controller.SetPreBuildWithXml(streamReader.ReadToEnd());
          }
          UIController.RegisterEvents(controller);
        }
      }
    }

    private static void RegisterEvents(ModOptionController controller)
    {
      controller.OnBuildUI += builder =>
      {
          builder.GetPreBuild<OptHLayout>("hlayout01").Base.childForceExpandHeight = false;
          builder.GetPreBuild<OptVLayout>("vlayout01").Base.childForceExpandHeight = false;
          OptToggle preBuild1 = builder.GetPreBuild<OptToggle>("autoCompleteEnabled");
          preBuild1.Checked = AutoCompleteFetchQuestsUIConfig.autoCompleteEnabled.Value;
          preBuild1.OnValueChanged += (Action<bool>)(isChecked => AutoCompleteFetchQuestsUIConfig.autoCompleteEnabled.Value = isChecked);
      };
    }
  }



