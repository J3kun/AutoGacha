using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace AutoGacha;

[HarmonyPatch]
public class AutoGacha
{

	[HarmonyPrefix, HarmonyPatch(typeof(LayerDragGrid), nameof(LayerDragGrid.CreateGacha))]
     static bool AutoDispenseGachaBalls(ref LayerDragGrid __result,TraitGacha gacha)
	{
		if (!AutoGachaUIConfig.autoGachaEnabled.Value) {
			return true;
		}
		
		__result = LayerDragGrid.Create((InvOwnerDraglet) new InvOwnerAutoGacha(gacha.owner)
		{
		gacha = gacha
		});
		return false;
	}

	[HarmonyPostfix, HarmonyPatch(typeof(TraitGachaBall), nameof(TraitGachaBall.OnUse))]
	 static void AutoGachaBall(TraitGachaBall __instance, Chara c)
	{
		if (!AutoGachaUIConfig.autoGachaEnabled.Value) {
			return;
		}

		AutoGachaPlugin.bepLogger.LogInfo("Autogacha-ing gacha balls");
		if (__instance.owner.Num > 0) {
			__instance.OnUse(c);
		}
	}

}