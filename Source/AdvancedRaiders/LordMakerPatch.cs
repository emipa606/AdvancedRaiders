using System.Collections.Generic;
using HarmonyLib;
using Verse;
using Verse.AI.Group;

namespace AdvancedRaiders;

[HarmonyPatch(typeof(LordMaker))]
[HarmonyPatch("MakeNewLord")]
internal static class LordMakerPatch
{
    [HarmonyPostfix]
    public static void AddBeastmasterPets(IEnumerable<Pawn> startingPawns)
    {
        if (startingPawns == null)
        {
            return;
        }

        foreach (var pawn in startingPawns)
        {
            if (pawn != null && pawn.kindDef == AdvancedRaidersDefOf.Tribal_Beastmaster)
            {
                SpecialUnitUtility.AddPetsToBeastmasterLord(pawn);
            }
        }
    }
}