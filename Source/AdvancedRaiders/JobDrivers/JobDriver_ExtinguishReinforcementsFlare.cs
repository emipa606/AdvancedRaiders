using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace AdvancedRaiders;

public class JobDriver_ExtinguishReinforcementsFlare : JobDriver
{
    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        return pawn.Reserve(TargetA, job);
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
        yield return Toils_Goto.Goto(TargetIndex.A, PathEndMode.ClosestTouch);
        var waitToil = Toils_General.Wait(30);
        waitToil.WithProgressBarToilDelay(TargetIndex.A);
        waitToil.FailOnDespawnedOrNull(TargetIndex.A);
        waitToil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
        yield return waitToil;
        yield return Toils_General.Do(extinguish);
    }

    private void extinguish()
    {
        GenSpawn.Spawn(AdvancedRaidersDefOf.Filth_BurntFlare, TargetLocA, Map);
        TargetThingA.Destroy();
    }
}