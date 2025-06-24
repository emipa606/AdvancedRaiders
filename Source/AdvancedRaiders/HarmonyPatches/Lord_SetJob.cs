using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Verse.AI.Group;

namespace AdvancedRaiders;

[HarmonyPatch(typeof(Lord), nameof(Lord.SetJob))]
public static class Lord_SetJob
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        foreach (var line in instructions)
        {
            if (line.opcode == OpCodes.Newobj &&
                (ConstructorInfo)line.operand ==
                typeof(Trigger_FractionPawnsLost).GetConstructor([typeof(float)]))
            {
                yield return new CodeInstruction(OpCodes.Newobj,
                    typeof(Trigger_FractionPawnsLostAndNoInspirer).GetConstructor([typeof(float)]));
            }
            else
            {
                yield return line;
            }
        }
    }
}