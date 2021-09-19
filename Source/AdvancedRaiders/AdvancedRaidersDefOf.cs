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
        public static HediffDef UkuphilaResurrection;

        public static ThingDef OmegaStimulant;
        public static ThingDef UkuphilaHerb;

        public static JobDef OmegaStimShot;
        public static JobDef MakeUkuphilaZombie;

        public static PawnKindDef Mercenary_Medic;
        public static PawnKindDef Tribal_Medic;
        static AdvancedRaidersDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(AdvancedRaidersDefOf));
    }
}
