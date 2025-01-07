using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace AutoCompleteFetchQuests;

[HarmonyPatch]
public class AutoComplete
{
	static List<Quest> questsToComplete = new List<Quest>{};
	[HarmonyPrefix, HarmonyPatch(typeof(ItemQuest), nameof(ItemQuest.SetQuest))]
	 static bool logCompletableFetchQuest(Quest q)
	{
		if (!AutoCompleteFetchQuestsUIConfig.autoCompleteEnabled.Value) {
			return true;
		}
		// checks if its a supply quest and if item is available
		if (q is QuestSupply questSupply && questSupply.GetDestThing() != null)
        {
			AutoCompletePlugin.bepLogger.LogInfo("Logging a quest to autocomplete:" + q.uid);
			questsToComplete.Add(q);
        }
		return true;
	}

		[HarmonyPostfix, HarmonyPatch(typeof(LayerQuestBoard), nameof(LayerQuestBoard.RefreshQuest))]
	 static void AutoCompleteFetchQuests(LayerQuestBoard __instance)
	{
		if (!AutoCompleteFetchQuestsUIConfig.autoCompleteEnabled.Value) {
			return;
		}
		if (questsToComplete.Count == 0) {
			return;
		}
		AutoCompletePlugin.bepLogger.LogInfo("Autocompleting Quests");
		
		foreach(Quest q in questsToComplete) {
			AutoCompletePlugin.bepLogger.LogInfo("Autocompleted:" + q.uid);
			q.Deliver(q.chara, null);
		}
		questsToComplete.Clear();
		__instance.RefreshQuest();
	}

	[HarmonyPrefix, HarmonyPatch(typeof(QuestManager), nameof(QuestManager.Remove))]
	 static bool IgnoreAutoCompletedQuestRemoval(Quest q)
	{
		if (!AutoCompleteFetchQuestsUIConfig.autoCompleteEnabled.Value) {
			return true;
		}
		AutoCompletePlugin.bepLogger.LogInfo("Auto Ignore Quest Removal was called for quest:" + q.id);
		bool wasIgnored = false;
		
		if (questsToComplete.Contains(q)) {
			wasIgnored = true;
			AutoCompletePlugin.bepLogger.LogInfo("An autocompleted fetch quest was ignored.");
		}
		return !wasIgnored;
	}
}