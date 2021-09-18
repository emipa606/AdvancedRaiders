using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using Verse.AI;

namespace AdvancedRaiders
{
    public static class MedicAIUtility
    {
        

        public static bool TryFindOmegaStimShotTarget(Pawn medic, float searchRadius, out Pawn target)
        {
            if (medic.Faction == null)
            {
                Log.Warning("Tried to search for omega stim shot targets for medic with no faction");
                target = null;
                return false;
            }
            var curMap = medic.Map;

            Predicate<Thing> validator = (Predicate<Thing>)(t =>
            {
                Pawn pawn = t as Pawn;
                return pawn.RaceProps.Humanlike && 
                pawn.Downed && 
                pawn.Faction == medic.Faction && 
                (medic.CanReserve((LocalTargetInfo)(Thing)pawn) && 
                !pawn.health.hediffSet.HasHediff(AdvancedRaidersDefOf.OmegaStimulantHigh));
            });


            target = (Pawn) GenClosest.ClosestThingReachable(medic.Position, curMap, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Some), searchRadius, validator);

            if (target == null)
            {
                return false;
            }

            return true;
        }

        
    }
}
