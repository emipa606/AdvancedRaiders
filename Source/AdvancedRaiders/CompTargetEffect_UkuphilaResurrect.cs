using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace AdvancedRaiders
{
    public class CompTargetEffect_UkuphilaResurrect : CompTargetEffect
    {
        public override void DoEffectOn(Pawn user, Thing target)
        {
            if (!user.CanReserveAndReach(target, PathEndMode.Touch, Danger.Deadly))
                return;

        }
    }
}
