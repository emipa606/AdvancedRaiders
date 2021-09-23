using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace AdvancedRaiders
{
    public class JobGiver_BreakTurret : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            Building turretToBreak;
            if (SpecialUnitAIUtility.TryFindBreakableEnemyTurret(pawn,25f, out turretToBreak))
            {
                Job job = JobMaker.MakeJob(AdvancedRaidersDefOf.BreakTurret);
                job.targetA = turretToBreak;
                job.count = 1;
                return job;
            }

            return null;
        }
    }
}
