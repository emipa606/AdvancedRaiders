using System.Collections.Generic;
using Verse.AI;

namespace AdvancedRaiders;

public class JobDriver_InspireAllies : JobDriver
{
    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        return true;
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
        var ability = GetActor().abilities.GetAbility(AdvancedRaidersDefOf.InspireAlliesAbility, true);
        AddFailCondition(() => ability == null);
        AddFailCondition(() => !ability.CanCast || ability.Casting);

        //if (ability.CanCast && !ability.Casting)
        yield return Toils_General.Do(() => ability.Activate(TargetA, TargetB));
    }
}