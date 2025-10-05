using BepInEx;
using BepInEx.Logging;
using GlobalEnums;
using HarmonyLib;
using System;
using UnityEngine;

namespace Corpus_Callosum;
// Open Assembly-CSharp in Object Browser

// TODO - adjust the plugin guid as needed
[BepInAutoPlugin(id: "io.github.corpus_callosum")]
public partial class CorpusCallosumPlugin : BaseUnityPlugin
{
    private void Awake()
    {
        Logger.LogInfo($"Plugin {Name} ({Id}) has loaded!");
        
        Harmony.CreateAndPatchAll(typeof(CorpusCallosumPlugin));
        if (!Enum.IsDefined(typeof(CrestType), "Weaver"))
        {
            int NewValue = Enum.GetValues(typeof(CrestType)).Length;
            CrestType Weaver = (CrestType)NewValue;
            Logger.LogInfo($"{Name}({Id}) Weaver crest has loaded");
        }
        else
        {
            Logger.LogInfo($"{Name}({Id}) Weaver crest couldn't load");
        }
        Logger.LogInfo($"Crest list: {Enum.GetValues(typeof(CrestType))}");
    }

    /*
    [HarmonyPostfix]
    [HarmonyPatch(typeof(HeroController), "Dash")]
    private static void DashPostfix(HeroController __instance)
    {
        if (__instance.warriorState.IsInRageMode)
        {
            __instance.AddCurrency(10, CurrencyType.Money, true); // Adds 70 for some reason
        }
    }*/

    /*
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ClassWithFunctionYouWant), "name of the function you want)]
    private static void PreferablyWhateverTheFunctionIsCalledAppendedByPostfix(ClassWithFunctionYouWant __instance, any other variables)
    {
        //__instance is how you refer to things within ClassWithFunctionYouWant
    }
    */
}