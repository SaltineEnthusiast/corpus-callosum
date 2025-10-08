using Corpus_Callosum;
using Corpus_Callosum.Crest;
using Corpus_Callosum.AnimHandler;
using Corpus_Callosum.Attacks;
using Corpus_Callosum.Attacks.Weaver;
using HutongGames.PlayMaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TeamCherry.Localization;
using UnityEngine;
using static ToolCrest;
using HarmonyLib;

namespace Corpus_Callosum.Crest
{
    public static class CrestManager
    {
        public static GameObject weaverRoot;


        public static WeaverCrest weaverCrest;
        public static HeroController.ConfigGroup weaverConfigGroup;

        public static GameObject CloneSlashObject(GameObject slashToClone, GameObject slashRoot)
        {
            GameObject clonedSlash = UnityEngine.Object.Instantiate(slashToClone);
            clonedSlash.transform.parent = slashRoot.transform;
            clonedSlash.transform.localPosition = new Vector3(0, 0, -0.001f);
            NailSlash slash = clonedSlash.GetComponent<NailSlash>();
            NailSlashRecoil nailSlashRecoil = clonedSlash.GetComponent<NailSlashRecoil>();
            if (nailSlashRecoil != null)
            {
                nailSlashRecoil.heroCtrl = HeroController.instance;
            }

            slash.hc = HeroController.instance;
            slash.enabled = true;
            return clonedSlash;
        }

        public static GameObject CloneDownslashObject(GameObject slashToClone, GameObject slashRoot)
        {
            GameObject clonedSlash = UnityEngine.Object.Instantiate(slashToClone);
            clonedSlash.transform.parent = slashRoot.transform;
            clonedSlash.transform.localPosition = new Vector3(0, 0, -0.001f);
            Downspike slash = clonedSlash.GetComponent<Downspike>();
            slash.heroCtrl = HeroController.instance;
            //Helper.SetPrivateField(slash, nameof(slash.hc), HeroController.instance);
            slash.enabled = true;
            return clonedSlash;
        }

        public static void AddWeaverRoot()
        {
            GameObject hornet = HeroController.instance.gameObject;
            GameObject attacks = hornet.Child("Attacks");

            weaverRoot = new GameObject();
            weaverRoot.name = "Weaver";
            weaverRoot.transform.parent = attacks.transform;
            weaverRoot.transform.localPosition = new Vector3();

        }


