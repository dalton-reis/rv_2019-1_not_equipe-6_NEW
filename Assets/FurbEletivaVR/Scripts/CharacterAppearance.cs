
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CharacterModel
{
    public string Name;
    public int MeshRendererIndex;
    public List<Material> Materials;
}

public class CharacterAppearance : MonoBehaviour
{
    [Header("Configuration")]
    public GameObject Body;
    public int ModelIndex = 0;
    public int MaterialIndex = 0;

    [Header("Data")]
    public int MeshAmount = 14;
    public List<CharacterModel> CharacterModels;

    private void Start()
    {
        SetupModel();
    }

    public void SetupModel()
    {
        for (int i = 0; i < MeshAmount; i++) // Deactivate every model
            Body.transform.GetChild(i).gameObject.SetActive(false);

        var selectedModel = CharacterModels[ModelIndex];

        var modelGameObject = Body.transform.GetChild(selectedModel.MeshRendererIndex).gameObject;
        // Activate the selected model game object
        modelGameObject.SetActive(true);
        // And setup the selected material
        modelGameObject.GetComponent<SkinnedMeshRenderer>().material = selectedModel.Materials[MaterialIndex];
    }
}
