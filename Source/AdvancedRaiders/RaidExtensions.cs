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
        public static ThinkNode_JobGiver GetPawnClassRelatedJobGiver(this Pawn pawn)
        {
            if (pawn.kindDef == AdvancedRaidersDefOf.Mercenary_Medic)
                return new JobGiver_OmegaStimShot();

            if (pawn.kindDef == AdvancedRaidersDefOf.Tribal_Medic)
                return new JobGiver_MakeUkuphilaZombie();

            return null;
        }

        public static void TryKindSpecificJobGiversOnOwnedPawns(this Lord lord)
        {
            JobIssueParams pars;            //понять бы еще как правильно использовать JobGiver...
            pars.maxDistToSquadFlag = 30f;
            
            foreach(var p in lord.ownedPawns)
            {
                var jobGiver = GetPawnClassRelatedJobGiver(p);
                
                if (jobGiver!=null)
                {
                    Job job = jobGiver.TryIssueJobPackage(p, pars).Job;       //забиваем микроскопом гвозди??
                    if (job != null && p.CurJobDef !=job.def)
                    {
                        job.count = 1;
                        p.jobs.StartJob(job, resumeCurJobAfterwards: true);
                    }
                }
            }
        }
    }
}
