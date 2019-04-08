using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerController : MonoBehaviour
{
    public AICharacterControl characterControl;

    private void Awake()
    {
        if (characterControl == null)
            characterControl = GetComponentInChildren<AICharacterControl>();
    }
}
