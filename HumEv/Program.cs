namespace HumEv
{
    internal class Program()
    {
        private static void Main(string[] args)
        {
            float elevationM = 1598f;
            float latitudeDeg = 47.9704f;
            float waterProx = 0.7f;
            float vegScore = 0.65f;
            WeatherType weather = WeatherType.Thunderstorms;
            TopographyType topo = TopographyType.Mountain;
            int maxIterations = 50;
            
            float outdoorDryBulbC = UnitConverter.FahrenheitToCelsius(75f);
            float outdoorRelHumidity = 0.28f;
            
            EnvironmentModifiers.ApplyWeatherMods(weather, ref outdoorDryBulbC, ref outdoorRelHumidity);
            EnvironmentModifiers.ApplyTopographyMods(topo, ref outdoorDryBulbC, ref outdoorRelHumidity);
            EnvironmentModifiers.ApplyWaterProximityMods(waterProx, ref outdoorDryBulbC, ref outdoorRelHumidity);
            EnvironmentModifiers.ApplyVegetationCoverageMods(vegScore, ref outdoorDryBulbC, ref outdoorRelHumidity);
            
            float localPressure = Atmospherics.PressureAtElevation(elevationM);
            float indoorDryBulbC = UnitConverter.FahrenheitToCelsius(65f);
            
            float outdoorWetBulbC = Psychrometrics.GetWetBulbTemperature(outdoorDryBulbC, outdoorRelHumidity, 
                localPressure, maxIterations);
            
            float outdoorVaporPressure = outdoorRelHumidity * Psychrometrics.TetensEqSatVapPress(outdoorDryBulbC);
            float indoorRelHumidity = outdoorVaporPressure / Psychrometrics.TetensEqSatVapPress(indoorDryBulbC);
            
            float indoorWetBulbC = Psychrometrics.GetWetBulbTemperature(indoorDryBulbC, indoorRelHumidity, 
                localPressure, maxIterations);
            
            DisplayResults(elevationM, latitudeDeg, weather, topo,
                outdoorDryBulbC, outdoorRelHumidity, localPressure, outdoorWetBulbC,
                indoorDryBulbC, indoorRelHumidity, indoorWetBulbC);
        }
        
        private static void DisplayResults(float elevationM, float latitudeDeg, WeatherType weather, 
            TopographyType topo, float outdoorDryBulbC, float outdoorRelHumidity, float localPressure,
            float outdoorWetBulbC, float indoorDryBulbC, float indoorRelHumidity, float indoorWetBulbC)
        {
            Console.WriteLine("=== HumEv Indoor/Outdoor Wet Bulb Test ===\n");
            Console.WriteLine($"Elevation: {elevationM} m, Latitude: {latitudeDeg}°");
            Console.WriteLine($"Weather: {weather}");
            Console.WriteLine($"Topology: {topo}\n");
            Console.WriteLine($"Outdoor Conditions: {outdoorDryBulbC:F1} °C, " +
                              $"RH {outdoorRelHumidity * 100:F0}%, Pressure {localPressure:F1} kPa");
            Console.WriteLine($"Calculated Outdoor Wet Bulb: {outdoorWetBulbC:F2} °C\n");
            Console.WriteLine($"Indoor Conditions: {indoorDryBulbC:F1} °C, RH {indoorRelHumidity * 100:F0}%");
            Console.WriteLine($"Calculated Indoor Wet Bulb: {indoorWetBulbC:F2} °C\n");
        }
    }
}