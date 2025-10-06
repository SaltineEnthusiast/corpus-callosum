using Corpus_Callosum.Crest;
using SFCore.Utils;

namespace Corpus_Callosum.Attacks
{
    public abstract class BaseSpell : BaseAttack
    {
        public WeaverCrestHandler handler;
        public string EVENTNAME;
        public BaseSpell(WeaverCrestHandler handler)
        {
            if (HeroController.instance == null) { return; }
            PlayMakerFSM spellfsm = HeroController.instance.gameObject.LocateMyFSM("Silk Specials");
            fsm = spellfsm;


            this.handler = handler;
        }

        public void SetStateAsInit(string statename)
        {
            fsm.AddGlobalTransition(EVENTNAME, statename);
        }
    }
}
