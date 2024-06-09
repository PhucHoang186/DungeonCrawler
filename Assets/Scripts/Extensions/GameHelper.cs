using System.Collections;
using System.Collections.Generic;
using GameUI;
using NaughtyAttributes.Test;
using UnityEngine;

public static class GameHelper
{
    public static Vector3 QuaracticCurve(Vector3 pointA, Vector3 pointB, Vector3 pointC, float t)
    {
        Vector3 pointAB = Vector3.Lerp(pointA, pointB, t);
        Vector3 pointBC = Vector3.Lerp(pointB, pointC, t);
        Vector3 pointAC = Vector3.Lerp(pointAB, pointBC, t);
        return pointAC;
    }

    public static Vector3 CubicCurve(Vector3 pointA, Vector3 pointB, Vector3 pointC, Vector3 pointD, float t)
    {
        Vector3 pointABC = QuaracticCurve(pointA, pointB, pointC, t);
        Vector3 pointBCD = QuaracticCurve(pointB, pointC, pointD, t);
        Vector3 pointABCD = Vector3.Lerp(pointABC, pointBCD, t);
        return pointABCD;
    }

    public static Vector3 ShowBezierCurve(Vector3 startPoint, Vector3 startHandlePoint, Vector3 endHandlePoint, Vector3 endPoint, float t)
    {
        var finalPos = CubicCurve(startPoint, startHandlePoint, endHandlePoint, endPoint, t);
        return finalPos;
    }
}
