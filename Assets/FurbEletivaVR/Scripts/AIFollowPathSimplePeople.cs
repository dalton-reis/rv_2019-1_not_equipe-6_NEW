using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(SimplePeopleCharacter))]
public class AIFollowPathSimplePeople : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
    public SimplePeopleCharacter character { get; private set; } // the character we are controlling
    public GameObject Path;

    public bool Stop { get; set; }

    private List<Vector3> path = new List<Vector3>();
    private int current_path_point_index = 0;

    public Vector3? CurrentPathPoint
    {
        get
        {
            if (current_path_point_index >= path.Count)
                return null;

            return path[current_path_point_index];
        }
    }

    private void Awake()
    {
        List<Transform> path_points = new List<Transform>();
        Path.GetComponentsInChildren<Transform>(path_points);
        path_points.RemoveAt(0); // The first will be the transform of the 'walkpath'
        foreach (var path_point in path_points)
            path.Add(path_point.position);
    }

    private void Start()
    {
        // get the components on the object we need ( should not be null due to require component so no need to check )
        agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        character = GetComponent<SimplePeopleCharacter>();

        agent.updateRotation = false;
        agent.updatePosition = true;
    }


    private void Update()
    {
        if (Stop)
        {
            agent.ResetPath();
            character.Move(Vector3.zero, false, false);
            return;
        }

        if (CurrentPathPoint.HasValue)
            agent.SetDestination(CurrentPathPoint.Value);


        if ((agent.destination - gameObject.transform.position).magnitude > agent.stoppingDistance)
            character.Move(agent.desiredVelocity, false, false);
        else
        {
            if (CurrentPathPoint.HasValue)
                NextPoint();
            else
                character.Move(Vector3.zero, false, false);
        }
    }

    public void NextPoint()
    {
        current_path_point_index++;
    }
}
