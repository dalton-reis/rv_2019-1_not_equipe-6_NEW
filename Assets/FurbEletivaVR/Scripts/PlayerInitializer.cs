
using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    public CharacterAppearance CharacterAppearance;

    private void Start()
    {
        GameManager instance = GameManager.Instance;
        CharacterAppearance.ModelIndex = instance.SelectedModel;
        CharacterAppearance.MaterialIndex = instance.SelectedModelMaterial;

        CharacterAppearance.SetupModel();
    }
}
