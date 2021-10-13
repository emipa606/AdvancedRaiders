using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using Verse.AI;

namespace AdvancedRaiders
{
    public class MentalState_MurderousRageTaunted : MentalState
    {
        public Pawn target;

        public override void MentalStateTick()
        {
            if (!this.pawn.IsHashIntervalTick(120))
                return;

            if (target == null || target.Dead)
            {
                RecoverFromState();
                return;
            }

            if (pawn.CurJobDef != AdvancedRaidersDefOf.KillTaunter)
            {
                Job job = JobMaker.MakeJob(AdvancedRaidersDefOf.KillTaunter);
                job.targetA = target;
                pawn.jobs.StartJob(job);
            }
        }
    }

    public class JobDriver_KillWithoutReservations : JobDriver_Kill
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }
    }
}
