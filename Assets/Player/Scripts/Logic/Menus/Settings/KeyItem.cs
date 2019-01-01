using UnityEngine;
using TMPro;

public class KeyItem : MonoBehaviour
{
    private SettingsMenu settingsMenu;
    private InputKey inputKey;

    [Header("UI")]
    public TMP_Text inputNameTxt;
    public TMP_Text inputKeyTxt;

    public void Init (SettingsMenu settingsMenu, InputKey inputKey)
    {
        this.settingsMenu = settingsMenu;
        this.inputKey = inputKey;
        inputNameTxt.text = inputKey.name;
        inputKeyTxt.text = inputKey.keyCode.ToString();
    }
    
    public void ChangeInput ()
    {
        settingsMenu.ChangeKey(inputKey.name);
    }
}
