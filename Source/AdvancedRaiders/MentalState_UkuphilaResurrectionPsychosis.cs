using RimWorld;
using Verse;
using Verse.AI;

namespace AdvancedRaiders;

public class MentalState_UkuphilaResurrectionPsychosis : MentalState
{
    public override bool AllowRestingInBed => false;

    //private int _findNewTargetCounter = 0;                      //zombie doesnt need to search for new target every 30 ticks
    public override bool ForceHostileTo(Thing t)
    {
        return t is Pawn p && p.RaceProps.Humanlike && ForceHostileTo(p.Faction);
    }

    public override bool ForceHostileTo(Faction f)
    {
        return pawn.Faction == null || pawn.Faction != f;
    }

    public override RandomSocialMode SocialModeMax()
    {
        return RandomSocialMode.Off;
    }
}