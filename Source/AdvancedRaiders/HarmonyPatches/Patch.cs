using System.Reflection;
using Verse;

namespace AdvancedRaiders;

[StaticConstructorOnStartup]
internal static class Patch
{
    static Patch()
    {
        AdvancedRaidersMod.HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
    }
}