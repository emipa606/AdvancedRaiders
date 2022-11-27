using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace AdvancedRaiders;

public class JobDriver_ResurrectAndGiveIngestible : JobDriver
{
    private Corpse Corpse => TargetThingA as Corpse;
    private Thing Item => TargetThingB;

    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        return pawn.Reserve(TargetA, job, errorOnFailed: errorOnFailed) &&
               pawn.Reserve(TargetB, job, errorOnFailed: errorOnFailed);
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
        yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.B)
            .FailOnDespawnedOrNull(TargetIndex.A);
        yield return Toils_Haul.StartCarryThing(TargetIndex.B);
        yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
        var toil = Toils_General.Wait(600);
        toil.WithProgressBarToilDelay(TargetIndex.A);
        toil.FailOnDespawnedOrNull(TargetIndex.A);
        toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
        yield return toil;

        var innerPawn = Corpse.InnerPawn;
        yield return Toils_General.Do(Resurrect);
        yield return Toils_Ingest.FinalizeIngest(innerPawn, TargetIndex.B);
    }

    private void Resurrect()
    {
        var innerPawn = Corpse.InnerPawn;
        ResurrectionUtility.Resurrect(innerPawn);
        Messages.Message("MessagePawnResurrected".Translate(innerPawn), innerPawn, MessageTypeDefOf.PositiveEvent);
    }
}