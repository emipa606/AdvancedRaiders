using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Security;
using Verse;
using RimWorld;
using HarmonyLib;
using Verse.AI.Group;
using System.Reflection.Emit;
using Verse.AI;

namespace AdvancedRaiders
{
    [StaticConstructorOnStartup]
    class Patch
    {
        static Patch()
        {
            AdvancedRaiders.harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(Lord))]
    [HarmonyPatch("SetJob")]
    public static class OogaBoogaGoWaaaagh
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> DontPanicFleeIfInspirerPresent(IEnumerable<CodeInstruction> instructions)
        {
            foreach(var line in instructions)
            {
                if ((line.opcode == OpCodes.Newobj) &&
                    ((ConstructorInfo) line.operand == typeof(Trigger_FractionPawnsLost).GetConstructor(new Type[] { typeof(float) })))     
                {
                    yield return new CodeInstruction(OpCodes.Newobj, typeof(Trigger_FractionPawnsLostAndNoInspirer).GetConstructor(new Type[] { typeof(float) }));
                }
                else
                    yield return line;
            }
        }
    }
   
    [HarmonyPatch(typeof(Pawn))]
    [HarmonyPatch("SpawnSetup")]
    static class PawnSetupPatch
    {
        [HarmonyPostfix]
        public static void PawnSetupPostfix(Pawn __instance)
        {
            if (__instance == null)
                return;

            if (__instance.Spawned)
            {
                if (__instance.kindDef == AdvancedRaidersDefOf.Tribal_ChiefCommander && 
                    __instance.abilities.GetAbility(AdvancedRaidersDefOf.InspireAlliesAbility) == null)

                    __instance.abilities.GainAbility(AdvancedRaidersDefOf.InspireAlliesAbility);

                if (__instance.kindDef == AdvancedRaidersDefOf.Tribal_Beastmaster)
                {
                    SpecialUnitUtility.GenBeastmasterPetsAndRelations(__instance);
                }
            }
        }
    }

    [HarmonyPatch(typeof(Building_TurretGun))]
    [HarmonyPatch("IsValidTarget")]
    static class TurretPatch
    {
        [HarmonyPostfix]
        public static void DontShootPawnsWithBlueScreenBelt(Building_TurretGun __instance, ref bool __result, Thing t)
        {
            if (t == null)
                return;
            if (__result == true && 
                __instance.Faction!=Faction.OfMechanoids &&
                t is Pawn pawn)
            {
                if (pawn.apparel == null)
                    return;

                foreach (var ap in pawn.apparel.WornApparel)
                {
                    if (ap.def == AdvancedRaidersDefOf.Apparel_BlueScreenBelt)
                    {
                        __result = false;
                        break;
                    }    
                }
            }
        }
    }

    [HarmonyPatch(typeof(LordMaker))]
    [HarmonyPatch("MakeNewLord")]
    static class LordMakerPatch
    {
        [HarmonyPostfix]
        public static void AddBeastmasterPets(IEnumerable<Pawn> startingPawns)
        {
            if (startingPawns == null)
                return;

            foreach(var pawn in startingPawns)
            {
                if (pawn!=null && pawn.kindDef == AdvancedRaidersDefOf.Tribal_Beastmaster)
                    SpecialUnitUtility.AddPetsToBeastmasterLord(pawn);
            }
        }
    }
}
