using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using Verse.AI;
namespace AdvancedRaiders
{
    public class JobDriver_InjectStimulant : JobDriver
    {
        public Pawn injectionTarget;

        public bool InjectionTargetIsMobile => injectionTarget.health.State == PawnHealthState.Mobile;
        public void InitAction()
        {
            injectionTarget = null;
        }

        public void TickAction()
        {

        }

        public bool SearchForOmegaStimulantInjectionTarget()
        {
            var curMap = this.GetActor().Map;
            var downedAllies = from p in curMap.mapPawns.FreeHumanlikesSpawnedOfFaction(this.GetActor().Faction)
                               where p.Downed && p.WouldBeMobileAfterOmegaStimInjection()
                               select p;

            if (downedAllies.Count() == 0)
            {
                return false;
            }

            Pawn closestDownedAlly = downedAllies.First();
            float minPathToDownedCost = 1e35f;
            foreach (var downed in downedAllies)
            {
                float distToDowned = (curMap.pathFinder.FindPath(this.GetActor().Position, downed.Position, this.GetActor(), PathEndMode.InteractionCell)).TotalCost;
                if (distToDowned < minPathToDownedCost)
                {
                    minPathToDownedCost = distToDowned;
                    closestDownedAlly = downed;
                }
            }

            injectionTarget = closestDownedAlly;
            return true;
        }

        public bool HasAnyStimulants
        {
            get
            {
                if (GetActor().HasAnyOf()) //TODO
            }
        }

        

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return new Toil()
            {
                initAction = new Action(this.InitAction),
                tickAction = new Action(this.TickAction),
                defaultCompleteMode = ToilCompleteMode.FinishedBusy
            };
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }
    }
}
