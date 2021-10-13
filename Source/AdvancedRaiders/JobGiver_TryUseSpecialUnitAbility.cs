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

        protected Job MercenaryPacifierJob(Pawn pacifier)
        {
            Pawn victim;
            if (SpecialUnitAIUtility.TryFindPacificationTarget(pacifier, 30f, out victim))
            {
                Job job = JobMaker.MakeJob(AdvancedRaidersDefOf.PacifyDownedPawn);
                job.targetA = victim;
                job.count = 1;
                return job;
            }

            return null;
        }

        

        protected Job EvacMasterOrFleeJob(Pawn beast)
        {
            IntVec3 evacSpot;
            if (!RCellFinder.TryFindBestExitSpot(beast, out evacSpot))
                return null;

            Pawn master = beast.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond);
            if (!master.Downed)
                return null;

            Job job;
            if (master.CarriedBy != null || beast.def.defName == "Boomrat")
            {
                job = JobMaker.MakeJob(JobDefOf.Flee);
                job.targetA = evacSpot;
            }
            else
            {
                job = JobMaker.MakeJob(JobDefOf.Steal);
                job.targetA = master;
                job.targetB = evacSpot;
            }

            job.count = 1;
            return job;
        }
        
        protected Job MercenaryBulldozerJob(Pawn dozer)
        {
            if (dozer.CurJobDef == JobDefOf.CastAbilityOnThing)     
                return null;
            
            Pawn victim;
            var ability = dozer.abilities.GetAbility(AdvancedRaidersDefOf.TauntAbility);

            if (ability == null || !ability.CanCast || ability.Casting)
                return null;

            if (SpecialUnitAIUtility.TryFindTauntTarget(dozer,30,out victim))
            {
                Job job = JobMaker.MakeJob(JobDefOf.CastAbilityOnThing);
                job.targetA = victim;
                job.ability = ability;
                job.verbToUse = ability.verb;
                job.count = 1;
                return job;
            }

            return null;
        }

        protected override Job TryGiveJob(Pawn pawn)
        {
            //TODO make some kind of extention for PawnKindDef class. ie IsMedic or GetPawnClass()
            if (pawn.kindDef == AdvancedRaidersDefOf.Mercenary_Medic)
                return MercenaryMedicJob(pawn);

            if (pawn.kindDef == AdvancedRaidersDefOf.Tribal_Medic)
                return TribalMedicJob(pawn);

            if (pawn.kindDef == AdvancedRaidersDefOf.Mercenary_Technician)
                return MercenaryTechnicianJob(pawn);

            if (pawn.kindDef == AdvancedRaidersDefOf.Tribal_ChiefCommander)
                return InspirerJob(pawn);

            if (pawn.kindDef == AdvancedRaidersDefOf.MercenaryPacifier_Bloodlust || pawn.kindDef == AdvancedRaidersDefOf.MercenaryPacifier_Psychopath)
                return MercenaryPacifierJob(pawn);

            if (pawn.RaceProps.Animal)
                return EvacMasterOrFleeJob(pawn);

            if (pawn.kindDef == AdvancedRaidersDefOf.Mercenary_Bulldozer)
                return MercenaryBulldozerJob(pawn);

            return null;
        }
    }

    
    
}
