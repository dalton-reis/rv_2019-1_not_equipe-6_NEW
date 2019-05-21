
using UnityEngine;
using UnityEngine.SceneManagement;

public class CityManager : MonoBehaviour
{
    public void ChangeToMainScene()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
