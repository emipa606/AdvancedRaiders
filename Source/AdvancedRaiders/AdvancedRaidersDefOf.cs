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
        public static JobDef KillTaunter;
        public static JobDef ZhivayaResurrection;

        public static PawnKindDef Mercenary_MedicRanged;
        public static PawnKindDef Mercenary_MedicMelee;
        public static PawnKindDef Tribal_MedicRanged;
        public static PawnKindDef Tribal_MedicMelee;
        public static PawnKindDef Mercenary_Technician;
        public static PawnKindDef Tribal_ChiefCommanderRanged;
        public static PawnKindDef Tribal_ChiefCommanderMelee;
        public static PawnKindDef MercenaryPacifier_Bloodlust;
        public static PawnKindDef MercenaryPacifier_Psychopath;
        public static PawnKindDef Tribal_Beastmaster;
        public static PawnKindDef Mercenary_Bulldozer;

        public static MentalStateDef UkuphilaResurrectionPsychosis;
        public static MentalStateDef MurderousRageTaunted;

        public static AbilityDef InspireAlliesAbility;
        public static AbilityDef TauntAbility;

        public static ThoughtDef WitnessedPacification;
        public static ThoughtDef WitnessedPacificationBloodlust;
        static AdvancedRaidersDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(AdvancedRaidersDefOf));
    }
}
