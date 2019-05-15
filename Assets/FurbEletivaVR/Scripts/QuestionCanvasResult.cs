
using UnityEngine;
using UnityEngine.UI;

public class QuestionCanvasResult : MonoBehaviour
{
    public Image okImage;
    public Image failImage;

    private void Awake()
    {
        okImage.enabled = false;
        failImage.enabled = false;
    }

    public void Answer(bool isOk)
    {
        if (isOk)
            okImage.enabled = true;
        else
            failImage.enabled = true;

        Destroy(gameObject, 1f);
    }
}
