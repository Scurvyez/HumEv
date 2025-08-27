namespace HumEv
{
    public class UnitConverter
    {
        public static float FahrenheitToCelsius(float tempF)
        {
            return (tempF - 32f) * 5f / 9f;
        }
    }
}