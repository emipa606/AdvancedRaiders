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
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {

            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.AddFailCondition(() => (!GetActor().CanReserve(TargetA) && !GetActor().HasReserved(TargetA)));

            yield return Toils_Reserve.Reserve(TargetIndex.A);
            yield return Toils_Goto.Goto(TargetIndex.A, PathEndMode.ClosestTouch);
            yield return Toils_General.Wait(100);
            yield return Toils_General.Do(() =>
                Turret.GetComp<CompBreakdownable>().DoBreakdown()
            );
            yield return Toils_General.Do(() =>                             //these guys arent really engineers, you know
                GetActor().meleeVerbs.TryMeleeAttack(Turret)
            );
            
            yield return Toils_Reserve.Release(TargetIndex.A);

        }
    }
}
