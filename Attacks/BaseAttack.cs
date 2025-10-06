using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
//using SFCore.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corpus_Callosum.Attacks
{
    public abstract class BaseAttack
    {
        public PlayMakerFSM fsm;
        public string name;

        public abstract void CreateAttack();
    }
}