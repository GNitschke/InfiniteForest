using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexMetrics
{
    public const float outerRadius = 15f;
    public const float innerRadius = outerRadius * 0.866025404f;

    public static Vector3[] corners =
    {
        new Vector3(0f, 0f, outerRadius),
        new Vector3(innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(0f, 0f, -outerRadius),
        new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(-innerRadius, 0f, 0.5f * outerRadius)
    };

    public static Vector3[] neighborPos =
    {
        new Vector3(innerRadius, 0f, outerRadius * 1.5f),
        new Vector3(innerRadius * 2f, 0f, 0f),
        new Vector3(innerRadius, 0f, outerRadius * -1.5f),
        new Vector3(-innerRadius, 0f, outerRadius * -1.5f),
        new Vector3(innerRadius * -2f, 0f, 0f),
        new Vector3(-innerRadius, 0f, outerRadius * 1.5f)
    };
}
