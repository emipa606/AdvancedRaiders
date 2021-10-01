using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace AdvancedRaiders
{
    public class CompAbilityEffect_InspireAllies : CompAbilityEffect
    {
        public new CompProperties_AbilityInspireAllies Props => Props;
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            
        }

       

        protected void InspirePawn(Pawn pawn)
        {
            var hediff = pawn.health.hediffSet.GetFirstHediffOfDef(AdvancedRaidersDefOf.InspirationHediff);
            if (hediff == null)
            {
                hediff = pawn.health.AddHediff(AdvancedRaidersDefOf.InspirationHediff);
                hediff.Severity += Props.inspirationStrength;
            }
            else
            {
                hediff.Severity += Props.inspirationStrength * InspirationMultipiler(pawn);
            }
        }
        protected float InspirationMultipiler(Pawn pawn)
        {
            var hediff = pawn.health.hediffSet.GetFirstHediffOfDef(AdvancedRaidersDefOf.InspirationHediff);

            if (hediff == null)
                return 1f;

            return hediff.Severity > 1 ? 0 : 1 - hediff.Severity;
        }
    }

    public class CompProperties_AbilityInspireAllies : AbilityCompProperties
    {
        public float inspirationStrength;

        public CompProperties_AbilityInspireAllies() => this.compClass = typeof(CompAbilityEffect_InspireAllies);
    }
}
