using UnityEngine;

public class Math : MonoBehaviour
{
    public static bool LineLineIntersection(Vector3 linePoint1,
         Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
    {

        Vector3 lineVec3 = linePoint2 - linePoint1;
        Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
        Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

        float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

        //is coplanar, and not parallel
        if (Mathf.Abs(planarFactor) < 0.0001f
                && crossVec1and2.sqrMagnitude > 0.0001f)
        {
            float s = Vector3.Dot(crossVec3and2, crossVec1and2)
                    / crossVec1and2.sqrMagnitude;
            return true;
        }
        else
        {
            return false;
        }
    }

    bool IntersectRayTriangle(in Ray ray, in Vector3 v0, in Vector3 v1, in Vector3 v2, out Vector3 IntersectionPoint)
    {
        IntersectionPoint = Vector3.zero;

        Vector3 rayOrigin = ray.origin;
        Vector3 rayVector = ray.direction;

        const float EPSILON = 0.0000001f;
        Vector3 edge1, edge2, h, s, q;
        float a, f, u, v;

        edge1 = v1 - v0;
        edge2 = v2 - v0;
        h = Vector3.Cross(rayVector, edge2);
        a = Vector3.Dot(edge1, h);
        if (a > -EPSILON && a < EPSILON)
            return false;    // This ray is parallel to this triangle.
        f = 1.0f / a;
        s = rayOrigin - v0;
        u = f * Vector3.Dot(s, h);
        if (u < 0.0f || u > 1.0f)
            return false;
        q = Vector3.Cross(s, edge1);
        v = f * Vector3.Dot(rayVector, q);
        if (v < 0.0f || u + v > 1.0f)
            return false;
        // At this stage we can compute t to find out where the intersection point is on the line.
        float t = f * Vector3.Dot(edge2, q);
        if (t > EPSILON) // ray intersection
        {
            IntersectionPoint = rayOrigin + rayVector * t;
            return true;
        }
        else // This means that there is a line intersection but not a ray intersection.
            return false;
    }
}
