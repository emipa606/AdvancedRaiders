using HarmonyLib;
using Mlie;
using UnityEngine;
using Verse;

namespace AdvancedRaiders;

[StaticConstructorOnStartup]
public class AdvancedRaiders : Mod
{
    public static ARSettings settings;
    public static Harmony harmonyInstance;
    public static string currentVersion;

    public AdvancedRaiders(ModContentPack content) : base(content)
    {
        GetSettings<ARSettings>().Write();
        harmonyInstance = new Harmony("saloid.AdvancedRaiders");
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(ModLister.GetActiveModWithIdentifier("Mlie.AdvancedRaiders"));
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        ARSettings.DoSettingsWindowContents(inRect);
        base.DoSettingsWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "Advanced raiders";
    }
}