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
        public static bool TryFindFirstAidTarget(Pawn medic, float searchRadius, FirstAidAction action, out Pawn target)
        {
            if (action==FirstAidAction.None)
            {
                target = null;
                Log.Warning("Tried to find first aid target for None action");
                return false;
            }
            var curMap = medic.Map;

            var potentialTargets = 
                               from p in curMap.mapPawns.FreeHumanlikesSpawnedOfFaction(medic.Faction)
                               where p.Downed && !p.health.hediffSet.HasHediff(AdvancedRaidersDefOf.OmegaStimulantHigh)
                               select p;

            if (potentialTargets.Count()==0)
            {
                target = null;
                return false;
            }

            var closestTarget = potentialTargets.First();
            float minDistToTarget = 1e35f;
            foreach (var t in potentialTargets)
            {
                float dist = medic.Position.DistanceTo(t.Position);
                if (dist < minDistToTarget)
                {
                    minDistToTarget = dist;
                    closestTarget = t;
                }
            }

            if (minDistToTarget>searchRadius)
            {
                target = null;
                return false;
            }

            target = closestTarget;
            return true;

        }
    }
}
