using UnityEngine;
using System.Collections;

internal class Celestial
{
    public GameObject obj;
    public int objNum;
    public Vector3 startPos;
    public Vector3 endPos;
    public Vector3 controlPoint;
    public bool isLaunching = false; // prevents overlapping launches

    public Vector3 QuadraticBezier(Vector3 a, Vector3 control, Vector3 b, float t)
    {
        Vector3 p0 = Vector3.Lerp(a, control, t);
        Vector3 p1 = Vector3.Lerp(control, b, t);
        return Vector3.Lerp(p0, p1, t);
    }
}