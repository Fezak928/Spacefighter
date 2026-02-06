using UnityEngine;

public static class PixelPerfectClamp
{
    public static Vector2 ClampVector2ToPixelUnit(Vector2 location, int pixelsPerUnit)
    {
        float pixelUnitX = Mathf.RoundToInt(location.x * pixelsPerUnit);
        float pixelUnitY = Mathf.RoundToInt(location.y * pixelsPerUnit);

        Vector2 vectorInPixels = new Vector2(pixelUnitX, pixelUnitY);

        return vectorInPixels / pixelsPerUnit;
    }

    public static Vector3 ClampVector3ToPixelUnit(Vector3 location, int pixelsPerUnit)
    {
        float pixelUnitX = Mathf.RoundToInt(location.x * pixelsPerUnit);
        float pixelUnitY = Mathf.RoundToInt(location.y * pixelsPerUnit);
        float pixelUnitZ = Mathf.RoundToInt(location.z * pixelsPerUnit);

        Vector3 vectorInPixels = new Vector3(pixelUnitX, pixelUnitY, pixelUnitZ);

        return vectorInPixels / pixelsPerUnit;
    }
}
