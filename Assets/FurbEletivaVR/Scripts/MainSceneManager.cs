
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainSceneManager : MonoBehaviour
{
    public CharacterAppearance CharacterAppearance;
    public TMP_Text ModelNameText;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        UpdateCharacterAppearance();
    }

    public void ChangeToCityScene()
    {
        ResetGameData();

        SceneManager.LoadScene("City", LoadSceneMode.Single);
    }

    private static void ResetGameData()
    {
        var gameManager = GameManager.Instance;
        gameManager.Ok = 0;
        gameManager.Total = 0;
    }

    public void PreviousModel()
    {
        var modelIndex = gameManager.SelectedModel;
        modelIndex--;

        if (modelIndex < 0)
            modelIndex = CharacterAppearance.CharacterModels.Count - 1;

        gameManager.SelectedModel = modelIndex;
        UpdateCharacterAppearance();
    }

    public void NextModel()
    {
        var modelIndex = gameManager.SelectedModel;
        modelIndex++;

        if (modelIndex >= CharacterAppearance.CharacterModels.Count)
            modelIndex = 0;

        gameManager.SelectedModel = modelIndex;
        UpdateCharacterAppearance();
    }

    public void PreviousMaterial()
    {
        var materialIndex = gameManager.SelectedModelMaterial;
        materialIndex--;

        if (materialIndex < 0)
            materialIndex = CharacterAppearance.CharacterModels[gameManager.SelectedModel].Materials.Count - 1;

        gameManager.SelectedModelMaterial = materialIndex;
        UpdateCharacterAppearance();
    }

    public void NextMaterial()
    {
        var materialIndex = gameManager.SelectedModelMaterial;
        materialIndex++;

        if (materialIndex >= CharacterAppearance.CharacterModels[gameManager.SelectedModel].Materials.Count)
            materialIndex = 0;

        gameManager.SelectedModelMaterial = materialIndex;
        UpdateCharacterAppearance();
    }

    private void UpdateCharacterAppearance()
    {
        int selectedModel = gameManager.SelectedModel;

        CharacterAppearance.ModelIndex = selectedModel;
        CharacterAppearance.MaterialIndex = gameManager.SelectedModelMaterial;
        CharacterAppearance.SetupModel();

        ModelNameText.text = CharacterAppearance.CharacterModels[selectedModel].Name;
    }
}
