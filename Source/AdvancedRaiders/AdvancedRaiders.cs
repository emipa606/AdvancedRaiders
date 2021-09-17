using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using HarmonyLib;

namespace AdvancedRaiders
{
    public class AdvancedRaiders : Mod
    {
        public static Harmony harmonyInstance;
        public AdvancedRaiders(ModContentPack content) : base(content)
        {
            harmonyInstance = new Harmony("saloid.AdvancedRaiders");
        }
    }
}
