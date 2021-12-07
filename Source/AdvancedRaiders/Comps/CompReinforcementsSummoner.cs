using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using Verse.AI;
namespace AdvancedRaiders
{
    public class CompReinforcementsSummoner : ThingComp
    {
        public CompProperties_ReinforcementsSummoner Props => props as CompProperties_ReinforcementsSummoner;
        private int _ageTicks;
        IncidentParms parms;
        public int reinforcementsPoints=250;
        public override void CompTick()
        {
            base.CompTick();
            _ageTicks++;        //TODO make age increase less frequent
            if(_ageTicks>Props.reinforcementsDelay)
            {

            }
        }

        public CompReinforcementsSummoner() : base()
        {
            _ageTicks = 0;
        }

        private void CallReinforcements()
        {
            
        }

    }

    public class CompProperties_ReinforcementsSummoner : CompProperties
    {
        public int reinforcementsDelay=1500;


        public CompProperties_ReinforcementsSummoner() => this.compClass = typeof(CompReinforcementsSummoner);
    }

}
