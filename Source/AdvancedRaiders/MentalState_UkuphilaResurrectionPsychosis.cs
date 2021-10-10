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
    public class MentalState_UkuphilaResurrectionPsychosis : MentalState
    {
        //private int _findNewTargetCounter = 0;                      //zombie doesnt need to search for new target every 30 ticks
        public override bool ForceHostileTo(Thing t) => (t is Pawn p) && p.RaceProps.Humanlike && this.ForceHostileTo(p.Faction);
        public override bool ForceHostileTo(Faction f) => pawn.Faction == null ? true : pawn.Faction != f;
        public override RandomSocialMode SocialModeMax() => RandomSocialMode.Off;
        public override bool AllowRestingInBed => false;

        /*public override void MentalStateTick()
        {
            if (_findNewTargetCounter!=5)
            {
                _findNewTargetCounter++;
                return;
            }
            _findNewTargetCounter = 0;

            Predicate<Thing> validator = (t =>
            {
                Pawn p = t as Pawn;
                return
                p != null &&
                p.RaceProps.Humanlike &&
                p.Faction != pawn.Faction &&
                p.health.State == PawnHealthState.Mobile;
            });

            Pawn target = (Pawn)GenClosest.ClosestThingReachable(
                pawn.Position, pawn.Map, 
                ThingRequest.ForGroup(ThingRequestGroup.Pawn), 
                PathEndMode.InteractionCell, 
                TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly), 
                validator: validator);

            if (target == null)
                return;

            Job job = JobMaker.MakeJob(JobDefOf.AttackMelee);
            job.targetA = target;
            pawn.jobs.StartJob(job);

        }*/
    }
}
