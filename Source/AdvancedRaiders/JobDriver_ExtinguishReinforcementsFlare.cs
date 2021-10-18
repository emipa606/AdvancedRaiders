using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
using RimWorld;

namespace AdvancedRaiders
{
    public class JobDriver_ExtinguishReinforcementsFlare : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(TargetA, job);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.Goto(TargetIndex.A, PathEndMode.ClosestTouch);
            Toil waitToil = Toils_General.Wait(30);
            waitToil.WithProgressBarToilDelay(TargetIndex.A);
            waitToil.FailOnDespawnedOrNull(TargetIndex.A);
            waitToil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            yield return waitToil;
            yield return Toils_General.Do(Extinguish);
        }

        private void Extinguish()
        {
            GenSpawn.Spawn(AdvancedRaidersDefOf.Filth_BurntFlare, TargetLocA, Map);
            TargetThingA.Destroy();
        }
    }
}
