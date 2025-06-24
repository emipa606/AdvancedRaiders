using System.Reflection;
using Verse;

namespace AdvancedRaiders;

[StaticConstructorOnStartup]
internal class Patch
{
    static Patch()
    {
        AdvancedRaiders.HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
    }
}