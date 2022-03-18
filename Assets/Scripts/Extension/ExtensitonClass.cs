
using UnityEngine;


public static class UtilClass
{


    public static void Reset(this LineRenderer lr)
    {
        lr.positionCount = 0;
    }

    public static void Reset(this LineRenderer lr, int count)
    {
        lr.positionCount = count;
    }

    public static Vector3 GetVectorFromAngle2D(float angle)
    {
        // angle = 0 -> 360
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public static Vector3 GetVectorFromAngle(float angle)
    {
        // angle = 0 -> 360
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Sin(angleRad), 0, Mathf.Cos(angleRad));
    }

    public static float GetAngleFromVector2D(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
       // int angle = Mathf.RoundToInt(n);

        return n;
    }

    public static float GetAngleFromVector(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2( dir.x, dir.z) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        //int angle = Mathf.RoundToInt(n);

        return n;
    }

    public static float GetAngleFromTwoPoint(Vector3 pointA, Vector3 pointB)
    {
        var dir = pointB - pointA;
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        //int angle = Mathf.RoundToInt(n);

        return n;
    }

    public static void LookAtToOnlyRotateOnYAxis(Transform originPos, Vector3 targetPos, float speed) {
        var lookPos = originPos.position - targetPos;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        originPos.rotation = Quaternion.Slerp(originPos.rotation, rotation, Time.deltaTime * speed);
    }
}