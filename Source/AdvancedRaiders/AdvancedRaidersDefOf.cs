using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace AdvancedRaiders
{
    [DefOf]
    public static class AdvancedRaidersDefOf
    {
        public static HediffDef OmegaStimulantHigh;
        public static ThingDef OmegaStimulant;
        public static JobDef FirstAid;
        static AdvancedRaidersDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(AdvancedRaidersDefOf));
    }
}
