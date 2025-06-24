using System.Collections.Generic;
using HarmonyLib;
using Verse;
using Verse.AI.Group;

namespace AdvancedRaiders;

[HarmonyPatch(typeof(LordMaker), nameof(LordMaker.MakeNewLord))]
internal static class LordMaker_MakeNewLord
{
    public static void Postfix(IEnumerable<Pawn> startingPawns)
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