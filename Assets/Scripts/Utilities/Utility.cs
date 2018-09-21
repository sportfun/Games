public class Utility
{
    static public float ScaleValue(float value, float newMin, float newMax, float oldMin = 0.0f, float oldMax = 1.0f)
    {
        return ((newMax - newMin) / (oldMax - oldMin) * (value - oldMax) + newMax);
    }
}
