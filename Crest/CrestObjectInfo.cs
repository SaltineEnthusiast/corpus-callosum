using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Corpus_Callosum.Crest
{
    public class CrestObjectInfo
    {
        public HeroControllerConfig heroControllerConfig;

        public GameObject? activeRootObject;
        public GameObject? slashObject;
        public GameObject? slashAltObject;
        public GameObject? slashUpObject;
        public GameObject? slashDownObject;
        public GameObject? slashWallObject;
        public GameObject? slashdashstabObject;


        public HeroController.ConfigGroup getConfigGroup()
        {
            return new HeroController.ConfigGroup()
            {
                ActiveRoot = activeRootObject,
                NormalSlashObject = slashObject,
                AlternateSlashObject = slashAltObject,
                DownSlashObject = slashDownObject,
                DashStab = slashdashstabObject,
                UpSlashObject = slashUpObject,
                WallSlashObject = slashWallObject
            };
        }
    }
}
