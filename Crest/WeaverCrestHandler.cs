using Steamworks;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Corpus_Callosum.Attacks;
using Corpus_Callosum.Attacks.Weaver;

namespace Corpus_Callosum.Crest
{
    public class WeaverCrestHandler : MonoBehaviour
    {
        private List<WeaponBase> allWeapons = new List<WeaponBase>();

        private List<WeaponBase> equippedWeapons = new List<WeaponBase>();
        private WeaponBase equippedWeapon;

        private bool airCharge = true; //used for reactor bounce

        public void Start()
        {
            CreateWeapons();
        }

        private void CreateWeapons()
        {
            WeaverCrestWeapon weaverCrest = new WeaverCrestWeapon();
            allWeapons.Add(weaverCrest);

            EquipWeapon(weaverCrest);
        }

        private void EquipWeapon(WeaponBase weapon)
        {
            HeroControllerConfig config = CrestManager.weaverCrest.heroControllerConfig;
            HeroController.ConfigGroup group = weapon.GetConfigGroup();
            group.Config = config;
            group.Setup();


            for (int i = 0; i < HeroController.instance.configs.Length; i++)
            {
                HeroController.ConfigGroup stored = HeroController.instance.configs[i];
                if (stored.Config.name != "Weaver") { continue; }
                HeroController.instance.configs[i] = group;
            }

            HeroController.instance.SetConfigGroup(group, group);

            CrestManager.weaverRoot.SetActive(true);

            equippedWeapon = weapon;
        }

        public WeaponBase getEquippedWeapon()
        {
            return equippedWeapon;
        }

        public bool isWeaponEquipped(WeaponBase weapon)
        {
            return equippedWeapon == weapon;
        }

        public void Update()
        {
            if (InputHandler.Instance.inputActions.QuickCast.IsPressed)
            {
                if (InputHandler.Instance.inputActions.Up.IsPressed)
                {
                    equippedWeapon.UpSpell();
                }
                if (InputHandler.Instance.inputActions.Down.IsPressed)
                {
                    equippedWeapon.DownSpell();
                }
                if (InputHandler.Instance.inputActions.Left.IsPressed || InputHandler.Instance.inputActions.Right.IsPressed)
                {
                    equippedWeapon.HorizontalSpell();
                }

                //hit the bigmoney
            }

            if (HeroController.instance == null)
            {
                return;
            }

            if ((HeroController.instance.cState.onGround || HeroController.instance.cState.touchingWall) && !airCharge)
            {
                airCharge = true;
            }
        }

        public bool UseAirCharge()
        {
            if (airCharge)
            {
                airCharge = false;
                return true;
            }
            return false;
        }

        public void HitLanded(HitInstance instance)
        {
            //
        }

        public void GotHit()
        {
            //
        }
    }
}
