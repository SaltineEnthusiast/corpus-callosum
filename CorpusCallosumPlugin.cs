using BepInEx;
using BepInEx.Logging;
using GlobalEnums;
using HarmonyLib;
using System;
using UnityEngine;
using Needleforge;
using Needleforge.Makers;

namespace Corpus_Callosum;
// Open Assembly-CSharp in Object Browser

// TODO - adjust the plugin guid as needed
[BepInAutoPlugin(id: "io.github.corpus_callosum")]
public partial class CorpusCallosumPlugin : BaseUnityPlugin
{
    public static string[] crests = ["Weaver"];
    static ManualLogSource logger;
    private void Awake()
    {
        logger = Logger;
        logger.LogInfo($"Plugin {Name} ({Id}) has loaded!");

        Harmony.CreateAndPatchAll(typeof(CorpusCallosumPlugin));
        foreach (CrestType crest in Enum.GetValues(typeof(CrestType)))
        {
            Log($"Global Enum: {crest}");
        }
    }
        #region Slot Info Explainations

        /*
        public Vector2 Position; // Position of slot

        public ToolItemType Type; // Color of slot or if it's a silk skill

        public AttackToolBinding AttackBinding; // Which direction you hold to use it (I believe only applies to Red and Silk Skill

        public int NavUpIndex; // Position in a number line that gets moved across as you move up in the ui of the crest

        public int NavDownIndex; // See above but for a downwards line

        public int NavLeftIndex; // See above but for a left facing line

        public int NavRightIndex; // See above but for a right facing line

        public int NavUpFallbackIndex; // What position on the up number line it would go to if it couldn't go up by one (this is a complete guess, also I think it's only important to apply this to the highest slots)

        public int NavDownFallbackIndex; // See above but for the down line

        public int NavLeftFallbackIndex; // You get the gist

        public int NavRightFallbackIndex;

        public bool IsLocked; // If you need to spend a locket to unlock it
        */
        #endregion
    public static ToolCrest.SlotInfo RegisterSlot(Vector2 pos, ToolItemType type, AttackToolBinding attackBinding, int upIdx, int downIdx, int leftIdx, int rightIdx, int upFallbackIdx, int downFallbackIdx, int leftFallbackIdx, int rightFallbackIdx, bool locked)
    {
        var slotData = new ToolCrest.SlotInfo();
        slotData.Position = pos;
        slotData.Type = type;
        slotData.AttackBinding = attackBinding;
        slotData.NavUpIndex = upIdx;
        slotData.NavDownIndex = downIdx;
        slotData.NavLeftIndex = leftIdx;
        slotData.NavRightIndex = rightIdx;
        slotData.NavUpFallbackIndex = upFallbackIdx;
        slotData.NavDownFallbackIndex = downFallbackIdx;
        slotData.NavLeftFallbackIndex = leftFallbackIdx;
        slotData.NavRightFallbackIndex = rightFallbackIdx;
        slotData.IsLocked = locked;
        return slotData;
    }

    public static void Log(string text)
    {
        logger.LogInfo($"{text}");
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