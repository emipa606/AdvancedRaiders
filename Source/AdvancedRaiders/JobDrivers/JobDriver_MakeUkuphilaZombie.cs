using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace AdvancedRaiders;

public class JobDriver_MakeUkuphilaZombie : JobDriver
{
    public Corpse TargetCorpse => TargetThingA as Corpse;
    public Thing UkuphilaHerb => TargetThingB;

    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        return pawn.Map.reservationManager.Reserve(pawn, job, job.targetA);
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
        _ = TargetCorpse.InnerPawn;
        this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
        AddFailCondition(() => !GetActor().CanReserve(TargetA) && !GetActor().HasReserved(TargetA));


        yield return Toils_Goto.Goto(TargetIndex.A, PathEndMode.ClosestTouch);
        yield return Toils_Misc.TakeItemFromInventoryToCarrier(GetActor(), TargetIndex.B);
        yield return Toils_General.Wait(100);
        yield return Toils_General.Do(MakeUkuphilaZombie);
        //better let zombies be controlled by lord
        /*yield return Toils_General.Do(() =>
            pawnToResurrect.mindState.mentalStateHandler.TryStartMentalState(
                AdvancedRaidersDefOf.UkuphilaResurrectionPsychosis,
                "resurrected with ukuphila herb",
                forceWake: true,
                otherPawn: GetActor(),
                transitionSilently: true));*/
    }

    private void MakeUkuphilaZombie()
    {
        var innerPawn = TargetCorpse.InnerPawn;
        SpecialUnitUtility.MakeUkuphilaZombie(innerPawn);
        if (!innerPawn.Dead && GetActor().GetLord() != null)
        {
            GetActor().GetLord().AddPawn(innerPawn);
        }

        UkuphilaHerb.Destroy();
    }
}