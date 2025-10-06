using HutongGames.PlayMaker;
using SFCore.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using static Mono.Security.X509.X520;

namespace Corpus_Callosum.Attacks
{
    public abstract class BaseNailArt : BaseAttack
    {
        public string PreinitialState
        {
            get
            {
               return "Nail Art";
            }
        }

        public WeaponBase? weapon;

        public BaseNailArt()
        {
            PlayMakerFSM nailartFSM = HeroController.instance.gameObject.LocateMyFSM("Nail Arts");
            fsm = nailartFSM;

        }

        public void SetStateAsInit(string stateName)
        {
            FsmState PreInitialChoiceState = fsm.GetState(PreinitialState);
            if (PreInitialChoiceState == null)
            {
                return;
            }

            PreInitialChoiceState.AddTransition(weapon.GetWeaponEvent(), stateName);
        }


        public void SetWeapon(WeaponBase weapon)
        {
            this.weapon = weapon;
        }
    }
}