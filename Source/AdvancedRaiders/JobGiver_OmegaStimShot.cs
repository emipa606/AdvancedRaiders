using Verse;
using Verse.AI;

namespace AdvancedRaiders
{
    public enum FirstAidAction
    {
        None,
        Stabilize,
        Evac,
        Painkiller,
        OmegaStimulantShot
    }

    public class JobGiver_OmegaStimShot : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            Pawn patient = null;
            
            
            
            if (MedicAIUtility.TryFindOmegaStimShotTarget(pawn, 20f, out patient))
            {
                var requiedThingDef = AdvancedRaidersDefOf.OmegaStimulant;
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
                    Job job = JobMaker.MakeJob(AdvancedRaidersDefOf.OmegaStimShot);
                    job.targetA = patient;
                    job.targetB = thingToUse;
                    job.count = 1;
                    
                    return job;
                }
                else
                {
                    patient = null;
                }
            }
            
            return null;
        }


    }
}
