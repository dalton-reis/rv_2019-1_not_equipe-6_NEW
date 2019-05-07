using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(SimplePeopleCharacter))]
public class AISimplePeople : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
    public SimplePeopleCharacter character { get; private set; } // the character we are controlling


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
        if (character.CurrentPathPoint.HasValue)
            agent.SetDestination(character.CurrentPathPoint.Value);


        if ((agent.destination - gameObject.transform.position).magnitude > agent.stoppingDistance)
            character.Move(agent.desiredVelocity, false, false);
        else
        {
            if (character.CurrentPathPoint.HasValue)
                character.NextPoint();
            else
                character.Move(Vector3.zero, false, false);
        }
    }
}
