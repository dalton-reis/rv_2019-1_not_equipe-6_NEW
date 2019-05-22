
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(SimplePeopleCharacter))]
public class AIFollowPersonSimplePeople : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
    public SimplePeopleCharacter character { get; private set; } // the character we are controlling

    public SimplePeopleCharacter CharacterToFollow;

    public GameObject QuestionCanvas;

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
        if (CharacterToFollow != null)
            agent.SetDestination(CharacterToFollow.transform.position);

        if ((agent.destination - gameObject.transform.position).magnitude > agent.stoppingDistance)
            character.Move(agent.desiredVelocity, false, false);
        else
            character.Move(Vector3.zero, false, false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CharacterToFollow != null)
            return; // We are already following a character

        var characterToFollow = other.GetComponent<SimplePeopleCharacter>();

        if (characterToFollow == null)
            return; // Not a SimplePeopleCharacter

        var aIFollowPathSimplePeople = other.GetComponent<AIFollowPathSimplePeople>();
        if (aIFollowPathSimplePeople == null)
            return;

        aIFollowPathSimplePeople.Stop = true;
        QuestionCanvas.SetActive(true);
        GetComponent<NPCQuestion>().StartQuestion();

        other.GetComponent<LookAt>().m_Target = transform;
    }
}
