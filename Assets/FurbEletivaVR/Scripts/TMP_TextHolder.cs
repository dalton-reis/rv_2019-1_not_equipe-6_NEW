
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TMP_TextHolder : MonoBehaviour
{
    public TMP_Text Text;
    public Image PanelImage;

    private void Awake()
    {
        if (Text == null)
            Text = GetComponentInChildren<TMP_Text>();

        if (PanelImage == null)
            PanelImage = GetComponentInChildren<Image>();
    }
}
