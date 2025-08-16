using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertPositionHelper : MonoBehaviour
{
    private static Plane plane = new Plane(Vector3.up, Vector3.zero);

    public static Vector3 GetMousePositionOnPlane(Camera camera) => GetWorldPositionOnPlane(camera, Input.mousePosition);

    public static Vector3 GetWorldPositionOnPlane(Camera camera, Vector3 worldPos)
    {
        var ray = camera.ScreenPointToRay(worldPos);
        plane.Raycast(ray, out var distance);
        return ray.GetPoint(distance);
    }
}
