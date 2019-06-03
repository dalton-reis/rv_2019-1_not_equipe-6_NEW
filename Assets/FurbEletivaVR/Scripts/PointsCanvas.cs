
using UnityEngine;
using TMPro;
using System.Collections;

public class PointsCanvas : MonoBehaviour
{
    public TMP_Text PointsText;

    GameManager gameManager;

    private void Awake()
    {
        PointsText.text = "";

        gameManager = GameManager.Instance;
    }

    public IEnumerator Ok()
    {
        PointsText.text = "+1";
        yield return new WaitForSeconds(3f);
        PointsText.text = $"+1\n{gameManager.Ok}/{gameManager.Total}";
        yield return new WaitForSeconds(2f);
        PointsText.text = "";
    }

    public IEnumerator Fail()
    {
        yield break;
    }

}
