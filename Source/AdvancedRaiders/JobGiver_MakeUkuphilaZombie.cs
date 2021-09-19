using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace AdvancedRaiders
{
    class JobGiver_MakeUkuphilaZombie : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            Corpse targetCorpse = null;
            if (MedicAIUtility.TryFindUkuphilaHerbResurrectionTarget(pawn, 20f, out targetCorpse))
            {
                var requiedThingDef = AdvancedRaidersDefOf.UkuphilaHerb;
                Thing thingToUse = null;
                foreach (var t in pawn.inventory.GetDirectlyHeldThings())
                {
                    if (t.def == requiedThingDef)
                    {
                        thingToUse = t;
                        break;
                    }
                }

                if (thingToUse != null)         //if medic actually has drug that can be used for this action
                {
                    Job job = JobMaker.MakeJob(AdvancedRaidersDefOf.MakeUkuphilaZombie);
                    job.targetA = targetCorpse;
                    job.targetB = thingToUse;
                    job.count = 1;

                    return job;
                }
                else
                {
                    targetCorpse = null;
                }
            }

            return null;
        }
    }
    
}
