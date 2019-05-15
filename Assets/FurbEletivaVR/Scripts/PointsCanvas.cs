
using UnityEngine;
using TMPro;
using System.Collections;

public class PointsCanvas : MonoBehaviour
{
    public TMP_Text okText;
    public TMP_Text failText;

    private void Awake()
    {
        changeOkAndFail();
    }

    public IEnumerator Ok()
    {
        okText.text = "+1";
        yield return new WaitForSeconds(3f);
        changeOkAndFail($"+1\n{GameManager.Instance.Ok}", $"\n{GameManager.Instance.Fail}");
        yield return new WaitForSeconds(2f);
        changeOkAndFail();
    }

    public IEnumerator Fail()
    {
        failText.text = "+1";
        yield return new WaitForSeconds(3f);
        changeOkAndFail($"\n{GameManager.Instance.Ok}", $"+1\n{GameManager.Instance.Fail}");
        yield return new WaitForSeconds(2f);
        changeOkAndFail();
    }

    private void changeOkAndFail(string okStr = "", string failStr = "")
    {
        okText.text = okStr;
        failText.text = failStr;
    }

}
