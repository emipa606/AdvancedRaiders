using HarmonyLib;
using Verse;

namespace AdvancedRaiders;

[HarmonyPatch(typeof(Pawn))]
[HarmonyPatch("SpawnSetup")]
internal static class PawnSetupPatch
{
    [HarmonyPostfix]
    public static void PawnSetupPostfix(Pawn __instance)
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

        //if (SpecialUnitUtility.AdvancedRaiderClass(__instance) == PawnClass.TribalBeastmaster)
        //    SpecialUnitUtility.GenBeastmasterPetsAndRelations(__instance);

        if (SpecialUnitUtility.AdvancedRaiderClass(__instance) == PawnClass.MercenaryBulldozer &&
            __instance.abilities.GetAbility(AdvancedRaidersDefOf.TauntAbility) == null)
        {
            __instance.abilities.GainAbility(AdvancedRaidersDefOf.TauntAbility);
        }
    }
}