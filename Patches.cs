using Corpus_Callosum.AnimHandler;
using Corpus_Callosum.Crest;
using HarmonyLib;
using Needleforge;
using Needleforge.Makers;
using System;
using System.Collections.Generic;
using System.Reflection;
using TeamCherry.Localization;
using UnityEngine;
using static PlayerDataTest;


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
        }
        catch (Exception ex)
        {
            return;
        }
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

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ToolItemManager), nameof(ToolItemManager.Awake))]
    private static void AddCrests()
    {
        Globals.Logger.Log("No new class needed");
        foreach (string name in CorpusCallosumPlugin.crests)
        {
            Sprite RealSprite;
            Sprite Silhouette;
            ToolCrest.SlotInfo[] slots = [];
            if (name == "Weaver")
            {
                RealSprite = null;
                Silhouette = null;
                // Idx Ref   :  x0  x1  x2  x3  x4  x5  x6
                // Up Idx    :  s3 y2b2 s1 b1y1 s2
                // Down Idx  :  s2 b1y2 s1 y2b2 s3
                // Left Idx  : b1y2 s  b2y1
                // Right Idx : b2y1 s  b1y2

                // \===s2===/ //
                // /========\ //
                // b1 |  | y1 //
                //    |s1|    //
                // y2 |  | b2 //
                // \========/ //
                // /===s3===\ //
                slots.AddToArray(CorpusCallosumPlugin.RegisterSlot(new Vector2(0, 0), ToolItemType.Skill, AttackToolBinding.Neutral, 2, 2, 1, 1, 0, 4, 0, 2, true)); // s1
                slots.AddToArray(CorpusCallosumPlugin.RegisterSlot(new Vector2(0, 400), ToolItemType.Skill, AttackToolBinding.Up, 4, 0, 1, 1, 0, 4, 0, 2, false)); // s2
                slots.AddToArray(CorpusCallosumPlugin.RegisterSlot(new Vector2(0, -400), ToolItemType.Skill, AttackToolBinding.Down, 0, 4, 1, 1, 0, 4, 0, 2, false)); // s3
                                                                                                                                                                      // Blue
                slots.AddToArray(CorpusCallosumPlugin.RegisterSlot(new Vector2(-200, 200), ToolItemType.Blue, AttackToolBinding.Neutral, 3, 1, 0, 2, 0, 4, 0, 2, false)); // b1
                slots.AddToArray(CorpusCallosumPlugin.RegisterSlot(new Vector2(200, -200), ToolItemType.Blue, AttackToolBinding.Neutral, 1, 3, 2, 0, 0, 4, 0, 2, true)); // b2
                                                                                                                                                                         // Yellow
                slots.AddToArray(CorpusCallosumPlugin.RegisterSlot(new Vector2(200, -200), ToolItemType.Yellow, AttackToolBinding.Neutral, 1, 3, 2, 0, 0, 4, 0, 2, false)); // y1
                slots.AddToArray(CorpusCallosumPlugin.RegisterSlot(new Vector2(-200, 200), ToolItemType.Yellow, AttackToolBinding.Neutral, 3, 1, 0, 2, 0, 4, 0, 2, true)); // y2
                int crestID = NeedleforgePlugin.AddCrest(RealSprite, Silhouette, name, slots);
                NeedleforgePlugin.bindEvents[name] = (healValue, healAmount, healTime) =>
                {
                    healAmount = 3; healValue = 1; healTime = 1.1f;
                };
                var actualCrest = CrestMaker.CreateCrest(RealSprite, Silhouette, name, slots);
                actualCrest.displayName.Key = "Weaver";
                actualCrest.description.Key = "THE SPIDER IS NOW MORE SPIDER THAN BEFORE YIPPEE";
            }
            else if (name == "Bard")
            {
                RealSprite = null;
                Silhouette = null;
                int crestID = NeedleforgePlugin.AddCrest(RealSprite, Silhouette, name, slots);
                NeedleforgePlugin.bindEvents[name] = (healValue, healAmount, healTime) =>
                {
                    healAmount = 0; healValue = 0; healTime = 3;
                };
            }
            else if (name == "Vessel")
            {
                RealSprite = null;
                Silhouette = null;
                int crestID = NeedleforgePlugin.AddCrest(RealSprite, Silhouette, name, slots);
                NeedleforgePlugin.bindEvents[name] = (healValue, healAmount, healTime) =>
                {
                    healAmount = 0; healValue = 0; healTime = 3;
                };
            }
            else
            {
                RealSprite = null;
                Silhouette = null;
                Globals.Logger.Log($"Bro, how did you mess this up, it's literally a single string\nAnyway, here's the name your dumbass entered into CorpusCallosumPlugin.crest: {name}");
            }
        }

        #region Weaver Exclusives
        [HarmonyPatch(typeof(HeroController), nameof(HeroController.instance.BindCompleted), MethodType.Getter)] // Invalid
        [HarmonyPostfix]
        static void WeaverSilkRefund(HeroController __instance)
        {
            Globals.Logger.Log("WeaverSilkRefund");
            if (Globals.Weaver.IsEquipped)
            {
                __instance.AddSilk(4, true, SilkSpool.SilkAddSource.Normal);
            }
        }

        [HarmonyPatch(typeof(HeroController), nameof(HeroController.instance.NailHitEnemy), MethodType.Getter)]
        [HarmonyPostfix]
        static void WeaverSilkGain(HeroController __instance)
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
        static void WeaverEquip(HeroController __instance)
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

    [HarmonyPatch(typeof(ToolItemManager), nameof(ToolItemManager.Awake))]
    public class TestTest
    {
        [HarmonyPostfix]
        public static void AddCrests()
        {
            Globals.Logger.Log("New class needed");
            foreach (string name in CorpusCallosumPlugin.crests)
            {
                Sprite RealSprite;
                Sprite Silhouette;
                ToolCrest.SlotInfo[] slots = [];
                if (name == "Weaver")
                {
                    RealSprite = null;
                    Silhouette = null;
                    // Idx Ref   :  x0  x1  x2  x3  x4  x5  x6
                    // Up Idx    :  s3 y2b2 s1 b1y1 s2
                    // Down Idx  :  s2 b1y2 s1 y2b2 s3
                    // Left Idx  : b1y2 s  b2y1
                    // Right Idx : b2y1 s  b1y2

                    // \===s2===/ //
                    // /========\ //
                    // b1 |  | y1 //
                    //    |s1|    //
                    // y2 |  | b2 //
                    // \========/ //
                    // /===s3===\ //
                    slots.AddToArray(CorpusCallosumPlugin.RegisterSlot(new Vector2(0, 0), ToolItemType.Skill, AttackToolBinding.Neutral, 2, 2, 1, 1, 0, 4, 0, 2, true)); // s1
                    slots.AddToArray(CorpusCallosumPlugin.RegisterSlot(new Vector2(0, 400), ToolItemType.Skill, AttackToolBinding.Up, 4, 0, 1, 1, 0, 4, 0, 2, false)); // s2
                    slots.AddToArray(CorpusCallosumPlugin.RegisterSlot(new Vector2(0, -400), ToolItemType.Skill, AttackToolBinding.Down, 0, 4, 1, 1, 0, 4, 0, 2, false)); // s3
                                                                                                                                                                          // Blue
                    slots.AddToArray(CorpusCallosumPlugin.RegisterSlot(new Vector2(-200, 200), ToolItemType.Blue, AttackToolBinding.Neutral, 3, 1, 0, 2, 0, 4, 0, 2, false)); // b1
                    slots.AddToArray(CorpusCallosumPlugin.RegisterSlot(new Vector2(200, -200), ToolItemType.Blue, AttackToolBinding.Neutral, 1, 3, 2, 0, 0, 4, 0, 2, true)); // b2
                                                                                                                                                                             // Yellow
                    slots.AddToArray(CorpusCallosumPlugin.RegisterSlot(new Vector2(200, -200), ToolItemType.Yellow, AttackToolBinding.Neutral, 1, 3, 2, 0, 0, 4, 0, 2, false)); // y1
                    slots.AddToArray(CorpusCallosumPlugin.RegisterSlot(new Vector2(-200, 200), ToolItemType.Yellow, AttackToolBinding.Neutral, 3, 1, 0, 2, 0, 4, 0, 2, true)); // y2
                    int crestID = NeedleforgePlugin.AddCrest(RealSprite, Silhouette, name, slots);
                    NeedleforgePlugin.bindEvents[name] = (healValue, healAmount, healTime) =>
                    {
                        healAmount = 3; healValue = 1; healTime = 1.1f;
                    };
                    var actualCrest = CrestMaker.CreateCrest(RealSprite, Silhouette, name, slots);
                    actualCrest.displayName.Key = "Weaver";
                    actualCrest.description.Key = "THE SPIDER IS NOW MORE SPIDER THAN BEFORE YIPPEE";
                }
                else if (name == "Bard")
                {
                    RealSprite = null;
                    Silhouette = null;
                    int crestID = NeedleforgePlugin.AddCrest(RealSprite, Silhouette, name, slots);
                    NeedleforgePlugin.bindEvents[name] = (healValue, healAmount, healTime) =>
                    {
                        healAmount = 0; healValue = 0; healTime = 3;
                    };
                }
                else if (name == "Vessel")
                {
                    RealSprite = null;
                    Silhouette = null;
                    int crestID = NeedleforgePlugin.AddCrest(RealSprite, Silhouette, name, slots);
                    NeedleforgePlugin.bindEvents[name] = (healValue, healAmount, healTime) =>
                    {
                        healAmount = 0; healValue = 0; healTime = 3;
                    };
                }
                else
                {
                    RealSprite = null;
                    Silhouette = null;
                    Globals.Logger.Log($"Bro, how did you mess this up, it's literally a single string\nAnyway, here's the name your dumbass entered into CorpusCallosumPlugin.crest: {name}");
                }
            }
        }
    }
}