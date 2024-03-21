using Verse;

namespace AdvancedRaiders;

public class CompProperties_ReinforcementsSummoner : CompProperties
{
    public readonly int reinforcementsDelay = 1500;

    public CompProperties_ReinforcementsSummoner()
    {
        compClass = typeof(CompReinforcementsSummoner);
    }
}