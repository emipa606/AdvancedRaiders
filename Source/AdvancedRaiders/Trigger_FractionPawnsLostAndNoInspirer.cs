
using Verse.AI.Group;

namespace AdvancedRaiders
{
    public class Trigger_FractionPawnsLostAndNoInspirer : Trigger
    {
        private float fraction = 0.5f;
        public Trigger_FractionPawnsLostAndNoInspirer(float fraction) => this.fraction = fraction;

        public override bool ActivateOn(Lord lord, TriggerSignal signal)
        {
            bool originalTriggerWouldBeActivated = 
                signal.type == TriggerSignalType.PawnLost && (double)lord.numPawnsLostViolently >= (double)lord.numPawnsEverGained * (double)this.fraction;          //Ctrl+C Ctrl+V from original method
            return originalTriggerWouldBeActivated && !lord.OwnsAnyInspirers();
        }
    }
}

