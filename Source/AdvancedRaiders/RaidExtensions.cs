using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI.Group;
using Verse.AI;
using Verse;
using RimWorld;

namespace AdvancedRaiders
{
    public static class RaidExtensions
    {
        public static void TryKindSpecificJobGiversOnOwnedPawns(this Lord lord)
        {
            JobGiver_FirstAid medicJobGiver = new JobGiver_FirstAid();
            JobIssueParams pars;
            pars.maxDistToSquadFlag = 30f;
            foreach(var p in lord.ownedPawns)
            {
                if (p.IsMedic() && p.jobs.curJob.def != AdvancedRaidersDefOf.FirstAid)
                {
                    Job job = medicJobGiver.TryIssueJobPackage(p, pars).Job;
                    if (job != null)
                    {
                        job.count = 1;
                        p.jobs.StartJob(job, resumeCurJobAfterwards: true);
                    }
                }
            }
        }
    }
}
