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

    

  /*  [HarmonyPatch(typeof(Building_Trap), "KnowsOfTrap")]
    class TechniciansCanSeeTrapsPatch
    {
        [HarmonyPostfix]
        public static bool TechnicansCanSeeTraps(ref bool __result, Pawn __p)
        {
            __result = __result || __p.kindDef == AdvancedRaidersDefOf.Mercenaty_Technician;
            return __result;
        }
    }*/

}
