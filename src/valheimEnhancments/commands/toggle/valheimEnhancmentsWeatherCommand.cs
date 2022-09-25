using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace valheimEnhancments.commands.toggle
{
    public class valheimEnhancmentsWeatherCommand : valheimEnhancmentsToogleCommand
    {
        public override string Name => "noweather";
        public override string Description => "Removes all weather effects";
        public override bool GetToggleValue() => valheimEnhancmentsWeatherCommand.NoWeatherEffects;
        public override void SetToggleValue(bool newValue)
        {
            valheimEnhancmentsWeatherCommand.NoWeatherEffects = newValue;

            foreach (var currentBiome in Enum.GetValues(typeof(Heightmap.Biome)) as Heightmap.Biome[])
            {
                ZLog.Log("getting for biome " + currentBiome);

                var available = AccessTools.Method(typeof(EnvMan), "GetAvailableEnvironments").Invoke(EnvMan.instance, new object[]
                {
                    currentBiome
                }) as List<EnvEntry>;

                if (available != null)
                    ZLog.Log($"{currentBiome}:{string.Join("|", available.Select(f => f.m_environment))}");
            }

        }

        public static bool NoWeatherEffects { get; set; }

        [HarmonyPatch(typeof(EnvMan), "IsFreezing")]
        private static class valheimEnhancmentsEnvManIsFreezingModification
        {
            private static bool Prefix(ref bool __result)
            {
                if (valheimEnhancmentsWeatherCommand.NoWeatherEffects == false)
                    return true;

                __result = false;
                return false;
            }
        }

        [HarmonyPatch(typeof(EnvMan), "IsCold")]
        private static class valheimEnhancmentsEnvManIsColdModification
        {
            private static bool Prefix(ref bool __result)
            {
                if (valheimEnhancmentsWeatherCommand.NoWeatherEffects == false)
                    return true;

                __result = false;
                return false;
            }
        }

        [HarmonyPatch(typeof(EnvMan), "IsWet")]
        private static class valheimEnhancmentsEnvManIsWetModification
        {
            private static bool Prefix(ref bool __result)
            {
                if (valheimEnhancmentsWeatherCommand.NoWeatherEffects == false)
                    return true;

                __result = false;
                return false;
            }
        }
    }
}
