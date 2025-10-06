using System;
using System.Collections.Generic;
using System.Text;
using Corpus_Callosum.Crest;

namespace Corpus_Callosum.Attacks
{
    public class WeaponBase
    {
        public string? name;
        public string? weaponEvent;
        public BaseNailArt nailArt;
        public BaseSpell? upSpell { get; set; }
        public BaseSpell? downSpell { get; set; }
        public BaseSpell? horizontalSpell { get; set; }

        public BaseAttack? specialDownSlash { get; set; }
        public BaseAttack? specialDashStab { get; set; }

        public CrestObjectInfo? objectInfo;
        internal HeroController.ConfigGroup configGroup;
        public WeaverCrestHandler handler;

        public WeaponBase()
        {
            if (HeroController.instance == null) { return; }
            handler = HeroController.instance.GetComponent<WeaverCrestHandler>();
        }

        public bool isEquipped()
        {
            return handler.isWeaponEquipped(this);
        }

        public void Initialise()
        {

            InitNailArt(nailArt);

            InitSpell(upSpell);
            InitSpell(downSpell);
            InitSpell(horizontalSpell);

            InitAttack(specialDownSlash);

        }

        private void InitSpell(BaseSpell? spell)
        {
            if (spell == null) { return; }
            spell.CreateAttack();
        }

        private void Spell(string EVENT)
        {
            if (HeroController.instance == null) { return; }
            PlayMakerFSM spellfsm = HeroController.instance.gameObject.LocateMyFSM("Silk Specials");
            spellfsm.SendEvent(EVENT);
        }

        public void UpSpell()
        {
            if (upSpell == null) { return; }
            Spell(upSpell.EVENTNAME);
        }

        public void HorizontalSpell()
        {
            if (horizontalSpell == null) { return; }
            ;
            Spell(horizontalSpell.EVENTNAME);
        }

        public void DownSpell()
        {
            if (downSpell == null) { return; }
            Spell(downSpell.EVENTNAME);
        }

        public HeroController.ConfigGroup GetConfigGroup()
        {
            return configGroup;
        }

        private void InitNailArt(BaseNailArt? nailArt)
        {
            if (nailArt == null) { return; }
            nailArt.SetWeapon(this);
            nailArt.CreateAttack();
        }

        private void InitAttack(BaseAttack? attack)
        {
            if (attack == null) { return; }
            attack.CreateAttack();
        }

        public string GetWeaponEvent()
        {
            if (weaponEvent == null)
            {
                return "";
            }

            return weaponEvent;
        }
    }
}
