using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using Verse.AI.Group;
using Verse.AI;

namespace AdvancedRaiders
{
    public static class SpecialUnitUtility
    {
        
        public static void MakeUkuphilaZombie(Pawn pawn)
        {
            if (!pawn.Dead)
            {
                Log.Error("Attemp to make ukuphila zombie from living pawn");
                return;
            }

            if (!pawn.RaceProps.Humanlike)
            {
                Log.Error("Attempt to make ukuphila zombie from non-humanlike");
                return;
            }

            if (!pawn.health.hediffSet.HasHead)
            {
                Log.Message("Attempted to make ukuphila zombie from headless corpse. Fail");
                return;
            }

            ResurrectionUtility.Resurrect(pawn);
            pawn.health.AddHediff(AdvancedRaidersDefOf.UkuphilaResurrection);
            
            if (pawn.Dead)
                Log.Error("Looks like even forbidden herbs couldn't revive " + pawn.Name + ". Press F and fix this asap, it shouldnt happen");



        }
    }
}
