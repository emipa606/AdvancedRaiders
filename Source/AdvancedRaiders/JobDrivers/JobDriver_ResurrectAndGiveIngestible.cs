using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;

namespace AdvancedRaiders
{
    public class JobDriver_ResurrectAndGiveIngestible : JobDriver
    {
        Corpse Corpse => TargetThingA as Corpse;
        Thing Item => TargetThingB;
        public override bool TryMakePreToilReservations(bool errorOnFailed) => 
            pawn.Reserve(TargetA, job, errorOnFailed: errorOnFailed) && 
            pawn.Reserve(TargetB, job, errorOnFailed: errorOnFailed);

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedOrNull<Toil>(TargetIndex.B).FailOnDespawnedOrNull<Toil>(TargetIndex.A);
            yield return Toils_Haul.StartCarryThing(TargetIndex.B);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull<Toil>(TargetIndex.A);
            Toil toil = Toils_General.Wait(600);
            toil.WithProgressBarToilDelay(TargetIndex.A);
            toil.FailOnDespawnedOrNull<Toil>(TargetIndex.A);
            toil.FailOnCannotTouch<Toil>(TargetIndex.A, PathEndMode.Touch);
            yield return toil;

            Pawn innerPawn = Corpse.InnerPawn;
            yield return Toils_General.Do(new Action(Resurrect));
            yield return Toils_Ingest.FinalizeIngest(innerPawn, TargetIndex.B);
        }

        private void Resurrect()
        {
            Pawn innerPawn = Corpse.InnerPawn;
            ResurrectionUtility.Resurrect(innerPawn);
            Messages.Message("MessagePawnResurrected".Translate(innerPawn), innerPawn, MessageTypeDefOf.PositiveEvent);
        }
    }

}
