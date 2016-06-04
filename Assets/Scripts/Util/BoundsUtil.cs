using UnityEngine;
using System.Collections;
using DanmakU;

public class BoundsUtil
{
	public static Vector2 VerifyBounds(Vector2 objCenter, Bounds2D obj, Bounds2D field)
    {
        Vector2 offset = new Vector2();
        if (obj.Min.x < field.Min.x)
            offset.x += field.Min.x - obj.Min.x;
        else if (obj.Max.x > field.Max.x)
            offset.x += field.Max.x - obj.Max.x;
        if (obj.Min.y < field.Min.y)
            offset.y += field.Min.y - obj.Min.y;
        else if (obj.Max.y > field.Max.y)
            offset.y += field.Max.y - obj.Max.y;
        return objCenter + offset;
    }
}
