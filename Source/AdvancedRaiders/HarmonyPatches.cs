using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Verse;
using Verse.AI;
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
        public static void PawnSetupPostfix(Pawn __instance)
        {
            if (__instance == null)
                return;

            if (__instance.Spawned)
            {
                if (SpecialUnitUtility.AdvancedRaiderClass(__instance) ==PawnClass.TribalInspirer && __instance.abilities.GetAbility(AdvancedRaidersDefOf.InspireAlliesAbility) == null)
                    __instance.abilities.GainAbility(AdvancedRaidersDefOf.InspireAlliesAbility);

                //if (SpecialUnitUtility.AdvancedRaiderClass(__instance) == PawnClass.TribalBeastmaster)
                //    SpecialUnitUtility.GenBeastmasterPetsAndRelations(__instance);

                if (SpecialUnitUtility.AdvancedRaiderClass(__instance) == PawnClass.MercenaryBulldozer && __instance.abilities.GetAbility(AdvancedRaidersDefOf.TauntAbility) == null)
                    __instance.abilities.GainAbility(AdvancedRaidersDefOf.TauntAbility);
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
                if (pawn != null && pawn.kindDef == AdvancedRaidersDefOf.Tribal_Beastmaster)
                {
                    SpecialUnitUtility.AddPetsToBeastmasterLord(pawn);
                }
            }
        }
    }

    [HarmonyPatch(typeof(IncidentWorker_Raid))]
    [HarmonyPatch("TryGenerateRaidInfo")]
    static class SpawnBeastmasterPetsPatch
    {
        [HarmonyPostfix]
        public static void SpawnBeasts(bool __result, ref List<Pawn> pawns, IncidentParms parms)
        {
            if (__result && pawns != null)
            {
                List<Pawn> beasts = new List<Pawn>();
                for (int i = 0; i < pawns.Count; i++)
                {
                    if (SpecialUnitUtility.AdvancedRaiderClass(pawns[i]) == PawnClass.TribalBeastmaster)
                    {
                        beasts.AddRange(SpecialUnitUtility.GenBeastmasterPetsAndRelations(pawns[i]));
                    }
                }
                pawns.AddRange(beasts);
                parms.raidArrivalMode.Worker.Arrive(beasts, parms);
            }
            
        }

    }
            
    

    /*
    
    I have no idea how to patch lambda. Help.

    [HarmonyPatch(typeof(PawnGroupMakerUtility), "ChoosePawnGenOptionsByPoints")]
    static class ApplySettingsToPawnGenerator
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> MultiplyWeight(IEnumerable<CodeInstruction> instructions)
        {
            // Original IL code piece:
            
            //    IL_0000: ldarg.1      // gr
            //    IL_0001: ldfld        float32 RimWorld.PawnGenOption::selectionWeight
            //    IL_0006: ldsfld       class Verse.SimpleCurve RimWorld.PawnGroupMakerUtility::PawnWeightFactorByMostExpensivePawnCostFractionCurve
            //    IL_000b: ldarg.1      // gr
            //    IL_000c: callvirt     instance float32 RimWorld.PawnGenOption::get_Cost()
            //    IL_0011: ldarg.0      // this
            //    IL_0012: ldfld        float32 RimWorld.PawnGroupMakerUtility/'<>c__DisplayClass5_0'::highestCost
            //    IL_0017: div
            //    IL_0018: callvirt     instance float32 Verse.SimpleCurve::Evaluate(float32)
            //    IL_001d: mul
            //    IL_001e: ret
            
            OpCode prevOpCode = OpCodes.Ldarg_0;            
            foreach (var line in instructions)
            {
                yield return line;
                if (line.opcode == OpCodes.Mul && prevOpCode==OpCodes.Callvirt)
                {
                    Log.Message("AR: patching sucsessful");
                    //this should be equal to <*SpecialUnitUtility.GetSpawnFactor(SpecialUnitUtility.AdvancedRaiderKindDefClass(gr.kind))>

                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return new CodeInstruction(OpCodes.Ldfld, typeof(PawnGenOption).GetField("kind"));
                    yield return new CodeInstruction(OpCodes.Call, typeof(SpecialUnitUtility).GetMethod("AdvancedRaiderKindDefClass"));
                    yield return new CodeInstruction(OpCodes.Call, typeof(SpecialUnitUtility).GetMethod("GetSpawnFactor"));
                    yield return new CodeInstruction(OpCodes.Mul);

                }

                prevOpCode = line.opcode;
            }   
        }
    }
    */

    [HarmonyPatch(typeof(PawnGroupMakerUtility))]
    [HarmonyPatch("ChoosePawnGenOptionsByPoints")]
    static class RemoveDisabledPawnsPatch
    {
        static PawnKindDef scavDef = DefDatabase<PawnKindDef>.AllDefs.Where((PawnKindDef def) => def.defName == "Scavenger").First();
        static PawnKindDef slasherDef = DefDatabase<PawnKindDef>.AllDefs.Where((PawnKindDef def) => def.defName == "Mercenary_Slasher").First();
        static PawnKindDef berserkerDef = DefDatabase<PawnKindDef>.AllDefs.Where((PawnKindDef def) => def.defName == "Tribal_Berserker").First();
        static PawnKindDef archerDef = DefDatabase<PawnKindDef>.AllDefs.Where((PawnKindDef def) => def.defName == "Tribal_Archer").First();
        static PawnKindDef gunnerDef = DefDatabase<PawnKindDef>.AllDefs.Where((PawnKindDef def) => def.defName == "Mercenary_Gunner").First();
        

        //TODO add AR frequency controls
        [HarmonyPostfix]
        public static IEnumerable<PawnGenOption> ReplaceDisabledRaiderTypesWithNeutral(IEnumerable<PawnGenOption> options)
        {
            foreach (var option in options)
            {
                var kind = option.kind;
                var pawnClass = SpecialUnitUtility.AdvancedRaiderKindDefClass(kind);
                if (!SpecialUnitUtility.IsAllowedAdvancedRaiderClass(pawnClass))
                {
                    if (SpecialUnitUtility.IsARAndMeleeFighter(kind))
                    {
                        if (pawnClass == PawnClass.MercenaryMedic ||
                            pawnClass == PawnClass.MercenaryBulldozer ||
                            pawnClass == PawnClass.MercenaryPacifier ||
                            pawnClass == PawnClass.MercenaryTechnician)
                        {
                            if (Rand.Chance(0.8f))
                                option.kind = scavDef;
                            else
                                option.kind = slasherDef;
                        }

                        else if (pawnClass!=PawnClass.None)
                        {
                            option.kind = berserkerDef;
                        }
                    }
                    else
                    {
                        if (pawnClass == PawnClass.MercenaryMedic ||
                            pawnClass == PawnClass.MercenaryBulldozer ||
                            pawnClass == PawnClass.MercenaryPacifier ||
                            pawnClass == PawnClass.MercenaryTechnician)
                        {
                            option.kind = gunnerDef;
                        }

                        else if (pawnClass !=PawnClass.None)
                        {
                            option.kind = archerDef;
                        }
                    }
                }
                yield return option;    
            }
        }
    }
}
