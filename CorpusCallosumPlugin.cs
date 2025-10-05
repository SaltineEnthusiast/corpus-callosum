using BepInEx;
using HarmonyLib;

namespace Corpus_Callosum;

// TODO - adjust the plugin guid as needed
[BepInAutoPlugin(id: "io.github.corpus_callosum")]
public partial class CorpusCallosumPlugin : BaseUnityPlugin
{
    private void Awake()
    {
        // Put your initialization logic here
        Logger.LogInfo($"Plugin {Name} ({Id}) has loaded!");
        
        Harmony.CreateAndPatchAll(typeof(CorpusCallosumPlugin));
    }
}