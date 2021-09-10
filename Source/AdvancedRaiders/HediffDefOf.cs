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
    public static class HediffDefOf
    {
        public static HediffDef OmegaStimulant;

        static HediffDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(HediffDefOf));
    }
}
