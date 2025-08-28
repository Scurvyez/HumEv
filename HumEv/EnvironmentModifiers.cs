namespace HumEv
{
    public static class EnvironmentModifiers
    {
        // --- Weather ---
        private const float RainHumidityBoost = 0.3f;
        private const float RainTempDrop = 2f;

        private const float ThunderstormHumidityBoost = 0.4f;
        private const float ThunderstormTempDrop = 4f;

        private const float DrySpellHumidityDrop = 0.2f;
        private const float DrySpellTempRise = 3f;

        // --- Topography ---
        private const float ValleyTempDrop = 1.5f;
        private const float ValleyHumidityBoost = 0.05f;

        private const float MountainTempDrop = 2.0f;
        private const float MountainHumidityDrop = 0.1f;

        private const float CoastalTempDrop = 0.5f;
        private const float CoastalHumidityBoost = 0.1f;

        // --- Water Proximity ---
        private const float WaterBaselineTemp = 15f;
        private const float WaterTempModerationFactor = 0.3f;
        private const float WaterHumidityBoostFactor = 0.25f;

        // --- Vegetation Coverage ---
        private const float VegetationTempModerationMax = 2.0f;
        private const float VegetationComfortTemp = 18f;
        private const float VegetationHumidityBoostFactor = 0.15f;
        private const float VegetationWarmSideFactor = 0.5f;
        
        // --- Latitude ---
        private const float MinMaxLatitude = 90f;
        
        // --- Seasonal ---
        private const int DaysInYear = 365;
        private const float MaxLatTempSwingAtPolesC = 15f;
        private const float SeasonalCycleFactor = 2f;
        private const int SeasonalSolsticePhaseShift = 80;
        private const float SeasonalHumidityAdj = 0.05f;
        
        // --- Day/Night ---
        private const float DayNightTempAmplitude = 6.0f;
        private const float DayNightHumidityAmplitude = 0.1f;
        
        public static void ApplyWeatherMods(WeatherType weather,  
            ref float dryBulbC, ref float relHumidity)
        {
            switch (weather)
            {
                case WeatherType.Rain:
                    relHumidity = Math.Min(1.0f, relHumidity + RainHumidityBoost);
                    dryBulbC -= RainTempDrop;
                    break;
                case WeatherType.Thunderstorms:
                    relHumidity = Math.Min(1.0f, relHumidity + ThunderstormHumidityBoost);
                    dryBulbC -= ThunderstormTempDrop;
                    break;
                case WeatherType.DrySpell:
                    relHumidity = Math.Max(0.1f, relHumidity - DrySpellHumidityDrop);
                    dryBulbC += DrySpellTempRise;
                    break;
                case WeatherType.Clear:
                    break;
            }
        }
        
        public static void ApplyTopographyMods(TopographyType topo, 
            ref float dryBulbC, ref float relHumidity)
        {
            switch (topo)
            {
                case TopographyType.Valley:
                    relHumidity = Math.Min(1.0f, relHumidity + ValleyHumidityBoost);
                    dryBulbC -= ValleyTempDrop;
                    break;
                case TopographyType.Mountain:
                    relHumidity = Math.Max(0.1f, relHumidity - MountainHumidityDrop);
                    dryBulbC -= MountainTempDrop;
                    break;
                case TopographyType.Coastal:
                    relHumidity = Math.Min(1.0f, relHumidity + CoastalHumidityBoost);
                    dryBulbC -= CoastalTempDrop;
                    break;
                case TopographyType.Plains:
                    break;
            }
        }
        
        public static void ApplyWaterProximityMods(float proximityScore,
            ref float dryBulbC, ref float relHumidity)
        {
            proximityScore = Math.Clamp(proximityScore, 0f, 1f);
            
            float waterBaseline = WaterBaselineTemp; 
            float tempDifference = dryBulbC - waterBaseline;
            
            dryBulbC -= tempDifference * WaterTempModerationFactor * proximityScore;
            
            float humidityBoost = WaterHumidityBoostFactor * proximityScore * (1f - relHumidity);
            relHumidity = Math.Min(1f, relHumidity + humidityBoost);
        }
        
        public static void ApplyVegetationCoverageMods(float vegetationScore,
            ref float dryBulbC, ref float relHumidity)
        {
            vegetationScore = Math.Clamp(vegetationScore, 0f, 1f);
            
            float tempModeration = VegetationTempModerationMax * vegetationScore;
            float avgComfortTemp = VegetationComfortTemp;
            
            if (dryBulbC > avgComfortTemp)
                dryBulbC -= tempModeration;
            else
                dryBulbC += tempModeration * VegetationWarmSideFactor;
            
            float humidityBoost = VegetationHumidityBoostFactor * vegetationScore * (1f - relHumidity);
            relHumidity = Math.Min(1f, relHumidity + humidityBoost);
        }
        
        public static void ApplyLatitudeSeasonMods(float latitudeDeg, int dayOfYear,
            ref float dryBulbC, ref float relHumidity)
        {
            latitudeDeg = Math.Clamp(latitudeDeg, -MinMaxLatitude, MinMaxLatitude);
            dayOfYear = Math.Clamp(dayOfYear, 1, DaysInYear);
            
            float seasonalFactor = MathF.Sin(SeasonalCycleFactor 
                * MathF.PI * (dayOfYear - SeasonalSolsticePhaseShift) / DaysInYear);
            
            float tempAdjustment = MaxLatTempSwingAtPolesC 
                                   * (MathF.Abs(latitudeDeg) / MinMaxLatitude) * seasonalFactor;
            dryBulbC += tempAdjustment;
            
            float humidityAdjustment = SeasonalHumidityAdj * seasonalFactor;
            relHumidity = Math.Clamp(relHumidity + humidityAdjustment, 0f, 1f);
        }
        
        public static void ApplyDayNightCycleMods(float timeOfDayNorm, 
            ref float dryBulbC, ref float relHumidity)
        {
            timeOfDayNorm = Math.Clamp(timeOfDayNorm, 0f, 1f);
            
            float angle = timeOfDayNorm * 2f * MathF.PI;
            float tempOffset = DayNightTempAmplitude * MathF.Sin(angle - MathF.PI / 2f);
            dryBulbC += tempOffset;
            
            float humidityOffset = -DayNightHumidityAmplitude * MathF.Sin(angle - MathF.PI / 2f);
            relHumidity = Math.Clamp(relHumidity + humidityOffset, 0f, 1f);
        }
    }
}