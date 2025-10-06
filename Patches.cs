using Corpus_Callosum.Crest;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using TeamCherry.Localization;
using UnityEngine;
using Corpus_Callosum.AnimHandler;

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
        if (HeroController.instance.playerData.CurrentCrestID == "Weaver")
        {
            __result = 0.45f;
        }
    }

    //Nail Art charge begin time patch
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.instance.CurrentNailChargeBeginTime), MethodType.Getter)]
    [HarmonyPostfix]
    private static void ModifyBeginTime(ref float __result)
    {
        if (HeroController.instance.playerData.CurrentCrestID == "Weaver")
        {
            __result = 0.25f;
        }
    }

    //Hit instance for style meter
    [HarmonyPatch(typeof(HealthManager), nameof(HealthManager.TakeDamage))]
    [HarmonyPostfix]
    private static void SendHitToManager(HitInstance hitInstance)
    {
        if (HeroController.instance == null) { return; }

        WeaverCrestHandler handler = HeroController.instance.GetComponent<WeaverCrestHandler>();
        if (handler == null) { return; }

        handler.HitLanded(hitInstance);
    }

    //got hit for style meter
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.instance.TakeDamage), typeof(GameObject), typeof(GlobalEnums.CollisionSide), typeof(int), typeof(GlobalEnums.HazardType), typeof(GlobalEnums.DamagePropertyFlags))]
    [HarmonyPostfix]
    private static void SendGotHitToManager()
    {
        if (HeroController.instance == null) { return; }

        WeaverCrestHandler handler = HeroController.instance.GetComponent<WeaverCrestHandler>();
        if (handler == null) { return; }

        handler.GotHit();
    }

}