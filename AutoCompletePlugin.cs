using BepInEx;
using HarmonyLib;
using UnityEngine;
using BepInEx.Logging;
using BepInEx.Configuration;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;

namespace AutoCompleteFetchQuests;

[BepInPlugin("j3kun.autocompletefetchquests.mod", "AutoCompleteFetchQuests", "1.0.0")]
public class AutoCompletePlugin : BaseUnityPlugin
{
    private static Harmony harmony;
    internal static ManualLogSource bepLogger;



     private void Awake()
    {
        bepLogger = Logger;
        bepLogger.LogInfo("AutoCompleteFetchQuests Loaded.");
    }

    private void Start()
    {
        AutoCompleteFetchQuestsUIConfig.LoadConfig(Config);
        harmony = new Harmony("j3kun.autocompletefetchquests.mod");
        harmony.PatchAll(typeof(AutoComplete));
        bepLogger.LogInfo("HarmonyPatchLoaded for AutoCompleteFetchQuests");
        if (this.IsModOptionsInstalled())
      {
        try
        {
          UIController.RegisterUI();
        }
        catch (Exception ex)
        {
          bepLogger.LogError((object) ("An error occurred during UI registration: " + ex.Message));
        }
      }
      else
        bepLogger.LogError((object) "Mod Options is not installed. Skipping UI registration.");
    }
    

    private void OnDestroy()
    {
        harmony.UnpatchSelf();
    }

    private bool IsModOptionsInstalled()
    {
      try
      {
        return ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).Any<Assembly>((Func<Assembly, bool>) (assembly => assembly.GetName().Name == "ModOptions"));
      }
      catch (Exception ex)
      {
        bepLogger.LogError((object) ("Error while checking for Mod Options: " + ex.Message));
        return false;
      }
    }

}