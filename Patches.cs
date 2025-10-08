using Corpus_Callosum.AnimHandler;
using Corpus_Callosum.Crest;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using TeamCherry.Localization;
using UnityEngine;
using static PlayerDataTest;
using weaverCrest = Corpus_Callosum.Crest.WeaverCrest;


namespace Corpus_Callosum;

[HarmonyPatch]
public static class Patches
{
    //This patch is specifically because I use resources from runtime, mainly for HeroController.instance.
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.Start))]
    [HarmonyPostfix]
    private static void Postfix()
    {
        try
        {
            AnimManager.InitAnimations();
            CrestManager.AddCrest();
        }
        catch (Exception ex)
        {
            return;
        }

    }

    [HarmonyPatch(typeof(Language), nameof(Language.SwitchLanguage), typeof(LanguageCode))]
    [HarmonyPostfix]
    private static void AddNewSheet()
    {
        Dictionary<string, Dictionary<string, string>> fullStore = Helper.GetPrivateStaticField<Dictionary<string, Dictionary<string, string>>>(typeof(Language), "_currentEntrySheets");

        fullStore.Add("Corpus_Callosum", new Dictionary<string, string>()
        {
            { "WEAVERCRESTNAME", "Weaver Crest" },
            { "WEAVERCRESTDESC", "Me when the when the when I when :)" }
        });

        Helper.SetPrivateStaticField(typeof(Language), "_currentEntrySheets", fullStore);
    }

    //Nail Art charge time patch
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.instance.CurrentNailChargeTime), MethodType.Getter)]
    [HarmonyPostfix]
    private static void ModifyChargeTime(ref float __result)
    {
        if (Globals.Weaver.IsEquipped)
        {
            __result = 0.45f;
        }
    }

    //Nail Art charge begin time patch
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.instance.CurrentNailChargeBeginTime), MethodType.Getter)]
    [HarmonyPostfix]
    private static void ModifyBeginTime(ref float __result)
    {
        if (Globals.Weaver.IsEquipped)
        {
            __result = 0.25f;
        }
    }

    #region Weaver Exclusives
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.instance.BindCompleted), MethodType.Getter)]
    [HarmonyPostfix]
    private static void WeaverSilkRefund(HeroController __instance)
    {
        Globals.Logger.Log("WeaverSilkRefund");
        if (Globals.Weaver.IsEquipped)
        {
            __instance.AddSilk(4, true, SilkSpool.SilkAddSource.Normal);
        }
    }

    [HarmonyPatch(typeof(HeroController), nameof(HeroController.instance.NailHitEnemy), MethodType.Getter)]
    [HarmonyPostfix]
    private static void WeaverSilkGain(HeroController __instance)
    {
        Globals.Logger.Log("WeaverSilkGain");
        if (Globals.Weaver.IsEquipped)
        {
            __instance.AddSilk(1, true, SilkSpool.SilkAddSource.Normal);
        }
    }

    // DEBUG TOOL
    ///*
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.instance.Jump), MethodType.Getter)]
    [HarmonyPostfix]
    private static void WeaverEquip(HeroController __instance)
    {
        Globals.Logger.Log("WeaverEquip");
        if (!Globals.Weaver.IsEquipped)
        {
            ToolItemManager.UnlockAllCrests();
            ToolItemManager.SetEquippedCrest("Weaver");
            Globals.Logger.Log("Weaver Crest Equipped");
        }
        else
        {
            Globals.Logger.Log($"Weaver Crest FAILED AT EQUIPPING WHY ARE YOU LIKE THIS\n    Weaver.IsEquipped: {Globals.Weaver.IsEquipped}\n    Crest List: {ToolItemManager.GetAllCrests}");
        }
        //*/
        #endregion
    }
}