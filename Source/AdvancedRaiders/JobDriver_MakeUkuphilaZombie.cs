using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace AdvancedRaiders
{
    public class JobDriver_MakeUkuphilaZombie : JobDriver
    {
        public Corpse TargetCorpse => TargetThingA as Corpse;
        public Thing UkuphilaHerb => TargetThingB;
        
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Map.reservationManager.Reserve(pawn, job, job.targetA); ;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            Pawn pawnToResurrect = TargetCorpse.InnerPawn;
            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.AddFailCondition(() => (!GetActor().CanReserve(TargetA) && !GetActor().HasReserved(TargetA)));

            
            yield return Toils_Goto.Goto(TargetIndex.A, PathEndMode.ClosestTouch);
            yield return Toils_Misc.TakeItemFromInventoryToCarrier(GetActor(), TargetIndex.B);
            yield return Toils_General.Wait(100);
            yield return Toils_General.Do((Action)this.MakeUkuphilaZombie);
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
            Pawn innerPawn = TargetCorpse.InnerPawn;
            SpecialUnitUtility.MakeUkuphilaZombie(innerPawn);
            if(!innerPawn.Dead && GetActor().GetLord()!=null)
                GetActor().GetLord().AddPawn(innerPawn);        
            UkuphilaHerb.Destroy();
        }
    }
}
