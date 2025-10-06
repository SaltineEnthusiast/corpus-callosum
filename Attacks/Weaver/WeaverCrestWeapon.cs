using Corpus_Callosum.Crest;
using Corpus_Callosum.Attacks.Weaver;
using Corpus_Callosum.AnimHandler;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using UnityEngine;

namespace Corpus_Callosum.Attacks.Weaver
{
    internal class WeaverCrestWeapon : WeaponBase
    {
        public WeaverCrestWeapon()
        {
            CreateWeaponObjects();

            weaponEvent = "WEAVERCREST";
            nailArt = new WeaverArt();
            specialDownSlash = new WeaverDownslash(objectInfo.slashDownObject);
            specialDashStab = new WeaverDashstab(objectInfo.slashdashstabObject);

            Initialise();

            configGroup = objectInfo.getConfigGroup();


        }

        private void CreateWeaponObjects()
        {
            objectInfo = new CrestObjectInfo();
            GameObject weaverRoot = CrestManager.weaverRoot;

            GameObject hornet = HeroController.instance.gameObject;
            GameObject attacks = hornet.Child("Attacks");
            GameObject hunter = attacks.Child("Default");
            GameObject reaper = attacks.Child("Scythe");
            GameObject witch = attacks.Child("Witch");
            GameObject beast = attacks.Child("Warrior");
            GameObject wanderer = attacks.Child("Wanderer");

            objectInfo.slashWallObject = CrestManager.CloneSlashObject(reaper.Child("WallSlash"), weaverRoot);

            //special case for our dash stab
            GameObject weaverdashstab = UnityEngine.Object.Instantiate(hunter.Child("Dash Stab"));
            weaverdashstab.transform.parent = weaverRoot.transform;
            weaverdashstab.transform.localPosition = new UnityEngine.Vector3(0, 0, -0.001f);
            DashStabNailAttack dashstab = weaverdashstab.GetComponent<DashStabNailAttack>();
            dashstab.scale.x = -1.4f;
            dashstab.heroController = HeroController.instance;
            dashstab.hc = HeroController.instance;
            dashstab.enabled = true;
            DamageEnemies damager = weaverdashstab.GetComponent<DamageEnemies>();
            damager.magnitudeMult = 2.25f;
            damager.nailDamageMultiplier = 1.5f;
            damager.doesNotTink = true;

            objectInfo.slashdashstabObject = weaverdashstab;


            //repackage sprites to fix artefact
            //Regular slash
            GameObject weaverslash = CrestManager.CloneSlashObject(hunter.Child("Slash"), weaverRoot);
            WeaverBaseSlash weaverSlash1 = new WeaverBaseSlash
            {
                SlashObject = weaverslash,
                SlashAnimatorObject = AnimManager.WeaverCrestAnimator,
                SlashPrefabName = "WeaverCrestSlash",
                AnimationName = "SlashEffect",
                LocalPosition = new UnityEngine.Vector3(-1.1f, 0.4f, -0.001f),
                LocalScale = new UnityEngine.Vector3(1.1f, 1f, 1)
            };
            weaverSlash1.SetupSlash();
            objectInfo.slashObject = weaverslash;

            //Altslash
            GameObject weaverslashalt = CrestManager.CloneSlashObject(hunter.Child("Slash"), weaverRoot);
            WeaverBaseSlash weaverSlash2 = new WeaverBaseSlash
            {
                SlashObject = weaverslashalt,
                SlashAnimatorObject = AnimManager.WeaverCrestAnimator,
                SlashPrefabName = "WeaverCrestSlashAlt",
                AnimationName = "SlashAltEffect",
                LocalPosition = new UnityEngine.Vector3(-0.6f, 0.4f, -0.001f),
                LocalScale = new UnityEngine.Vector3(1.1f, 0.85f, 1)
            };
            weaverSlash2.SetupSlash();
            objectInfo.slashAltObject = weaverslashalt;

            //Slashup
            GameObject weaverupslash = CrestManager.CloneSlashObject(hunter.Child("UpSlash"), weaverRoot);
            WeaverBaseSlash weaverSlash3 = new WeaverBaseSlash
            {
                SlashObject = weaverupslash,
                SlashAnimatorObject = AnimManager.WeaverCrestAnimator,
                SlashPrefabName = "WeaverCrestSlashUp",
                AnimationName = "SlashUpEffect",
                LocalPosition = new UnityEngine.Vector3(-1f, 1.1f, -0.001f),
                LocalScale = new UnityEngine.Vector3(1.1f, 1f, 1)
            };
            weaverSlash3.SetupSlash();
            objectInfo.slashUpObject = weaverupslash;

            //Downslash
            GameObject weaverdownslash = CrestManager.CloneSlashObject(wanderer.Child("DownSlash"), weaverRoot);
            WeaverBaseSlash weaverdownslash = new WeaverBaseSlash
            {
                SlashObject = weaverdownslash,
                SlashAnimatorObject = AnimManager.WeaverCrestAnimator,
                SlashPrefabName = "WeaverCrestDownslash",
                AnimationName = "DownslashEffect",
                LocalPosition = new UnityEngine.Vector3(),
                LocalScale = new UnityEngine.Vector3(1.1f, 1f, 1)
            };
            weaverdownslash.SetupSlash();
            objectInfo.slashDownObject = weaverdownslash;
        }
    }
}
