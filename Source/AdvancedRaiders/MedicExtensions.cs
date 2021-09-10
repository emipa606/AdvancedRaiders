using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace AdvancedRaiders
{
    public static class MedicExtensions
    {
        public static bool WouldBeMobileAfterOmegaStimInjection(this Pawn pawn)
        {
            if (pawn.health.State == PawnHealthState.Mobile)
                return true;
            if (pawn.Dead)
                return false;
            
            return !(pawn.health.WouldBeDownedAfterAddingHediff(AdvancedRaiders.HediffDefOf.OmegaStimulant, null, 0.01f));
            
        }

        public static bool HasAnyOf(this Pawn pawn, ThingDef def) => pawn.GetDirectlyHeldThings().Contains(def);
    }
}
