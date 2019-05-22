
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField]
    public Transform m_Target;
    [SerializeField]
    private float m_Speed;


    void Update()
    {
        if (m_Target == null)
            return;

        Vector3 lTargetDir = m_Target.position - transform.position;
        lTargetDir.y = 0.0f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), Time.time * m_Speed);
    }
}