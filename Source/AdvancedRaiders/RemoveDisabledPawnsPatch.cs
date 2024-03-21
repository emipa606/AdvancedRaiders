using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace AdvancedRaiders;

[HarmonyPatch(typeof(PawnGroupMakerUtility))]
[HarmonyPatch("ChoosePawnGenOptionsByPoints")]
internal static class RemoveDisabledPawnsPatch
{
    private static readonly PawnKindDef scavDef = DefDatabase<PawnKindDef>.GetNamedSilentFail("Scavenger");

    private static readonly PawnKindDef slasherDef = DefDatabase<PawnKindDef>.GetNamedSilentFail("Mercenary_Slasher");

    private static readonly PawnKindDef berserkerDef = DefDatabase<PawnKindDef>.GetNamedSilentFail("Tribal_Berserker");

    private static readonly PawnKindDef archerDef = DefDatabase<PawnKindDef>.GetNamedSilentFail("Tribal_Archer");

    private static readonly PawnKindDef gunnerDef = DefDatabase<PawnKindDef>.GetNamedSilentFail("Mercenary_Gunner");

    //TODO add AR frequency controls
    [HarmonyPostfix]
    public static IEnumerable<PawnGenOptionWithXenotype> ReplaceDisabledRaiderTypesWithNeutral(
        IEnumerable<PawnGenOptionWithXenotype> options)
    {
        if (options == null)
        {
            yield break;
        }

        foreach (var option in options)
        {
            var kind = option.Option.kind;

            var pawnClass = SpecialUnitUtility.AdvancedRaiderKindDefClass(kind);
            if (!SpecialUnitUtility.IsAllowedAdvancedRaiderClass(pawnClass))
            {
                if (SpecialUnitUtility.IsARAndMeleeFighter(kind))
                {
                    if (pawnClass is PawnClass.MercenaryMedic or PawnClass.MercenaryBulldozer
                        or PawnClass.MercenaryPacifier or PawnClass.MercenaryTechnician)
                    {
                        option.Option.kind = Rand.Chance(0.8f) ? scavDef : slasherDef;
                    }

                    else if (pawnClass != PawnClass.None)
                    {
                        option.Option.kind = berserkerDef;
                    }
                }
                else
                {
                    if (pawnClass is PawnClass.MercenaryMedic or PawnClass.MercenaryBulldozer
                        or PawnClass.MercenaryPacifier or PawnClass.MercenaryTechnician)
                    {
                        option.Option.kind = gunnerDef;
                    }

                    else if (pawnClass != PawnClass.None)
                    {
                        option.Option.kind = archerDef;
                    }
                }
            }

            yield return option;
        }
    }
}