using Verse;

namespace AdvancedRaiders;

public class CompProperties_ReinforcementsSummoner : CompProperties
{
    public int reinforcementsDelay = 1500;


    public CompProperties_ReinforcementsSummoner()
    {
        compClass = typeof(CompReinforcementsSummoner);
    }
}