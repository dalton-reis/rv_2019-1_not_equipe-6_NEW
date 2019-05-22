
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class QuestionCanvasResult : MonoBehaviour
{
    public Image okImage;
    public Image failImage;

    public AudioClip okAudio;
    public AudioClip failAudio;

    private AudioSource audioSource;

    private void Awake()
    {
        okImage.enabled = false;
        failImage.enabled = false;

        audioSource = GetComponent<AudioSource>();
    }

    public void Answer(bool isOk)
    {
        if (isOk)
        { 
            okImage.enabled = true;
            audioSource.clip = okAudio;
        }
        else
        {
            failImage.enabled = true;
            audioSource.clip = failAudio;
        }

        audioSource.Play();

        Destroy(gameObject, 1f);
    }
}
