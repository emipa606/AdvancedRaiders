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
        public static void GiveAbilitiesToSpecialUnits(Pawn __instance)
        {
            if (__instance.Spawned)
            {
                if (__instance.kindDef == AdvancedRaidersDefOf.Tribal_ChiefCommander && 
                    __instance.abilities.GetAbility(AdvancedRaidersDefOf.InspireAlliesAbility) == null)

                    __instance.abilities.GainAbility(AdvancedRaidersDefOf.InspireAlliesAbility);
            }
        }
    }
}
