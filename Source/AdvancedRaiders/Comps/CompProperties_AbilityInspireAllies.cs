using Verse;

namespace AdvancedRaiders;

public class CompProperties_AbilityInspireAllies : AbilityCompProperties
{
    public readonly float soreThroatEffectFactor = 1;
    public float inspirationStrength;
    public float radius;

    public CompProperties_AbilityInspireAllies()
    {
        compClass = typeof(CompAbilityEffect_InspireAllies);
    }
}