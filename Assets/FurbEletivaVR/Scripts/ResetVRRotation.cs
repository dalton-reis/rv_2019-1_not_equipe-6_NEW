
using UnityEngine;

public class ResetVRRotation : MonoBehaviour
{
    void Update()
    {
        ResetVR();
    }

    void ResetVR()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
