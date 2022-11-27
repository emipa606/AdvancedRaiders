using RimWorld;
using Verse;
using Verse.AI;

namespace AdvancedRaiders;

public class JobGiver_TryUseSpecialUnitAbility : ThinkNode_JobGiver
{
    protected Job MercenaryMedicJob(Pawn medic)
    {
        if (!SpecialUnitAIUtility.TryFindOmegaStimShotTarget(medic, 20f, out var patient))
        {
            return null;
        }

        var requiedThingDef = AdvancedRaidersDefOf.OmegaStimulant;
        Thing thingToUse = null;
        foreach (var t in medic.inventory.GetDirectlyHeldThings())
        {
            if (t.def != requiedThingDef)
            {
                continue;
            }

            thingToUse = t;
            break;
        }

        if (thingToUse == null) //if medic actually has drug that can be used for this action
        {
            return null;
        }

        var job = JobMaker.MakeJob(AdvancedRaidersDefOf.OmegaStimShot);
        job.targetA = patient;
        job.targetB = thingToUse;
        job.count = 1;
        return job;
    }

    protected Job TribalMedicJob(Pawn medic)
    {
        if (!SpecialUnitAIUtility.TryFindUkuphilaHerbResurrectionTarget(medic, 20f, out var targetCorpse))
        {
            return null;
        }

        var requiedThingDef = AdvancedRaidersDefOf.UkuphilaHerb;
        Thing thingToUse = null;
        foreach (var t in medic.inventory.GetDirectlyHeldThings())
        {
            if (t.def != requiedThingDef)
            {
                continue;
            }

            thingToUse = t;
            break;
        }

        if (thingToUse == null) //if medic actually has drug that can be used for this action
        {
            return null;
        }

        var job = JobMaker.MakeJob(AdvancedRaidersDefOf.MakeUkuphilaZombie);
        job.targetA = targetCorpse;
        job.targetB = thingToUse;
        job.count = 1;
        return job;
    }


    protected Job MercenaryTechnicianJob(Pawn technician)
    {
        if (!SpecialUnitAIUtility.TryFindBreakableEnemyTurret(technician, 30f, out var turretToBreak))
        {
            return null;
        }

        var job = JobMaker.MakeJob(AdvancedRaidersDefOf.BreakTurret);
        job.targetA = turretToBreak;
        job.count = 1;
        return job;
    }

    protected Job InspirerJob(Pawn inspirer)
    {
        var hediff = inspirer.health.hediffSet.GetFirstHediffOfDef(AdvancedRaidersDefOf.SoreThroat);
        if (hediff is { Visible: true })
        {
            return null;
        }

        var ability = inspirer.abilities.GetAbility(AdvancedRaidersDefOf.InspireAlliesAbility);
        if (ability is not { CanCast: true } || ability.Casting)
        {
            return null;
        }

        var job = JobMaker.MakeJob(JobDefOf
            .CastAbilityOnWorldTile); //not a world tile ability. me not smart to figure out how to make proper non-target cast ability job
        job.ability = ability;
        return job;
    }

    protected Job MercenaryPacifierJob(Pawn pacifier)
    {
        if (!ARSettings.allowPacification)
        {
            return null;
        }

        if (!SpecialUnitAIUtility.TryFindPacificationTarget(pacifier, 30f, out var victim))
        {
            return null;
        }

        var job = JobMaker.MakeJob(AdvancedRaidersDefOf.PacifyDownedPawn);
        job.targetA = victim;
        job.count = 1;
        return job;
    }

    protected Job EvacMasterOrFleeJob(Pawn beast)
    {
        if (!RCellFinder.TryFindBestExitSpot(beast, out var evacSpot))
        {
            return null;
        }

        var master = beast.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond);

        if (master == null || master.Spawned && !master.Downed)
        {
            return null;
        }

        Job job;
        if (!master.Spawned || master.CarriedBy != null || beast.def.defName == "Boomrat" || !beast.CanReserve(master))
        {
            job = JobMaker.MakeJob(JobDefOf.Flee);
            job.targetA = evacSpot;
            job.exitMapOnArrival = true;
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
        var canUseAbility = !SpecialUnitUtility.TooManyColonistsTaunted(dozer.Map) &&
                            dozer.CurJobDef != JobDefOf.CastAbilityOnThing;
        if (!canUseAbility)
        {
            return null;
        }

        var ability = dozer.abilities.GetAbility(AdvancedRaidersDefOf.TauntAbility);

        if (ability is not { CanCast: true } || ability.Casting)
        {
            return null;
        }

        if (!SpecialUnitAIUtility.TryFindTauntTarget(dozer, 30, out var victim))
        {
            return null;
        }

        var job = JobMaker.MakeJob(JobDefOf.CastAbilityOnThing);
        job.targetA = victim;
        job.ability = ability;
        job.verbToUse = ability.verb;
        job.count = 1;
        return job;
    }

    protected override Job TryGiveJob(Pawn pawn)
    {
        /*if (pawn.CurJobDef == JobDefOf.Wait_MaintainPosture)        //doesnt seem to be working
            return null;*/

        switch (SpecialUnitUtility.AdvancedRaiderClass(pawn))
        {
            case PawnClass.MercenaryMedic:
                return MercenaryMedicJob(pawn);

            case PawnClass.MercenaryBulldozer:
                return MercenaryBulldozerJob(pawn);

            case PawnClass.MercenaryPacifier:
                return MercenaryPacifierJob(pawn);

            case PawnClass.MercenaryTechnician:
                return MercenaryTechnicianJob(pawn);

            case PawnClass.TribalMedic:
                return TribalMedicJob(pawn);

            case PawnClass.TribalInspirer:
                return InspirerJob(pawn);

            case PawnClass.TribalBeastmaster:
                break; //those guys have no special abilities
        }

        return pawn.RaceProps.Animal ? EvacMasterOrFleeJob(pawn) : null;
    }
}