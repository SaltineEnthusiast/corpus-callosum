using Corpus_Callosum;
using Corpus_Callosum.Attacks;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SFCore.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Corpus_Callosum.Attacks.Weaver
{
    public class WeaverDownslash : BaseAttack
    {
        private GameObject downslashObject;

        public WeaverDownslash(GameObject downslashObject)
        {
            this.downslashObject = downslashObject;
        }

        public override void CreateAttack()
        {
            fsm = HeroController.instance.gameObject.LocateMyFSM("Crest Attacks");
            if (fsm == null) { return; }

            FsmOwnerDefault hornetOwnerDefault = Helper.GetHornetOwnerDefault();

            FsmEvent WEAVERDOWNSLASH = fsm.CreateFsmEvent("WEAVER DOWNSLASH");


            //Setup state

            FsmState WeaverDownslashSetupState = fsm.AddState("Weaver Downslash Setup");
            WeaverDownslashSetupState.AddMethod(() =>
            {

                HeroController.instance.RelinquishControlNotVelocity();
                HeroController.instance.StopAnimationControl();
                HeroController.instance.AffectedByGravity(false);
                HeroController.instance.gameObject.GetComponent<Rigidbody2D>().SetVelocity(0, 0);
                fsm.GetFsmBoolVariable("Disabled Animation").Value = true;
            });

            //Antic State (Hornet starting to spin)

            FsmState WeaverDownslashAnticState = fsm.AddState("Weaver Downslash Antic");
            WeaverDownslashAnticState.AddAnimationAction("Weaver", "Downslash Antic");
            WeaverDownslashAnticState.AddWatchAnimationAction("FINISHED");

            //Slash state

            FsmState WeaverDownslashState = fsm.AddState("Weaver Downslash");
            WeaverDownslashState.AddMethod(() =>
            {
                HeroController.instance.gameObject.GetComponent<Rigidbody2D>().SetVelocity(0, -35);
                HeroController.instance.AffectedByGravity(true);
                HeroController.instance.SetCState("downAttacking", true);
                downslashObject.GetComponent<NailSlash>().StartSlash();
            });
            WeaverDownslashState.AddAnimationAction("Weaver", "Downslash");
            WeaverDownslashState.AddWatchAnimationAction("ANIM END");

            //Slash end state

            FsmState WeaverDownslashEndState = fsm.AddState("Weaver Downslash End");
            WeaverDownslashEndState.AddMethod(() =>
            {
                HeroController.instance.gameObject.GetComponent<Rigidbody2D>().SetVelocity(0, 0);
                HeroController.instance.SetCState("downAttacking", false);
                HeroController.instance.FinishDownspike(true);
            });

            //Slash bounce state

            FsmState WeaverDownslashBounceState = fsm.AddState("Weaver Downslash Bounce");
            WeaverDownslashBounceState.AddMethod(() =>
            {
                HeroController.instance.gameObject.GetComponent<Rigidbody2D>().SetVelocity(0, 0);
                HeroController.instance.SetCState("downAttacking", false);
                HeroController.instance.SetStartWithDownSpikeBounce();
            });

            //Transitions

            FsmState idleState = fsm.GetState("Idle");
            idleState.AddTransition(WEAVERDOWNSLASH.name, WeaverDownslashSetupState.name);
            WeaverDownslashSetupState.AddTransition("FINISHED", WeaverDownslashAnticState.name);
            WeaverDownslashAnticState.AddTransition("FINISHED", WeaverDownslashState.name);
            WeaverDownslashState.AddTransition("ANIM END", WeaverDownslashEndState.name);

            WeaverDownslashState.AddTransition("ATTACK LANDED", WeaverDownslashBounceState.name);
            WeaverDownslashState.AddTransition("BOUNCE TINKED", WeaverDownslashBounceState.name);
            WeaverDownslashState.AddTransition("BOUNCE CANCEL", WeaverDownslashBounceState.name);
            WeaverDownslashState.AddTransition("LEAVING SCENE", WeaverDownslashEndState.name);
            WeaverDownslashEndState.AddTransition("FINISHED", "End");
            WeaverDownslashBounceState.AddTransition("FINISHED", "End");
        }
    }
}