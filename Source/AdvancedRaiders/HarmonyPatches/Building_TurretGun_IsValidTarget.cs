using HarmonyLib;
using RimWorld;
using Verse;

namespace AdvancedRaiders;

[HarmonyPatch(typeof(Building_TurretGun), "IsValidTarget")]
internal static class Building_TurretGun_IsValidTarget
{
    public static void Postfix(Building_TurretGun __instance, ref bool __result, Thing t)
    {
        if (t == null)
        {
            return;
        }

        if (!__result ||
            __instance.Faction == Faction.OfMechanoids ||
            t is not Pawn pawn)
        {
            return;
        }

        if (pawn.apparel == null)
        {
            return;
        }

        foreach (var ap in pawn.apparel.WornApparel)
        {
            if (ap.def != AdvancedRaidersDefOf.Apparel_BlueScreenBelt)
            {
                continue;
            }

            __result = false;
            break;
        }
    }
}