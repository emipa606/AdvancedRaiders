using RimWorld;
using Verse;
using Verse.AI;

namespace AdvancedRaiders;

public static class SpecialUnitAIUtility
{
    public static bool TryFindOmegaStimShotTarget(Pawn medic, float searchRadius, out Pawn targetPawn)
    {
        if (medic.Faction == null)
        {
            Log.Warning("Tried to search for omega stim shot targets for medic with no faction");
            targetPawn = null;
            return false;
        }

        var curMap = medic.Map;


        targetPawn = (Pawn)GenClosest.ClosestThingReachable(
            medic.Position,
            curMap,
            ThingRequest.ForGroup(ThingRequestGroup.Pawn),
            PathEndMode.OnCell,
            TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Some),
            searchRadius,
            Validator);

        return targetPawn != null;

        bool Validator(Thing t)
        {
            var pawn = t as Pawn;
            return pawn != null && pawn.RaceProps.Humanlike && pawn.Downed && pawn.Faction == medic.Faction &&
                   medic.CanReserve((LocalTargetInfo)pawn) &&
                   !pawn.health.hediffSet.HasHediff(AdvancedRaidersDefOf.OmegaStimulantHigh);
        }
    }

    public static bool TryFindUkuphilaHerbResurrectionTarget(Pawn medic, float searchRadius, out Corpse target)
    {
        if (medic.Faction == null)
        {
            Log.Warning("Tried to search for ukuphila resurrection targets for medic with no faction");
            target = null;
            return false;
        }

        var curMap = medic.Map;

        target = (Corpse)GenClosest.ClosestThingReachable(
            medic.Position,
            curMap,
            ThingRequest.ForGroup(ThingRequestGroup.Corpse),
            PathEndMode.OnCell,
            TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Some),
            searchRadius,
            Validator);

        return target != null;

        bool Validator(Thing t)
        {
            var corpse = t as Corpse;
            return corpse != null && corpse.InnerPawn.RaceProps.Humanlike &&
                   corpse.InnerPawn.health.hediffSet.HasHead && corpse.InnerPawn.Faction == medic.Faction &&
                   medic.CanReserve((LocalTargetInfo)corpse) &&
                   !corpse.InnerPawn.health.hediffSet.HasHediff(AdvancedRaidersDefOf.UkuphilaResurrection);
        }
    }


    public static bool TryFindBreakableEnemyTurret(Pawn technician, float searchRadius, out Building turret)
    {
        var curMap = technician.Map;


        turret = (Building)GenClosest.ClosestThingReachable(
            technician.Position,
            curMap,
            ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial),
            PathEndMode.ClosestTouch,
            TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Some),
            searchRadius,
            Validator);

        return turret != null;

        bool Validator(Thing t)
        {
            CompBreakdownable turretComp;
            if (t is Building building)
            {
                turretComp = building.TryGetComp<CompBreakdownable>();
            }
            else
            {
                return false;
            }

            return turretComp != null &&
                   building.def.building.IsTurret && //building.def.building.def.building.def.building....
                   !turretComp.BrokenDown && technician.Faction.HostileTo(building.Faction) &&
                   building.Faction != Faction.OfMechanoids && technician.CanReserve(building);
        }
    }

    public static bool TryFindPacificationTarget(Pawn pacifier, float searchRadius, out Pawn targetPawn)
    {
        targetPawn = (Pawn)GenClosest.ClosestThingReachable(
            pacifier.Position,
            pacifier.Map,
            ThingRequest.ForGroup(ThingRequestGroup.Pawn),
            PathEndMode.ClosestTouch,
            TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Some),
            searchRadius,
            Validator);

        return targetPawn != null;

        bool Validator(Thing t)
        {
            var pawn = t as Pawn;
            return pawn != null && pawn.Faction.HostileTo(pacifier.Faction) && pawn.RaceProps.Humanlike &&
                   pawn.Downed &&
                   pawn.health.capacities.GetLevel(PawnCapacityDefOf.Consciousness) >
                   0.1 && //beating someone who had already passed out isn't that much of a joy, you know
                   pacifier.CanReserve(pawn);
        }
    }

    public static bool TryFindTauntTarget(Pawn dozer, float searchRadius, out Pawn targetPawn)
    {
        targetPawn = (Pawn)GenClosest.ClosestThingReachable(
            dozer.Position,
            dozer.Map,
            ThingRequest.ForGroup(ThingRequestGroup.Pawn),
            PathEndMode.OnCell,
            TraverseParms.For(TraverseMode.NoPassClosedDoors),
            searchRadius,
            Validator);

        return targetPawn != null;

        bool Validator(Thing t)
        {
            var pawn = t as Pawn;
            return pawn != null && pawn.Faction.HostileTo(dozer.Faction) &&
                   pawn.health.State == PawnHealthState.Mobile &&
                   //!pawn.skills.GetSkill(SkillDefOf.Shooting).TotallyDisabled &&
                   //!pawn.skills.GetSkill(SkillDefOf.Melee).TotallyDisabled &&
                   pawn.RaceProps.Humanlike && pawn.MentalStateDef != AdvancedRaidersDefOf.MurderousRageTaunted &&
                   dozer.CanSee(pawn) && dozer.CanReserve(pawn);
        }
    }
}