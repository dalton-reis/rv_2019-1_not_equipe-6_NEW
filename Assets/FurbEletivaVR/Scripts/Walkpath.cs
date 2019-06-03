
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Walkpath : MonoBehaviour
{
    public List<Transform> PointsToWalk;
    public GameObject StreetSign;

    [Header("Next Waypaths")]
    public Walkpath LeftWaypath;
    public Walkpath RightWaypath;

    public Walkpath DefaultWalkpath;

    [Header("Events")]
    public UnityEvent LeftWasSelected = new UnityEvent();
    public UnityEvent RightWasSelected = new UnityEvent();

    private int current_path_point_index = 0;

    private bool endWasTriggered = false;
    private bool readyToTriggerEnd = false;

    public Vector3? CurrentPathPoint
    {
        get
        {
            if (current_path_point_index >= PointsToWalk.Count)
                return null;

            if (current_path_point_index == PointsToWalk.Count - 1)
                readyToTriggerEnd = true;

            return PointsToWalk[current_path_point_index].position;
        }
    }

    public void NextPoint()
    {
        current_path_point_index++;
    }

    public void OnLeftWasSelected()
    {
        LeftWasSelected.Invoke();
    }

    public void OnRightWasSelected()
    {
        RightWasSelected.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!readyToTriggerEnd)
            return;

        if (endWasTriggered)
            return;

        var aiFollowPathSimplePeople = other.GetComponent<AIFollowPathSimplePeople>();

        if (aiFollowPathSimplePeople == null)
            return;

        endWasTriggered = true;

        aiFollowPathSimplePeople.ReachedWaypathEnd();
    }
}
