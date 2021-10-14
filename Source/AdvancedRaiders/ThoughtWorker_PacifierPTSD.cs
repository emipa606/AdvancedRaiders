using RimWorld;
using Verse;

namespace AdvancedRaiders
{
    public class ThoughtWorker_PacifierPTSD : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            var hediff = p.health.hediffSet.GetFirstHediffOfDef(AdvancedRaidersDefOf.PacifierPTSD);
            if (hediff == null)
                return ThoughtState.Inactive;

            //TODO rewrite this YandereDev-tier piece
            if (hediff.Severity > 0.9f)
                return ThoughtState.ActiveAtStage(3);

            if (hediff.Severity > 0.6f)
                return ThoughtState.ActiveAtStage(2);

            if (hediff.Severity > 0.3f)
                return ThoughtState.ActiveAtStage(1);

            return ThoughtState.ActiveAtStage(0);
        }
    }
}
