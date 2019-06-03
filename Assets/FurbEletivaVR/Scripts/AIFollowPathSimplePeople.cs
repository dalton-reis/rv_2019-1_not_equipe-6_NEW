
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(SimplePeopleCharacter))]
public class AIFollowPathSimplePeople : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
    public SimplePeopleCharacter character { get; private set; } // the character we are controlling
    public Walkpath CurrentPath;

    public AudioSource AudioToPlayWhenYouPressSomething;

    public bool Stop { get; set; }

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

        if (CurrentPath.CurrentPathPoint.HasValue)
            agent.SetDestination(CurrentPath.CurrentPathPoint.Value);

        if ((agent.destination - gameObject.transform.position).magnitude > agent.stoppingDistance)
            character.Move(agent.desiredVelocity, false, false);
        else
        {
            if (CurrentPath.CurrentPathPoint.HasValue)
                CurrentPath.NextPoint();
            else
                character.Move(Vector3.zero, false, false);
        }
    }

    public void ReachedWaypathEnd()
    {
        if (CurrentPath.DefaultWalkpath != null)
        {
            CurrentPath.DefaultWalkpath.LeftWasSelected.RemoveAllListeners();
            CurrentPath.DefaultWalkpath.RightWasSelected.RemoveAllListeners();
            CurrentPath = CurrentPath.DefaultWalkpath;
            return;
        }

        if (CurrentPath.LeftWaypath != null
                && CurrentPath.RightWaypath != null)
        {
            // Active the street sign
            CurrentPath.StreetSign.SetActive(true);

            // Setup events to change the current waypath
            CurrentPath.LeftWasSelected.AddListener(() =>
            {
                AudioToPlayWhenYouPressSomething.Play();
                CurrentPath.StreetSign.SetActive(false);
                CurrentPath = CurrentPath.LeftWaypath;
            });

            CurrentPath.RightWasSelected.AddListener(() =>
            {
                AudioToPlayWhenYouPressSomething.Play();
                CurrentPath.StreetSign.SetActive(false);
                CurrentPath = CurrentPath.RightWaypath;
            });
        }
    }
}
