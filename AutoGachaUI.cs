using BepInEx;
using EvilMask.Elin.ModOptions;
using EvilMask.Elin.ModOptions.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;


namespace AutoGacha;

  public static class UIController
  {
    public static void RegisterUI()
    {
      foreach (object obj in ModManager.ListPluginObject)
      {
        if (obj is BaseUnityPlugin baseUnityPlugin && baseUnityPlugin.Info.Metadata.GUID == "evilmask.elinplugins.modoptions")
        {
          ModOptionController controller = ModOptionController.Register("j3kun.autogacha.mod", "AutoGacha");
          AutoGachaPlugin.bepLogger.LogInfo(controller.ToString());
          controller.SetTranslation("j3kun.autogacha.mod", "AutoGacha");
          string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
          if (directoryName != null)
          {
            AutoGachaUIConfig.InitializeXmlPath(Path.Combine(directoryName, "AutoGachaUIConfig.xml"));
          }
          if (File.Exists(AutoGachaUIConfig.XmlPath))
          {
            using (StreamReader streamReader = new StreamReader(AutoGachaUIConfig.XmlPath))
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
          OptToggle preBuild1 = builder.GetPreBuild<OptToggle>("autoGachaEnabled");
          preBuild1.Checked = AutoGachaUIConfig.autoGachaEnabled.Value;
          preBuild1.OnValueChanged += (Action<bool>)(isChecked => AutoGachaUIConfig.autoGachaEnabled.Value = isChecked);
      };
    }
  }



