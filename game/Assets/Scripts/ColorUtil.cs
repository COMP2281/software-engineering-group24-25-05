using UnityEngine;

public class ColorUtil
{
    public static Color BlendColors(Color colorA, Color colorB, float blendFactor)
    {
        float inverseBlend = 1f - blendFactor;

        return new Color(
            colorA.r * inverseBlend + colorB.r * blendFactor,
            colorA.g * inverseBlend + colorB.g * blendFactor,
            colorA.b * inverseBlend + colorB.b * blendFactor,
            colorA.a * inverseBlend + colorB.a * blendFactor
        );
    }
}
