using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace AdvancedRaiders
{
    public class JobGiver_TryUseSpecialUnitAbility : ThinkNode_JobGiver
    { 
        protected Job MercenaryMedicJob(Pawn medic)
        {
            Pawn patient = null;

            if (SpecialUnitAIUtility.TryFindOmegaStimShotTarget(medic, 20f, out patient))
            {
                var requiedThingDef = AdvancedRaidersDefOf.OmegaStimulant;
                Thing thingToUse = null;
                foreach (var t in medic.inventory.GetDirectlyHeldThings())
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
            }

            return null;
        }

        protected Job TribalMedicJob(Pawn medic)
        {
            Corpse targetCorpse;
            if (SpecialUnitAIUtility.TryFindUkuphilaHerbResurrectionTarget(medic, 20f, out targetCorpse))
            {
                var requiedThingDef = AdvancedRaidersDefOf.UkuphilaHerb;
                Thing thingToUse = null;
                foreach (var t in medic.inventory.GetDirectlyHeldThings())
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
            }

            return null;
        }


        protected Job MercenaryTechnicianJob(Pawn technician)
        {
            Building turretToBreak;
            if (SpecialUnitAIUtility.TryFindBreakableEnemyTurret(technician, 30f, out turretToBreak))
            {
                Job job = JobMaker.MakeJob(AdvancedRaidersDefOf.BreakTurret);
                job.targetA = turretToBreak;
                job.count = 1;
                return job;
            }
            return null;
        }

        protected Job InspirerJob(Pawn inspirer)
        {
            Ability ability = inspirer.abilities.GetAbility(AdvancedRaidersDefOf.InspireAlliesAbility);
            if (ability == null || !ability.CanCast || ability.Casting)
                return null;

            Job job = JobMaker.MakeJob(JobDefOf.CastAbilityOnWorldTile);                        //not a world tile ability. me not smart to figure out how to make proper non-target cast ability job
            job.ability = ability;

            return job;
        }
        protected override Job TryGiveJob(Pawn pawn)
        {
            if (pawn.kindDef == AdvancedRaidersDefOf.Mercenary_Medic)
                return MercenaryMedicJob(pawn);

            if (pawn.kindDef == AdvancedRaidersDefOf.Tribal_Medic)
                return TribalMedicJob(pawn);

            if (pawn.kindDef == AdvancedRaidersDefOf.Mercenary_Technician)
                return MercenaryTechnicianJob(pawn);

            if (pawn.kindDef == AdvancedRaidersDefOf.Tribal_ChiefCommander)
                return InspirerJob(pawn);

            return null;
        }
    }

    
    
}
