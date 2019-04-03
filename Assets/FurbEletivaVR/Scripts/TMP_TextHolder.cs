
using TMPro;
using UnityEngine;

public class TMP_TextHolder : MonoBehaviour
{
    public TMP_Text Text;

    private void Awake()
    {
        if (Text == null)
            Text = GetComponentInChildren<TMP_Text>();
    }
}
