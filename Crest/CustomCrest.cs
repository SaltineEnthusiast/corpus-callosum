using HarmonyLib;
using System;
using UnityEngine;

namespace Corpus_Callosum.Crest
{
    public abstract class CustomCrest
    {
        public string name;
        public ToolCrest crest;

        public Sprite crestGlow;
        public Sprite crestSilhouette;
        public Sprite crestSprite;

        public HeroControllerConfig heroControllerConfig;

        public bool isUnlocked;

        private HeroController.ConfigGroup configGroup;

        public GameObject activeRootObject;
        public GameObject slashObject;
        public GameObject slashAltObject;
        public GameObject slashUpObject;
        public GameObject slashDownObject;
        public GameObject slashWallObject;
        public GameObject slashDashObject;

        public HeroController.ConfigGroup getConfigGroup() { return configGroup;}

        public string nameKey;
        public string keySheet;
        public string descKey;

        public object[] slots = [];

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

        public void RegisterSlot(Vector2 pos, ToolItemType type, AttackToolBinding attackBinding, int upIdx, int downIdx, int leftIdx, int rightIdx, int upFallbackIdx, int downFallbackIdx, int leftFallbackIdx, int rightFallbackIdx, bool locked)
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
            slots.AddItem(slotData);
        }



        public void SetupConfigGroup()
        {
            //Not all slashes are accounted for.
            //Mostly the extra alt slashes.
            configGroup = new HeroController.ConfigGroup
            {
                Config = heroControllerConfig,
                ActiveRoot = activeRootObject,
                NormalSlashObject = slashObject,
                AlternateSlashObject = slashAltObject,
                UpSlashObject = slashUpObject,
                DownSlashObject = slashDownObject,
                DashStab = slashDashObject,
                WallSlashObject = slashWallObject
            };
            if (configGroup == null) { return; }
            configGroup.Setup();
        }
    }
}
