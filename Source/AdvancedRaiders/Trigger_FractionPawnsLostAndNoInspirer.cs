using Verse.AI.Group;

namespace AdvancedRaiders;

public class Trigger_FractionPawnsLostAndNoInspirer : Trigger
{
    private readonly float fraction;

    public Trigger_FractionPawnsLostAndNoInspirer(float fraction)
    {
        this.fraction = fraction;
    }

    public override bool ActivateOn(Lord lord, TriggerSignal signal)
    {
        var originalTriggerWouldBeActivated =
            signal.type == TriggerSignalType.PawnLost &&
            lord.numPawnsLostViolently >=
            lord.numPawnsEverGained * (double)fraction; //Ctrl+C Ctrl+V from original method
        return originalTriggerWouldBeActivated && !lord.OwnsAnyInspirers();
    }
}