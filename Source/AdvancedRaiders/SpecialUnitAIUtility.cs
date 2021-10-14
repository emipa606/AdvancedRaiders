using System;
using System.Linq;
using Verse;
using RimWorld;
using Verse.AI;

namespace AdvancedRaiders
{
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

            Predicate<Thing> validator = (Predicate<Thing>)(t =>        
            {
                Pawn pawn = t as Pawn;
                return 
                pawn!=null &&
                pawn.RaceProps.Humanlike && 
                pawn.Downed && 
                pawn.Faction == medic.Faction && 
                (medic.CanReserve((LocalTargetInfo)(Thing)pawn) && 
                !pawn.health.hediffSet.HasHediff(AdvancedRaidersDefOf.OmegaStimulantHigh));
            });


            targetPawn = (Pawn) GenClosest.ClosestThingReachable(
                medic.Position, 
                curMap, 
                ThingRequest.ForGroup(ThingRequestGroup.Pawn), 
                PathEndMode.OnCell, 
                TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Some), 
                searchRadius, 
                validator);

            return targetPawn != null;
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

            Predicate<Thing> validator = (Predicate<Thing>)(t =>
            {
                Corpse corpse = t as Corpse;
                return 
                corpse!=null &&
                corpse.InnerPawn.RaceProps.Humanlike &&
                corpse.InnerPawn.health.hediffSet.HasHead &&
                corpse.InnerPawn.Faction == medic.Faction &&
                (medic.CanReserve((LocalTargetInfo)(Thing)corpse) &&
                !corpse.InnerPawn.health.hediffSet.HasHediff(AdvancedRaidersDefOf.UkuphilaResurrection));
            });

            target = (Corpse) GenClosest.ClosestThingReachable(
                medic.Position,
                curMap,
                ThingRequest.ForGroup(ThingRequestGroup.Corpse),
                PathEndMode.OnCell,
                TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Some),
                maxDistance: searchRadius,
                validator);

            return target != null;
        }


        public static bool TryFindBreakableEnemyTurret(Pawn technician, float searchRadius, out Building turret)
        {
            var curMap = technician.Map;

            Predicate<Thing> validator = (Predicate<Thing>)(t =>
            {
                var building = t as Building;
                CompBreakdownable turretComp;
                if (building != null)
                    turretComp = building.TryGetComp<CompBreakdownable>();
                else
                    return false;

                return
                turretComp!=null &&
                building.def.building.IsTurret &&           //building.def.building.def.building.def.building....
                !turretComp.BrokenDown &&
                technician.Faction.HostileTo(building.Faction) &&
                building.Faction!=Faction.OfMechanoids &&   
                technician.CanReserve(building);
            });

                   
            turret = (Building) GenClosest.ClosestThingReachable(
                technician.Position,
                curMap,
                ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial),        
                PathEndMode.ClosestTouch,
                TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Some),
                maxDistance: searchRadius,
                validator);

            return turret != null;
        }

        public static bool TryFindPacificationTarget(Pawn pacifier, float searchRadius, out Pawn targetPawn)
        {
            Predicate<Thing> validator = (t =>
            {
                var pawn = t as Pawn;
                return
                pawn != null &&
                pawn.Faction.HostileTo(pacifier.Faction) &&
                pawn.RaceProps.Humanlike && 
                pawn.Downed &&
                pawn.health.capacities.GetLevel(PawnCapacityDefOf.Consciousness) > 0.1 &&      //beating someone who had already passed out isnt that much of a joy, you know
                pacifier.CanReserve(pawn);
            });

            targetPawn = (Pawn)GenClosest.ClosestThingReachable(
                pacifier.Position,
                pacifier.Map,
                ThingRequest.ForGroup(ThingRequestGroup.Pawn),
                PathEndMode.ClosestTouch,
                TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Some),
                searchRadius,
                validator);

            return targetPawn != null;
        }

        public static bool TryFindTauntTarget(Pawn dozer, float searchRadius, out Pawn targetPawn)
        {
            Predicate<Thing> validator = (t =>
            {
                var pawn = t as Pawn;
                return
                pawn != null &&
                pawn.Faction.HostileTo(dozer.Faction) &&
                pawn.health.State == PawnHealthState.Mobile &&
                //!pawn.skills.GetSkill(SkillDefOf.Shooting).TotallyDisabled &&
                //!pawn.skills.GetSkill(SkillDefOf.Melee).TotallyDisabled &&
                pawn.RaceProps.Humanlike &&
                pawn.MentalStateDef != AdvancedRaidersDefOf.MurderousRageTaunted &&
                dozer.CanSee(pawn) &&
                dozer.CanReserve(pawn);
            });

            targetPawn = (Pawn)GenClosest.ClosestThingReachable(
                dozer.Position,
                dozer.Map,
                ThingRequest.ForGroup(ThingRequestGroup.Pawn),
                PathEndMode.OnCell,
                TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly),
                searchRadius,
                validator);

            return targetPawn != null;
        }

        public static bool AtLeastNAlliesInInspireRadius(int nPawns, Pawn caster)
        {
            Ability ability = caster.abilities.GetAbility(AdvancedRaidersDefOf.InspireAlliesAbility);
            if (ability == null)
                return false;

            float radius = ((CompAbilityEffect_InspireAllies)ability.comps.Find((AbilityComp c) => c is CompAbilityEffect_InspireAllies)).Props.radius;
            var pawnsInRadius =
                from p in caster.Map.mapPawns.FreeColonistsSpawned
                where p.Position.DistanceTo(caster.Position) < radius &&
                p.health.State == PawnHealthState.Mobile &&
                p.Faction == caster.Faction
                select p;

            return pawnsInRadius.Count()-1 >= nPawns;       //-1 for caster themself
        }
    }
}