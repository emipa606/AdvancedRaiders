using RimWorld;
using System;

using Verse;
using Verse.AI;

namespace AdvancedRaiders
{
    public class MentalState_UkuphilaResurrectionPsychosis : MentalState
    {
        //private int _findNewTargetCounter = 0;                      //zombie doesnt need to search for new target every 30 ticks
        public override bool ForceHostileTo(Thing t) => (t is Pawn p) && p.RaceProps.Humanlike && this.ForceHostileTo(p.Faction);
        public override bool ForceHostileTo(Faction f) => pawn.Faction == null ? true : pawn.Faction != f;
        public override RandomSocialMode SocialModeMax() => RandomSocialMode.Off;
        public override bool AllowRestingInBed => false;
    }
}
