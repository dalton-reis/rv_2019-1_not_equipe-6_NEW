
using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class WalkablePlane : MonoBehaviour
{
    public Transform end;

    private NavMeshSurface navMeshSurface;

    private Vector3 start;

    private void Awake()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
    }

    public void SetFloorSize(float units)
    {
        if (units < 1f)
            throw new InvalidOperationException($"{nameof(units)} must be '>= 1', it is {units}");

        start = end.position;

        // Update the scale
        var scale = transform.localScale;
        scale.z = units / 5f;
        transform.localScale = scale;

        // Update the position based on the scale
        var position = transform.localPosition;
        position.z = (scale.z - 0.2f) * 5f + start.z;
        transform.localPosition = position;

        // Build the nav mesh
        navMeshSurface.BuildNavMesh();

        end.position = start + new Vector3(0, 0, units * 2f - 2f);
    }
}
