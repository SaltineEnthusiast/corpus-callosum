using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMaker;
using SFCore.Utils;
using UnityEngine;
//using CORPUS.Attacks.WeaverCrest;
using GlobalEnums;
//using CORPUS.Weapons;
//using CORPUS.Components;
using BepInEx;
using BepInEx.Logging;

namespace Corpus_Callosum.Crest
{
    public class WeaverCrest : CustomCrest
    {
        private bool wasSprinting = false;

        public void SetupWeapon()
        {
            PlayMakerFSM nailartFSM = HeroController.instance.gameObject.LocateMyFSM("Nail Arts");

            PlayMakerFSM crestAttackFSM = HeroController.instance.gameObject.LocateMyFSM("Crest Attacks");

            PlayMakerFSM sprintFSM = HeroController.instance.gameObject.LocateMyFSM("Sprint");

            ReviveNailArts(nailartFSM);

            HeroController.instance.gameObject.AddComponent<WeaverCrestHandler>();
        }

        private void ReviveNailArts(PlayMakerFSM nailartFSM)
        {
            FsmState TakeControlState = nailartFSM.GetState("Can Nail Art?");
            TakeControlState.AddMethod(() =>
            {
                wasSprinting = HeroController.instance.cState.isSprinting;
            });

            FsmEventTarget nailartTarget = new FsmEventTarget() { target = FsmEventTarget.EventTarget.FSMComponent, fsmComponent = nailartFSM };

            FsmState AnticTypeState = nailartFSM.GetState("Antic Type");
            AnticTypeState.AddMethod(() =>
            {
                if (HeroController.instance.playerData.CurrentCrestID == "Weaver")
                {
                    nailartFSM.SendEvent("WEAVER");
                }
            });

            FsmState SprintCheckState = nailartFSM.AddState("Sprinting?");
            SprintCheckState.AddMethod(() =>
            {
                if (wasSprinting || nailartFSM.GetFsmBoolIfExists("dashing") || GameManager.instance.inputHandler.inputActions.Dash.IsPressed)
                {
                    nailartFSM.SendEvent("DASHSLASH");
                }
            });


            FsmState CycloneCheckState = nailartFSM.AddState("Holding Up?");
            CycloneCheckState.AddMethod(() =>
            {
                if (GameManager.instance.inputHandler.inputActions.Up.IsPressed || GameManager.instance.inputHandler.inputActions.Down.IsPressed)
                {
                    nailartFSM.SendEvent("CYCLONESLASH");
                }
            });


            FsmState GreatSlashChoiceState = nailartFSM.AddState("Great Slash Choice");
            GreatSlashChoiceState.AddMethod(() =>
            {
                SendWeaponEvent(nailartFSM);
            });
            FsmState CycloneSlashChoiceState = nailartFSM.AddState("Cyclone Slash Choice");
            CycloneSlashChoiceState.AddMethod(() =>
            {
                SendWeaponEvent(nailartFSM);
            });
            FsmState DashSlashChoiceState = nailartFSM.AddState("Dash Slash Choice");
            DashSlashChoiceState.AddMethod(() =>
            {
                SendWeaponEvent(nailartFSM);
            });

            //transitions
            AnticTypeState.AddTransition("WEAVER", SprintCheckState.name);
            SprintCheckState.AddTransition("DASHSLASH", DashSlashChoiceState.name);
            SprintCheckState.AddTransition("FINISHED", CycloneCheckState.name);
            CycloneCheckState.AddTransition("CYCLONESLASH", CycloneSlashChoiceState.name);
            CycloneCheckState.AddTransition("FINISHED", GreatSlashChoiceState.name);
            GreatSlashChoiceState.AddTransition("FINISHED", "Antic");
            CycloneSlashChoiceState.AddTransition("FINISHED", "Antic");
            DashSlashChoiceState.AddTransition("FINISHED", "Antic");
        }

        private void SendWeaponEvent(PlayMakerFSM playmakerFSM)
        {
            if (HeroController.instance == null) { return; }
            WeaverCrestHandler handler = HeroController.instance.GetComponent<WeaverCrestHandler>();
            if (handler == null) { return; }
            playmakerFSM.SendEvent(handler.getEquippedWeapon().weaponEvent);
        }
    }
}
