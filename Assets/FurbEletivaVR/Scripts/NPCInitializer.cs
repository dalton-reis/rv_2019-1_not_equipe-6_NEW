
using UnityEngine;

public class NPCInitializer : MonoBehaviour
{
    public CharacterAppearance CharacterAppearance;
    public NPCQuestion NPCQuestion;

    private void Awake()
    {
        if (CharacterAppearance == null)
            CharacterAppearance = GetComponentInChildren<CharacterAppearance>();

        if (NPCQuestion == null)
            NPCQuestion = GetComponentInChildren<NPCQuestion>();
    }

    private void Start()
    {
        // Random Appearance
        RandomizeCharacterAppearance();

        // Random Question
        NPCQuestion.question = QuestionDatabase.Instance.GetQuestion();
    }

    private void RandomizeCharacterAppearance()
    {
        int modelCount = CharacterAppearance.CharacterModels.Count;
        if (modelCount > 0)
        {
            int selectedModelIndex = Random.Range(0, modelCount);

            int materialCount = CharacterAppearance.CharacterModels[selectedModelIndex].Materials.Count;
            if (materialCount > 0)
            {
                CharacterAppearance.ModelIndex = selectedModelIndex;
                CharacterAppearance.MaterialIndex = Random.Range(0, materialCount);

                CharacterAppearance.SetupModel();
            }
        }
    }
}
