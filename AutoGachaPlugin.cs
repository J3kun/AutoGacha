using BepInEx;
using HarmonyLib;
using UnityEngine;
using BepInEx.Logging;
using BepInEx.Configuration;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;

namespace AutoGacha;

[BepInPlugin("j3kun.autogacha.mod", "AutoGacha", "1.0.0")]
public class AutoGachaPlugin : BaseUnityPlugin
{
    private static Harmony harmony;
    internal static ManualLogSource bepLogger;



     private void Awake()
    {
        bepLogger = Logger;
        bepLogger.LogInfo("AutoGacha Loaded.");
    }

    private void Start()
    {
        AutoGachaUIConfig.LoadConfig(Config);
        harmony = new Harmony("j3kun.autogacha.mod");
        harmony.PatchAll(typeof(AutoGacha));
        bepLogger.LogInfo("HarmonyPatchLoaded for AutoGacha");
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