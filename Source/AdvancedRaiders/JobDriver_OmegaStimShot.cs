using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace AdvancedRaiders
{
    public class JobDriver_OmegaStimShot : JobDriver
    {
        public Pawn FirstAidTarget => TargetThingA as Pawn;
        public Thing FirstAidDrug => TargetThingB;

        private bool NoNeedInOmegaStim()
        {
            
            
            if (FirstAidTarget.health.State != PawnHealthState.Down)        
                return true;

            if (FirstAidTarget.health.hediffSet.HasHediff(AdvancedRaidersDefOf.OmegaStimulantHigh))
                return true;

            return false;

        }

        public bool FirstAidTargetIsMobile => FirstAidTarget.health.State == PawnHealthState.Mobile;
    
        public JobDriver_OmegaStimShot() : base()
        {
        }


        protected override IEnumerable<Toil> MakeNewToils()
        {
            AddEndCondition(delegate
            {

                if (NoNeedInOmegaStim())
                {
                    return JobCondition.Incompletable;
                }
                if (!GetActor().CanReserve(TargetA) && !GetActor().HasReserved(TargetA))
                {
                    return JobCondition.Incompletable;
                }

                return JobCondition.Ongoing;
            });

            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.AddFailCondition(NoNeedInOmegaStim);
            this.AddFailCondition(() => (!GetActor().CanReserve(TargetA) && !GetActor().HasReserved(TargetA)));

            yield return Toils_Reserve.Reserve(TargetIndex.A);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
            yield return Toils_Misc.TakeItemFromInventoryToCarrier(GetActor(), TargetIndex.B);
            yield return Toils_Ingest.ChewIngestible(FirstAidTarget, 1f, TargetIndex.B).FailOnCannotTouch<Toil>(TargetIndex.B, PathEndMode.Touch);

            Toil bringDownedBackToFight = new Toil();
            bringDownedBackToFight.defaultCompleteMode = ToilCompleteMode.Instant;
            bringDownedBackToFight.initAction = (Action)(() =>
            {
                if (GetActor().GetLord()!=null && !GetActor().GetLord().ownedPawns.Contains(FirstAidTarget))
                    GetActor().GetLord().AddPawn(FirstAidTarget);
            });
            yield return bringDownedBackToFight;

            yield return Toils_Ingest.FinalizeIngest(FirstAidTarget, TargetIndex.B);
            yield return Toils_Reserve.Release(TargetIndex.A);
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }
    }
}
