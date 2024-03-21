using RimWorld;
using Verse;

namespace AdvancedRaiders;

public class ThoughtWorker_PacifierPTSD : ThoughtWorker
{
    protected override ThoughtState CurrentStateInternal(Pawn p)
    {
        var hediff = p.health.hediffSet.GetFirstHediffOfDef(AdvancedRaidersDefOf.PacifierPTSD);
        if (hediff == null)
        {
            return ThoughtState.Inactive;
        }

        switch (hediff.Severity)
        {
            case > 0.9f:
                return ThoughtState.ActiveAtStage(3);
            case > 0.6f:
                return ThoughtState.ActiveAtStage(2);
            case > 0.3f:
                return ThoughtState.ActiveAtStage(1);
            default:
                return ThoughtState.ActiveAtStage(0);
        }
    }

    public override float MoodMultiplier(Pawn p)
    {
        return ARSettings.ptsdMultipiler;
    }
}