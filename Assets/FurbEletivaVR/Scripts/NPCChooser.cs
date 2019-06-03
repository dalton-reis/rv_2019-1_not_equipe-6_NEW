using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCChooser : MonoBehaviour
{
    public List<GameObject> NPCPaths1;
    public List<GameObject> NPCPaths2;
    public List<GameObject> NPCPaths3;
    public List<GameObject> NPCPaths4;

    private List<List<GameObject>> NPCPaths;

    private void Start()
    {
        NPCPaths = new List<List<GameObject>>();
        NPCPaths.Add(NPCPaths1);
        NPCPaths.Add(NPCPaths2);
        NPCPaths.Add(NPCPaths3);
        NPCPaths.Add(NPCPaths4);

        var selectedPath = NPCPaths[Random.Range(0, NPCPaths.Count)];

        foreach (var go in selectedPath)
            go.SetActive(true);
    }
}
