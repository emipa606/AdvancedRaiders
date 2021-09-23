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
        private static JobGiver_MakeUkuphilaZombie makeUkuphilaZombie = new JobGiver_MakeUkuphilaZombie();
        private static JobGiver_OmegaStimShot omegaStimShot = new JobGiver_OmegaStimShot();
        private static JobGiver_BreakTurret breakTurret = new JobGiver_BreakTurret();
        public static ThinkNode_JobGiver GetPawnClassRelatedJobGiver(this Pawn pawn)
        {
            if (pawn.kindDef == AdvancedRaidersDefOf.Mercenary_Medic)
                return omegaStimShot;

            if (pawn.kindDef == AdvancedRaidersDefOf.Tribal_Medic)
                return makeUkuphilaZombie;

            if (pawn.kindDef == AdvancedRaidersDefOf.Mercenaty_Technician)
                return breakTurret;

            return null;
        }

        public static void TryKindSpecificJobGiversOnOwnedPawns(this Lord lord)
        {
            JobIssueParams pars;            //понять бы еще как правильно использовать JobGiver...
            pars.maxDistToSquadFlag = 999f;
            
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
