using RimWorld;
using RimWorld.Planet;
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
        public new CompProperties_AbilityInspireAllies Props => props as CompProperties_AbilityInspireAllies;
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            InspireAlliesInRange(parent.pawn.Position);
        }

        public override void Apply(GlobalTargetInfo target) => Apply(null, null);               //rewrite later

        protected void InspireAlliesInRange(LocalTargetInfo center)
        {
            var alliesInRange =
                from p in parent.pawn.Map.mapPawns.FreeHumanlikesSpawnedOfFaction(parent.pawn.Faction)
                where center.Cell.DistanceTo(p.Position) <= Props.radius
                select p;

            foreach (var pawn in alliesInRange)
            {
                InspirePawn(pawn);
            }
        }

        public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest) => true;          //not the best decision probably

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

            return hediff.Severity > 1 ? 0 : (1 - hediff.Severity);
        }
    }

    public class CompProperties_AbilityInspireAllies : AbilityCompProperties
    {
        public float inspirationStrength;
        public float radius;

        public CompProperties_AbilityInspireAllies() => this.compClass = typeof(CompAbilityEffect_InspireAllies);
    }
}
