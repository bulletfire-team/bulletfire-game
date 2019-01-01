using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleTxt : MonoBehaviour {

    public TMP_Text text;

    public void Change ()
    {
        text.text = (GetComponent<Toggle>().isOn) ? "Oui" : "Non";
    }
}
