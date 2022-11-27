using Verse;

namespace AdvancedRaiders;

public class CompProperties_AbilityTaunt : AbilityCompProperties
{
    public int durationTicks = 600;

    public CompProperties_AbilityTaunt()
    {
        compClass = typeof(CompAbilityEffect_Taunt);
    }
}