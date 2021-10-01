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
        public static HediffDef InspirationHediff;
        public static ThingDef OmegaStimulant;
        public static ThingDef UkuphilaHerb;

        public static JobDef OmegaStimShot;
        public static JobDef MakeUkuphilaZombie;
        public static JobDef BreakTurret;
        public static JobDef InspireAllies;

        public static PawnKindDef Mercenary_Medic;
        public static PawnKindDef Tribal_Medic;
        public static PawnKindDef Mercenary_Technician;
        public static PawnKindDef Tribal_Drummer;

        public static MentalStateDef UkuphilaResurrectionPsychosis;

        public static AbilityDef InspiringDrumming;
        static AdvancedRaidersDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(AdvancedRaidersDefOf));
    }
}
