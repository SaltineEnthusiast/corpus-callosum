using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
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
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(HeroController), "Dash")]
    private static void DashPostfix(HeroController __instance) {
        if (__instance.warriorState.IsInRageMode)
        {
            __instance.dash_timer = 0f;
        }
    }
    
    /*
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ClassWithFunctionYouWant), "name of the function you want)]
    private static void PreferablyWhateverTheFunctionIsCalledAppendedByPostfix(ClassWithFunctionYouWant __instance, any other variables)
    {
        //__instance is how you refer to things within ClassWithFunctionYouWant
    }
    */
}