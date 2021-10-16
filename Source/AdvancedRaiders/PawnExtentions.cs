
using Verse;

namespace AdvancedRaiders
{
    public enum PawnClass
    {
        None,

        MercenaryMedic,
        MercenaryTechnician,
        MercenaryBulldozer,
        MercenaryPacifier,
        TribalMedic,
        TribalInspirer,
        TribalBeastmaster
    }

    public static class PawnExtentions
    {
        public static PawnClass AdvancedRaiderClass(this Pawn pawn)
        {
            PawnKindDef def = pawn.kindDef;

            if (def == AdvancedRaidersDefOf.Mercenary_MedicRanged || def == AdvancedRaidersDefOf.Mercenary_MedicMelee)
                return PawnClass.MercenaryMedic;

            if (def == AdvancedRaidersDefOf.Mercenary_Technician)
                return PawnClass.MercenaryTechnician;

            if (def == AdvancedRaidersDefOf.Mercenary_Bulldozer)
                return PawnClass.MercenaryBulldozer;

            if (def == AdvancedRaidersDefOf.MercenaryPacifier_Bloodlust || def == AdvancedRaidersDefOf.MercenaryPacifier_Psychopath)
                return PawnClass.MercenaryPacifier;

            if (def == AdvancedRaidersDefOf.Tribal_MedicRanged || def == AdvancedRaidersDefOf.Tribal_MedicMelee)
                return PawnClass.TribalMedic;

            if (def == AdvancedRaidersDefOf.Tribal_Beastmaster)
                return PawnClass.TribalBeastmaster;

            if (def == AdvancedRaidersDefOf.Tribal_ChiefCommanderRanged || def == AdvancedRaidersDefOf.Tribal_ChiefCommanderMelee)
                return PawnClass.TribalInspirer;

            return PawnClass.None;
        }
    }
}
