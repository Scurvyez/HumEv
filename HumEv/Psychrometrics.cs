namespace HumEv
{
    public static class Psychrometrics
    {
        // --- Tetens Equation ---
        private const float TetCoeffSlopeConst = 17.27f;
        private const float TetCoeffOffset = 237.3f;
        
        // --- Wet Bulb ---
        private const float PsychroCoeff = 0.00066f;
        private const float SatVaporPress = 0.6108f;
        private const float WetBulbConvergeTol = 0.001f;
        private const float WetBulbDampingFactor = 0.1f;
        
        public static float TetensEqSatVapPress(float t)
        {
            return SatVaporPress * MathF.Exp((TetCoeffSlopeConst * t) / (t + TetCoeffOffset));
        }
        
        public static float GetWetBulbTemperature(float dryBulbC, float relativeHumidity, 
            float pressureKPa, int maxIterations)
        {
            float psychrometricConstant = PsychroCoeff * pressureKPa;
            float vaporPressureAir = relativeHumidity * TetensEqSatVapPress(dryBulbC);
            float wetBulbC = dryBulbC;
            
            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                float saturationVaporPressure = TetensEqSatVapPress(wetBulbC);
                float functionResidual = saturationVaporPressure - psychrometricConstant
                    * (dryBulbC - wetBulbC) - vaporPressureAir;
                
                if (MathF.Abs(functionResidual) < WetBulbConvergeTol)
                    break;
                
                wetBulbC -= functionResidual * WetBulbDampingFactor;
            }
            return wetBulbC;
        }
    }
}