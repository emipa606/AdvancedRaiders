using Verse.AI;

namespace AdvancedRaiders;

public class JobDriver_KillWithoutReservations : JobDriver_Kill
{
    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        return true;
    }
}