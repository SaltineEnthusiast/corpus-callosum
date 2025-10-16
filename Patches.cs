using Corpus_Callosum.AnimHandler;
using Corpus_Callosum.Crest;
using HarmonyLib;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Needleforge;
using Needleforge.Data;
using Needleforge.Makers;
using Silksong.FsmUtil;
using System;
using System.Collections.Generic;
using System.Reflection;
using TeamCherry.Localization;
using UnityEngine;
using static PlayerDataTest;


namespace Corpus_Callosum;
internal class TestTest
{
    private void Awake()
    {
        CorpusCallosumPlugin.Log("Patches are awake");
    }
}

[HarmonyPatch(typeof(HeroController), nameof(HeroController.Start))]
internal class HeroControllerStart
{
    public static Action<FsmInt, FsmInt, FsmFloat> defaultBind = (value, amount, time) =>
    {
        value.Value = 3;
        amount.Value = 1;
        time.Value = 1.2f;
    };
    [HarmonyPostfix]
    internal void Postfix()
    {
        AnimManager.InitAnimations();
    }

    [HarmonyPostfix]
    public static void AddCrests(HeroController __instance) //AddToolsCrests.cs from Needleforge
    {
        ModHelper.Log("Adding Crests...");
        foreach (CrestData data in NeedleforgePlugin.newCrestData)
        {
            CrestMaker.CreateCrest(data.RealSprite, data.Silhouette, data.name, data.slots);
        }

        PlayMakerFSM bind = __instance.gameObject.GetFsmPreprocessed("Bind");
        FsmState CanBind = bind.GetState("Can Bind?");

        FsmState BindType = bind.GetState("Bind Type");

        FsmState QuickBind = bind.GetState("Quick Bind?");

        FsmInt healValue = bind.GetIntVariable("Heal Amount");
        FsmInt healAmount = bind.GetIntVariable("Bind Amount");
        FsmFloat healTime = bind.GetFloatVariable("Bind Time");

        foreach (ToolCrest crest in NeedleforgePlugin.newCrests)
        {
            FsmBool equipped = bind.AddBoolVariable($"Is {crest.name} Equipped");
            CanBind.AddAction(new CheckIfCrestEquipped()
            {
                Crest = crest,
                storeValue = equipped
            });

            FsmState newBindState = bind.AddState($"{crest.name} Bind");
            FsmEvent newBindTransition = BindType.AddTransition($"{crest.name}", newBindState.name);

            BindType.AddAction(new BoolTest()
            {
                boolVariable = equipped,
                isTrue = newBindTransition,
                everyFrame = false
            });

            newBindState.AddTransition("FINISHED", QuickBind.name);

            newBindState.AddLambdaMethod((action) =>
            {
                defaultBind.Invoke(healValue, healAmount, healTime);
                NeedleforgePlugin.bindEvents[crest.name].Invoke(healValue, healAmount, healTime);
                bind.SendEvent("FINISHED");
            });
        }
    }

    [HarmonyPostfix]
    public static void AddTools(HeroController __instance)
    {
        ModHelper.Log("Adding Tools...");
        foreach (ToolData data in NeedleforgePlugin.newToolData)
        {
            ModHelper.Log($"Adding {data.name}");
            ToolMaker.CreateBasicTool(data.inventorySprite, data.type, data.name);
        }
    }
}

[HarmonyPatch(typeof(HeroController), nameof(HeroController.instance.BindCompleted), MethodType.Getter)] // Invalid
internal class BindCompleted
{
    [HarmonyPostfix]
    static void WeaverSilkRefund(HeroController __instance)
    {
        CorpusCallosumPlugin.Log("WeaverSilkRefund");
        if (Globals.Weaver.IsEquipped)
        {
            __instance.AddSilk(4, true, SilkSpool.SilkAddSource.Normal);
        }
    }
}
[HarmonyPatch(typeof(HeroController), nameof(HeroController.instance.NailHitEnemy), MethodType.Getter)]
internal class NailHitEnemy
{
    [HarmonyPostfix]
    static void WeaverSilkGain(HeroController __instance)
    {
        CorpusCallosumPlugin.Log("WeaverSilkGain");
        if (Globals.Weaver.IsEquipped)
        {
            __instance.AddSilk(1, true, SilkSpool.SilkAddSource.Normal);
        }
    }
}
[HarmonyPatch(typeof(ToolItemManager), nameof(ToolItemManager.Awake))]
internal class ToolItemManagerAwake
{
    [HarmonyPostfix]
    internal void AddCrests()
    {
        CorpusCallosumPlugin.Log("New class needed");
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
                Globals.Weaver = actualCrest;
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
                CorpusCallosumPlugin.Log($"Bro, how did you mess this up, it's literally a single string\nAnyway, here's the name your dumbass entered into CorpusCallosumPlugin.crest: {name}");
            }
        }
    }
}