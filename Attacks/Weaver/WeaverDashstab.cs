using Corpus_Callosum.Attacks;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SFCore.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static ToolGameObjectActivator;

namespace Corpus_Callosum.Attacks.Weaver
{
    public class WeaverDashstab : BaseAttack
    {
        private GameObject dashstabObject;

        public WeaverDashstab(GameObject downslashObject)
        {
            this.dashstabObject = downslashObject;
        }
        public override void CreateAttack()
        {
            PlayMakerFSM sprintfsm = HeroController.instance.gameObject.LocateMyFSM("Sprint");
            if (sprintfsm == null) { return; }

            FsmEvent WEAVEREVENT = sprintfsm.CreateFsmEvent("WEAVER");

            FsmState startAttackState = sprintfsm.GetState("Start Attack");
            startAttackState.AddMethod(() =>
            {
                if (HeroController.instance.playerData.CurrentCrestID == "Weaver")
                {
                    sprintfsm.SendEvent("WEAVER");
                }
            });

            FsmState testSetState = sprintfsm.AddState("Set Weaver");
            testSetState = sprintfsm.CopyState("Set Attack Single", "Set Weaver");
            SetGameObject action = (SetGameObject)testSetState.Actions[3];
            action.gameObject = dashstabObject;




            sprintfsm.AddTransition("Start Attack", "WEAVER", "Set Weaver");
        }
    }
}