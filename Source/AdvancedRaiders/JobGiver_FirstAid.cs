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
    public enum FirstAidAction
    {
        None,
        Stabilize,
        Evac,
        Painkiller,
        OmegaStimulantShot
    }

    public class JobGiver_FirstAid : ThinkNode_JobGiver
    {
        

        private IEnumerable<FirstAidAction> PossibleFirstAidActions(Pawn medic)
        {
            //WIP
            var inventory = medic.inventory;

            if (inventory.Count(AdvancedRaidersDefOf.OmegaStimulant) > 0)
            {
                yield return FirstAidAction.OmegaStimulantShot;
            }    
        }
        

        protected override Job TryGiveJob(Pawn pawn)
        {
            List<FirstAidAction> possibleActions = PossibleFirstAidActions(pawn).ToList();
            if (possibleActions.Count()==0)
                return null;

            Pawn patient = null;
            //a bit of spaggeti here
            foreach (var action in possibleActions)
            {
                if (MedicAIUtility.TryFindFirstAidTarget(pawn, 15f, action, out patient))
                {
                    var requiedThingDef = MedicUtility.ActionRelatedThingDef(action);
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
                        Job job = JobMaker.MakeJob(AdvancedRaidersDefOf.FirstAid);
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
            }
            
            return null;
        }


    }
}
