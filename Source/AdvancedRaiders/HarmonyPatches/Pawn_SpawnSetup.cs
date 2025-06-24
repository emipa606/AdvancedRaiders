using HarmonyLib;
using Verse;

namespace AdvancedRaiders;

[HarmonyPatch(typeof(Pawn), nameof(Pawn.SpawnSetup))]
internal static class Pawn_SpawnSetup
{
    public static void Postfix(Pawn __instance)
    {
        if (__instance == null)
        {
            return;
        }

        if (!__instance.Spawned)
        {
            return;
        }

        if (SpecialUnitUtility.AdvancedRaiderClass(__instance) == PawnClass.TribalInspirer &&
            __instance.abilities.GetAbility(AdvancedRaidersDefOf.InspireAlliesAbility) == null)
        {
            __instance.abilities.GainAbility(AdvancedRaidersDefOf.InspireAlliesAbility);
        }

        if (SpecialUnitUtility.AdvancedRaiderClass(__instance) == PawnClass.MercenaryBulldozer &&
            __instance.abilities.GetAbility(AdvancedRaidersDefOf.TauntAbility) == null)
        {
            __instance.abilities.GainAbility(AdvancedRaidersDefOf.TauntAbility);
        }
    }
}