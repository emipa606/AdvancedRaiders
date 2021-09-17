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
   


    public class JobDriver_FirstAid : JobDriver
    {
        public Pawn FirstAidTarget => TargetThingA as Pawn;
        public Thing FirstAidDrug => TargetThingB;

        private bool TargetDoesntNeedFirstAid()
        {
            
            if (FirstAidTargetIsMobile)
                return true;

            if (FirstAidTarget.health.State != PawnHealthState.Down)        //add ressurect action
                return true;

            if (FirstAidTarget.health.hediffSet.HasHediff(AdvancedRaidersDefOf.OmegaStimulantHigh))
                return true;

            return false;

        }

        public bool FirstAidTargetIsMobile => FirstAidTarget.health.State == PawnHealthState.Mobile;
    
        public JobDriver_FirstAid() : base()
        {
        }


        protected override IEnumerable<Toil> MakeNewToils()
        {
            AddEndCondition(delegate
            {

                if (TargetDoesntNeedFirstAid())
                {
                    return JobCondition.Incompletable;
                }
                if (!GetActor().CanReserve(TargetA))
                    return JobCondition.Incompletable;
                return JobCondition.Ongoing;
            });

            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.AddFailCondition(TargetDoesntNeedFirstAid);

            yield return Toils_Reserve.Reserve(TargetIndex.A);


            Toil gotoToil = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
            yield return gotoToil;

            yield return Toils_Misc.TakeItemFromInventoryToCarrier(GetActor(), TargetIndex.B);

            Toil ingestToil = Toils_Ingest.ChewIngestible(FirstAidTarget, 1f, TargetIndex.B).FailOnCannotTouch<Toil>(TargetIndex.B, PathEndMode.Touch);


            yield return ingestToil;
            yield return Toils_Ingest.FinalizeIngest(FirstAidTarget, TargetIndex.B);
            yield return new Toil()
            {
                initAction = (Action)(() =>
                {
                    GetActor().GetLord().AddPawn(FirstAidTarget);
                })
            };
           
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }
    }
}
