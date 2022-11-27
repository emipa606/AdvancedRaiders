using Verse;

namespace AdvancedRaiders;

public class CompProperties_AbilityInspireAllies : AbilityCompProperties
{
    public float inspirationStrength;
    public float radius;
    public float soreThroatEffectFactor = 1;

    public CompProperties_AbilityInspireAllies()
    {
        compClass = typeof(CompAbilityEffect_InspireAllies);
    }
}