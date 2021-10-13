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
    public class JobDriver_BreakTurret : JobDriver
    {
        private Building Turret => TargetThingA as Building;
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Map.reservationManager.Reserve(pawn, job, job.targetA);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {

            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.AddFailCondition(() => (!GetActor().CanReserve(TargetA) && !GetActor().HasReserved(TargetA)));

            yield return Toils_Goto.Goto(TargetIndex.A, PathEndMode.ClosestTouch);
            yield return Toils_General.Wait(100);

            if (Rand.Value < 0.5)
            {
                //break turret
                yield return Toils_General.Do(() =>
                    Turret.GetComp<CompBreakdownable>().DoBreakdown()
                );
            }
            else
            {
                //bug turret
                yield return Toils_General.Do(() =>
                    Turret.SetFaction(GetActor().Faction)
                );
            }

            yield return Toils_General.Do(() =>                             //these guys arent really engineers, you know
                GetActor().meleeVerbs.TryMeleeAttack(Turret)
            );
        }
    }
}
