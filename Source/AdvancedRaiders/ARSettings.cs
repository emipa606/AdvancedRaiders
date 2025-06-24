using System;
using UnityEngine;
using Verse;

namespace AdvancedRaiders;

public class ARSettings : ModSettings
{
    public static float TurretBugChance = 0.5f;

    public static float MaxColonistsTauntedCoef = 0.4f;

    public static bool AllowPacification = true;
    public static float PtsdPerHit = 0.1f;
    public static float WitnessedPacificationPerHitChance = 0.5f;
    public static float WitnessedPacificationRadius = 15f;
    public static float PtsdMultiplier = 1f;

    public static float SoreThroatMultiplier = 1f;
    public static float InspirationMultiplier = 1f;

    public static bool AllowMercMedics = true;
    public static bool AllowTechnicians = true;
    public static bool AllowPacifiers = true;
    public static bool AllowBulldozers = true;
    public static bool AllowTribalMedics = true;
    public static bool AllowBeastmasters = true;
    public static bool AllowCommanderChiefs = true;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref TurretBugChance, "turretBugChance", 0.5f);

        Scribe_Values.Look(ref MaxColonistsTauntedCoef, "maxColonistsTauntedCoef", 0.4f);

        Scribe_Values.Look(ref AllowPacification, "allowPacification", true);
        Scribe_Values.Look(ref PtsdPerHit, "ptsdPerHit", 0.1f);
        Scribe_Values.Look(ref WitnessedPacificationPerHitChance, "witnessedPacificationPerHitChance", 0.5f);
        Scribe_Values.Look(ref WitnessedPacificationRadius, "witnessedPacificationRadius", 10f);
        Scribe_Values.Look(ref PtsdMultiplier, "ptsdMultipiler", 1f);

        Scribe_Values.Look(ref SoreThroatMultiplier, "soreThroatMultipiler", 1f);
        Scribe_Values.Look(ref InspirationMultiplier, "inspirationMultipiler", 1f);

        Scribe_Values.Look(ref AllowMercMedics, "allowMercMedics", true);
        Scribe_Values.Look(ref AllowTechnicians, "allowPacifiers", true);
        Scribe_Values.Look(ref AllowPacifiers, "allowPacifiers", true);
        Scribe_Values.Look(ref AllowBulldozers, "allowBulldozers", true);
        Scribe_Values.Look(ref AllowTribalMedics, "allowTribalMedics", true);
        Scribe_Values.Look(ref AllowBeastmasters, "allowBeastmasters", true);
        Scribe_Values.Look(ref AllowCommanderChiefs, "allowCommanderChiefs", true);
    }

    public static void DoSettingsWindowContents(Rect inRect)
    {
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(inRect);

        listingStandard.Label("AdRa.turretBugChance".Translate(TurretBugChance.ToStringPercent()));
        TurretBugChance = listingStandard.Slider(TurretBugChance, 0f, 1f);

        listingStandard.Label("AdRa.maxColonistsTauntedCoef".Translate(MaxColonistsTauntedCoef.ToStringPercent()));
        MaxColonistsTauntedCoef = listingStandard.Slider(MaxColonistsTauntedCoef, 0f, 1f);

        listingStandard.Label("AdRa.ptsdPerHit".Translate(Math.Round(PtsdPerHit, 2)));
        PtsdPerHit = listingStandard.Slider(PtsdPerHit, 0f, 1f);

        listingStandard.Label(
            "AdRa.witnessedPacificationPerHitChance".Translate(WitnessedPacificationPerHitChance.ToStringPercent()));
        WitnessedPacificationPerHitChance = listingStandard.Slider(WitnessedPacificationPerHitChance, 0f, 1f);

        listingStandard.Label(
            "AdRa.witnessedPacificationPerHitChance".Translate(Math.Round(WitnessedPacificationRadius)));
        WitnessedPacificationRadius = listingStandard.Slider(WitnessedPacificationRadius, 1f, 100f);

        listingStandard.Label("AdRa.ptsdMultipiler".Translate(PtsdMultiplier.ToStringPercent()));
        PtsdMultiplier = listingStandard.Slider(PtsdMultiplier, 0f, 2f);

        listingStandard.Label("AdRa.inspirationMultipiler".Translate(InspirationMultiplier.ToStringPercent()));
        InspirationMultiplier = listingStandard.Slider(InspirationMultiplier, 0f, 5f);

        listingStandard.Label("AdRa.soreThroatMultipiler".Translate(Math.Round(SoreThroatMultiplier, 2)));
        SoreThroatMultiplier = listingStandard.Slider(SoreThroatMultiplier, 0f, 5f);

        listingStandard.CheckboxLabeled("AdRa.allowPacification".Translate(), ref AllowPacification);
        listingStandard.CheckboxLabeled("AdRa.allowMercMedics".Translate(), ref AllowMercMedics);
        listingStandard.CheckboxLabeled("AdRa.allowTechnicians".Translate(), ref AllowTechnicians);
        listingStandard.CheckboxLabeled("AdRa.allowPacifiers".Translate(), ref AllowPacifiers);
        listingStandard.CheckboxLabeled("AdRa.allowBulldozers".Translate(), ref AllowBulldozers);
        listingStandard.CheckboxLabeled("AdRa.allowTribalMedics".Translate(), ref AllowTribalMedics);
        listingStandard.CheckboxLabeled("AdRa.allowBeastmasters".Translate(), ref AllowBeastmasters);
        listingStandard.CheckboxLabeled("AdRa.allowCommanderChiefs".Translate(), ref AllowCommanderChiefs);
        if (AdvancedRaiders.CurrentVersion != null)
        {
            GUI.contentColor = Color.gray;
            listingStandard.Label("AdRa.modVersion".Translate(AdvancedRaiders.CurrentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
    }
}