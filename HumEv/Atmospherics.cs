namespace HumEv
{
    public static class Atmospherics
    {
        // --- Atmospheric ---
        private const float StandardPressureKPa = 101.325f;
        private const float LapseRate = 0.0065f;
        private const float SeaLevelTempK = 288.15f;
        private const float GravityExp = 5.255f;
        
        public static float PressureAtElevation(float elevationM)
        {
            return StandardPressureKPa * MathF.Pow(1 - LapseRate * elevationM / SeaLevelTempK, GravityExp);
        }
    }
}