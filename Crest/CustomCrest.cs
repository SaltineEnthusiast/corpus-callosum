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
