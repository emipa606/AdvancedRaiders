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
        public static HediffDef PacifierPTSD;
        public static HediffDef ElectricalSpasm;

        public static ThingDef OmegaStimulant;
        public static ThingDef UkuphilaHerb;
        public static ThingDef MeleeWeapon_ShockBaton;
        public static ThingDef Apparel_BlueScreenBelt;

        public static JobDef OmegaStimShot;
        public static JobDef MakeUkuphilaZombie;
        public static JobDef BreakTurret;
        public static JobDef InspireAllies;
        public static JobDef PacifyDownedPawn;

        public static PawnKindDef Mercenary_Medic;
        public static PawnKindDef Tribal_Medic;
        public static PawnKindDef Mercenary_Technician;
        public static PawnKindDef Tribal_ChiefCommander;
        public static PawnKindDef MercenaryPacifier_Bloodlust;
        public static PawnKindDef MercenaryPacifier_Psychopath;
        public static PawnKindDef Tribal_Beastmaster;

        public static MentalStateDef UkuphilaResurrectionPsychosis;

        public static AbilityDef InspireAlliesAbility;

        public static ThoughtDef WitnessedPacification;
        public static ThoughtDef WitnessedPacificationBloodlust;
        static AdvancedRaidersDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(AdvancedRaidersDefOf));
    }
}
