using Verse;
using Verse.AI;

namespace AdvancedRaiders;

public class MentalState_MurderousRageTaunted : MentalState
{
    public Pawn target;

    public override void MentalStateTick(int delta)
    {
        base.MentalStateTick(delta);
        if (!pawn.IsHashIntervalTick(120, delta))
        {
            return;
        }

        if (target == null || target.Dead)
        {
            RecoverFromState();
            return;
        }

        if (pawn.CurJobDef == AdvancedRaidersDefOf.KillTaunter)
        {
            return;
        }

        var job = JobMaker.MakeJob(AdvancedRaidersDefOf.KillTaunter);
        job.targetA = target;
        pawn.jobs.StartJob(job);
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_References.Look(ref target, "target");
    }
}