using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Verse;
using RimWorld;
using HarmonyLib;
using Verse.AI.Group;

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

    [HarmonyPatch(typeof(LordJob), "LordJobTick")]
    class AssaultColonyPatch
    {
        [HarmonyPostfix]
        public static void TryGiveJobsToSpecialUnits(LordJob __instance)//i should find a better way to patch ticks. mb path the ticker itself?
        {
            if (Find.TickManager.TicksGame % 250 == 0)       //not the best way since all medics rush to target at the same time. FIX.       
            {
                __instance.lord.TryKindSpecificJobGiversOnOwnedPawns();
            }
        }
    }
}