        public static void AddCrest()
        {
            AddWeaverRoot();


            ToolCrestList currentList = ToolItemManager.Instance.crestList;
            ToolCrest reaper = currentList.GetByName("Reaper");
            Sprite crestGlow = reaper.CrestGlow;
            Sprite crestSilhouette = reaper.CrestSilhouette;
            Sprite crestSprite = reaper.CrestSprite;
            HeroControllerConfig reaperConfig = reaper.HeroConfig;


            //double test

            HeroControllerConfig weaverConfig = UnityEngine.GameObject.Instantiate(reaperConfig);
            weaverConfig.name = "Weaver";
            weaverConfig.downSlashType = HeroControllerConfig.DownSlashTypes.Custom;
            weaverConfig.downSlashEvent = "WEAVERCREST DOWNSLASH";
            //Crest Attacks has a state Idle to add to
            //Reference reaper to make it :)
            weaverConfig.heroAnimOverrideLib = AnimManager.WeaverCrestAnimator.GetComponent<tk2dSpriteAnimator>().Library;

            /* BLANK SLATE SO THAT YOU CAN JUST DOUBLE CLICK OR Ctrl+F TO FILL IN THE VALUES INSTEAD OF LOOKING THROUGH THE OLD TEXT TO DELETE THE ONES
            cRESTNAME = new cRESTCLASS()
            {
                name = "cRESTNAME", //
                crestGlow = crestGlow,
                crestSilhouette = crestSilhouette,
                crestSprite = crestSprite,
                heroControllerConfig = cONFIGNAME, //
                isUnlocked = true,
                activeRootObject = cRESTROOT, //
                keySheet = "CORPUS",
                nameKey = "cRESTNAMEKEY", //
                descKey = "cRESTDESCKEY", //
            };
            // Idx Ref   :  x0  x1  x2  x3  x4  x5  x6
            // Up Idx    :  
            // Down Idx  :
            // Left Idx  :
            // Right Idx :

            cRESTNAME.RegisterSlot();
            cRESTNAME.RegisterSlot();
            cRESTNAME.RegisterSlot();
            cRESTNAME.RegisterSlot();
            */

            weaverCrest = new WeaverCrest()
            {
                name = "Weaver",
                crestGlow = crestGlow,
                crestSilhouette = crestSilhouette,
                crestSprite = crestSprite,
                heroControllerConfig = weaverConfig,
                isUnlocked = true,
                activeRootObject = weaverRoot,
                keySheet = "CORPUS",
                nameKey = "WEAVERCREST",
                descKey = "WEAVERCRESTDESC",
            };
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

            // Directionals
            weaverCrest.RegisterSlot(new Vector2(0, 0), ToolItemType.Skill, AttackToolBinding.Neutral, 2, 2, 1, 1, 0, 4, 0, 2, true); // s1
            weaverCrest.RegisterSlot(new Vector2(0, 400), ToolItemType.Skill, AttackToolBinding.Up, 4, 0, 1, 1, 0, 4, 0, 2, false); // s2
            weaverCrest.RegisterSlot(new Vector2(0, -400), ToolItemType.Skill, AttackToolBinding.Down, 0, 4, 1, 1, 0, 4, 0, 2, false); // s3
            // Blue
            weaverCrest.RegisterSlot(new Vector2(-200, 200), ToolItemType.Blue, AttackToolBinding.Neutral, 3, 1, 0, 2, 0, 4, 0, 2, false); // b1
            weaverCrest.RegisterSlot(new Vector2(200, -200), ToolItemType.Blue, AttackToolBinding.Neutral, 1, 3, 2, 0, 0, 4, 0, 2, true); // b2
            // Yellow
            weaverCrest.RegisterSlot(new Vector2(200, -200), ToolItemType.Yellow, AttackToolBinding.Neutral, 1, 3, 2, 0, 0, 4, 0, 2, false); // y1
            weaverCrest.RegisterSlot(new Vector2(-200, 200), ToolItemType.Yellow, AttackToolBinding.Neutral, 3, 1, 0, 2, 0, 4, 0, 2, true); // y2

            RegisterCrest(weaverCrest);

            weaverCrest.SetupWeapon();
        }

        public static void RegisterCrest(CustomCrest crest)
        {
            ToolCrest crestData = new ToolCrest();
            crestData.name = crest.name;
            crestData.crestSprite = crest.crestSprite;
            crestData.crestGlow = crest.crestGlow;
            crestData.crestSilhouette = crest.crestSilhouette;
            crestData.heroConfig = crest.heroControllerConfig;

            crestData.isHidden = false;


            LocalisedString namestring = new LocalisedString()
            {
                Key = crest.nameKey,
                Sheet = crest.keySheet
            };
            LocalisedString descstring = new LocalisedString()
            {
                Key = crest.descKey,
                Sheet = crest.keySheet
            };
            crestData.displayName = namestring;
            crestData.description = descstring;

            foreach (ToolCrest.SlotInfo slot in crest.slots)
            {
                crestData.slots.AddItem(slot);
            }

            crestData.SaveData = new ToolCrestsData.Data
            {
                IsUnlocked = crest.isUnlocked,
                Slots = new List<ToolCrestsData.SlotData>(),
                DisplayNewIndicator = true
            };

            //Add config group to the herocontrollers list
            HeroController.ConfigGroup[] configgroups = HeroController.instance.configs;

            crest.SetupConfigGroup();

            List<HeroController.ConfigGroup> asList = configgroups.ToList();
            weaverConfigGroup = crest.getConfigGroup();
            asList.Add(weaverConfigGroup);
            HeroController.instance.configs = asList.ToArray();


            //Adding the ToolCrest to the ToolCrestList in ToolItemManager
            ToolCrestList currentList = ToolItemManager.Instance.crestList;
            currentList.Add(crestData);
            ToolItemManager.Instance.crestList = currentList;

            //Finishing touches
            crest.crest = crestData;
            if (crestData.name == "Weaver") { Globals.Weaver = crestData; }
            Globals.Logger.Log("Weaver crest finished");
        }
    }
}