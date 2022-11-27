using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace AdvancedRaiders;

[HarmonyPatch(typeof(IncidentWorker_Raid))]
[HarmonyPatch("TryGenerateRaidInfo")]
internal static class SpawnBeastmasterPetsPatch
{
    [HarmonyPostfix]
    public static void SpawnBeasts(bool __result, ref List<Pawn> pawns, IncidentParms parms)
    {
        if (!__result || pawns == null)
        {
            return;
        }

        var beasts = new List<Pawn>();
        foreach (var pawn in pawns)
        {
            if (SpecialUnitUtility.AdvancedRaiderClass(pawn) == PawnClass.TribalBeastmaster)
            {
                beasts.AddRange(SpecialUnitUtility.GenBeastmasterPetsAndRelations(pawn));
            }
        }

        pawns.AddRange(beasts);
        parms.raidArrivalMode.Worker.Arrive(beasts, parms);
    }
}